using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Overlays;
using UnityEngine;
using UnityEngine.AI;

public class TileBoard : MonoBehaviour
{
    public UI_Game2048 gameManager2048;
    public Tile tilePrefab;
    public TileGrid tileGrid;
    public TileState[] tilestates;
    private List<Tile> tiles;
    private bool isWaiting;
    private Vector2 mouseStartPosition;
    private Vector2 mouseEndPosition;
    private bool isDragging = false;
    [HideInInspector]public bool isInitialising = false;

    private void Awake()
    {
        tileGrid = GetComponentInChildren<TileGrid>();
        tiles = new List<Tile>(16);
    }
    private void Update()
    {
        if (!isWaiting)
        {
            HandleKeyBoardInput();
            HandleMouseInput();
        }

    }
    private void HandleKeyBoardInput()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
            MovebyDirection(Vector2Int.up);
        else if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
            MovebyDirection(Vector2Int.down);
        else if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
            MovebyDirection(Vector2Int.left);
        else if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
            MovebyDirection(Vector2Int.right);
    }
    private void HandleMouseInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            mouseStartPosition = Input.mousePosition;
            isDragging = true;
        }
        if (Input.GetMouseButtonUp(0) && isDragging) 
        {
            mouseEndPosition = Input.mousePosition;
            Vector2 dragVector = mouseEndPosition - mouseStartPosition;
            if (dragVector.magnitude < 20f) return;
            if (Mathf.Abs(dragVector.x) > Mathf.Abs(dragVector.y))
            {
                if (dragVector.x > 0) MovebyDirection(Vector2Int.right);
                else MovebyDirection(Vector2Int.left);
            }
            else
            {
                if (dragVector.y > 0) MovebyDirection(Vector2Int.up);
                else MovebyDirection(Vector2Int.down);
            }
            isDragging = false;
        }
    }
    private void MovebyDirection(Vector2Int direction)
    {
        if (direction == Vector2Int.up)
            MovingTile(Vector2Int.up, 0, 1, 1, 1);
        if (direction == Vector2Int.down)
            MovingTile(Vector2Int.down, 0, 1, tileGrid.height - 2, -1);
        if(direction == Vector2Int.left)
            MovingTile(Vector2Int.left, 1, 1, 0, 1);
        if (direction == Vector2Int.right)
            MovingTile(Vector2Int.right, tileGrid.width - 2, -1, 0, 1);
    }
    public void CreateTile()
    {
        Tile tile = Instantiate(tilePrefab,tileGrid.transform);
        tile.setState(tilestates[0],2);
        tile.Spawn(tileGrid.GetRandomEmptyCell());
        tiles.Add(tile);
    }
    public void ClearBoard()
    {
        foreach (var cell in tileGrid.tileCells)
        {
            cell.tile = null;
        }
        foreach (var tile in tiles)
        {
            Destroy(tile.gameObject);
        }
        tiles.Clear();
    }
    private void MovingTile(Vector2Int direction, int startX, int incrementX, int startY, int incrementY)
    {
        bool changed = false;
        for (int x = startX; x >= 0 && x < tileGrid.width; x += incrementX)
            for (int y = startY; y >= 0 && y < tileGrid.height; y += incrementY)
            {
                TileCell tileCell = tileGrid.GetCell(x, y);
                if (tileCell.occupied)
                {
                    changed |= CheckWillChange(tileCell.tile, direction);
                }
            }
        if (changed)
        {
            SaveSnapShot();
        }
        changed = false;
        for (int x = startX; x >= 0 && x < tileGrid.width; x += incrementX)
            for (int y = startY; y >= 0 && y < tileGrid.height; y += incrementY)
            {
                TileCell tileCell = tileGrid.GetCell(x, y);
                if (tileCell.occupied)
                {
                    changed |= MoveTiles(tileCell.tile, direction);
                }
            }

        if (changed)
        {
            StartCoroutine(WaitForChanges());
        }
    }
    private bool MoveTiles(Tile tile,Vector2Int direction)
    {
        TileCell newCell = null;
        TileCell adjacementCell = tileGrid.GetAdjacentCell(tile.tilecell,direction);
        while (adjacementCell != null)
        {
            if (adjacementCell.occupied)
            {
                //TODO Merging
                if (CanMerge(tile, adjacementCell.tile))
                {
                    Merge(tile, adjacementCell.tile);
                    return true;
                }

                break;
            }
            newCell = adjacementCell;
            adjacementCell = tileGrid.GetAdjacentCell(adjacementCell, direction);
        }
        if (newCell != null)
        { 
            tile.MoveTo(newCell);
            return true;
        }
        return false;
    }
    private bool CanMerge(Tile a,Tile b) => a.number == b.number && !b.isLocked;
    private void Merge(Tile a, Tile b)
    { 
        tiles.Remove(a);
        a.Merge(b.tilecell);

        int index = Mathf.Clamp(IndexOf(b.state) +1 , 0, tilestates.Length - 1);
        int number = b.number * 2;
        b.setState(tilestates[index],number);

        gameManager2048.IncreaseScore(number);
    }
    private int IndexOf(TileState tileState)
    {
        for (int i = 0; i < tilestates.Length; i++)
        {
            if (tileState == tilestates[i])
                return i;
        }
        return -1;
    }
    
    private IEnumerator WaitForChanges()
    { 
        isWaiting = true;
        yield return new WaitForSeconds(0.1f);
        isWaiting = false;

        foreach (var tile in tiles)
        { 
            tile.isLocked = false;
        }
        if (tiles.Count != tileGrid.size)
            CreateTile();
        if (checkForGameOver())
            gameManager2048.GameOver();



    }
    public bool checkForGameOver()
    {
        if (tiles.Count != tileGrid.size)
            return false;
        foreach (var tile in tiles)
        {
            TileCell up = tileGrid.GetAdjacentCell(tile.tilecell, Vector2Int.up);
            TileCell down = tileGrid.GetAdjacentCell(tile.tilecell, Vector2Int.down);
            TileCell left = tileGrid.GetAdjacentCell(tile.tilecell, Vector2Int.left);
            TileCell right = tileGrid.GetAdjacentCell(tile.tilecell, Vector2Int.right);
            if (up != null && CanMerge(tile, up.tile))
                return false;
            if (down != null && CanMerge(tile, down.tile))
                return false;
            if (left != null && CanMerge(tile, left.tile))
                return false;
            if (right != null && CanMerge(tile, right.tile))
                return false;
        }
        return true;
    }
    public Stack<GameSnapShot>  snapShotStack = new Stack<GameSnapShot>();
    public void SaveSnapShot() //儲存最後的棋盤狀態
    {
        if (isInitialising) return;
        GameSnapShot snapShot = new GameSnapShot()
        {
            score = gameManager2048.score,
            tilestates = new List<TileSnapShot>()
        };
        foreach (var tile in tiles)
        {
            Debug.Log(tile.tilecell.coordinates.ToString());
            snapShot.tilestates.Add(new TileSnapShot
            {
                number = tile.number,
                x = tile.tilecell.coordinates.x,
                y = tile.tilecell.coordinates.y
            });
        }
        snapShotStack.Push(snapShot);
    }
    public void ClearSnapShot()
    {
        snapShotStack.Clear();
    }
    public void Undo()
    {
        if (snapShotStack.Count == 0)
        {
            Debug.Log("沒有東西可以還原");
            return;
        }
        
        GameSnapShot snapShot = snapShotStack.Pop();
        ClearBoard();
        foreach (var snap in snapShot.tilestates)
        {
            Tile tile = Instantiate(tilePrefab,tileGrid.transform);
            TileState state = GetTileStateByNumber(snap.number);
            tile.setState(state, snap.number);

            TileCell tileCell = tileGrid.GetCell(snap.x,snap.y);
            tile.Spawn(tileCell);
            tiles.Add(tile);
        }
        gameManager2048.SetScoreExternally(snapShot.score);
    }
    private TileState GetTileStateByNumber(int number)
    {
        for (int i = 0; i < tilestates.Length; i++)
        { 
            if(number == Mathf.Pow(2,i+1))
                return tilestates[i];
        }
        return tilestates[tilestates.Length - 1];
    }
    private bool CheckWillChange(Tile tile, Vector2Int direction)
    {
        TileCell current = tile.tilecell;
        TileCell adjacement = tileGrid.GetAdjacentCell(current, direction);
        while (adjacement != null)
        {
            if (adjacement.occupied)
            {
                if (CanMerge(tile, adjacement.tile))
                    return true;
                break;
            }
            return true; // 發現空格就會移動
        }
        return false;
    }
}
