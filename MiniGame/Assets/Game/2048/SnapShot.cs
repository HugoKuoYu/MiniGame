using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace undoSystem
{
    [System.Serializable]
    public class TileSnapShot
    {
        public int number;
        public int x, y;
    }

    [System.Serializable]
    public class GameSnapShot
    {
        public List<TileSnapShot> tilestates;
        public int score;
    }
    public class TileBoardUndoManager
    { 
        private Stack<GameSnapShot> undoStack = new();
        private Stack<GameSnapShot> redoStack = new();

        private readonly TileBoard tileBoard;
        private readonly UI_Game2048 gameManager2048;
        public bool isInitialising { get; set; } = false;
        public TileBoardUndoManager(TileBoard tileBoard, UI_Game2048 gameManager2048)
        { 
            this.tileBoard = tileBoard;
            this.gameManager2048 = gameManager2048;
        }
        public void SaveSnapShot()
        {
            if (isInitialising == true) return;
            GameSnapShot snapShot = CreateSnapShot();
            undoStack.Push(snapShot);
            redoStack.Clear();
        }
        public void ClearSnapShots()
        { 
            undoStack.Clear();
            redoStack.Clear();
        }
        public void Undo()
        {
            if (undoStack.Count == 0)
            {
                Debug.Log("沒有可上一步的動作");
                return;
            }
            GameSnapShot current = CreateSnapShot();
            redoStack.Push(current);

            GameSnapShot undo = undoStack.Pop();
            RestoreSnapShot(undo);
        }

        public void Redo()
        {
            Debug.Log("Redo");
            if (redoStack.Count == 0)
            {
                Debug.Log("沒有可下一步的動作");
                return;
            }
            GameSnapShot current = CreateSnapShot();
            undoStack.Push(current);

            GameSnapShot redo = redoStack.Pop();
            RestoreSnapShot(redo);
        }

        public bool CanUndo() => undoStack.Count > 0;
        public bool CanRedo() => redoStack.Count > 0;

        public GameSnapShot CreateSnapShot()
        {
            GameSnapShot snapShot = new()
            {
                score = gameManager2048.score,
                tilestates = new List<TileSnapShot>()
            };
            foreach (var tile in tileBoard.tiles)
            {
                snapShot.tilestates.Add(new TileSnapShot
                {
                    number = tile.number,
                    x = tile.tilecell.coordinates.x,
                    y = tile.tilecell.coordinates.y,
                });
            }
            return snapShot;
        }
        public void RestoreSnapShot(GameSnapShot snapShot)
        {
            tileBoard.ClearBoard();
            foreach (var snap in snapShot.tilestates)
            {
                var tile = Object.Instantiate(tileBoard.tilePrefab, tileBoard.tileGrid.transform);
                var state = tileBoard.GetTileStateByNumber(snap.number);
                tile.setState(state, snap.number);

                var cell = tileBoard.tileGrid.GetCell(snap.x, snap.y);
                tile.Spawn(cell);
                tileBoard.tiles.Add(tile);
            }
            gameManager2048.SetScoreExternally(snapShot.score);
        }
    }


}