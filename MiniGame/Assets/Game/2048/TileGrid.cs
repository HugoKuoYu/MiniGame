using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.UIElements;

public class TileGrid : MonoBehaviour
{
    public TileRow[] tileRows { get;private set; }
    public TileCell[] tileCells { get; private set; }
    public int size => tileCells.Length;
    public int height => tileRows.Length;
    public int width => size / height;
    private void Awake()
    {
        tileRows = GetComponentsInChildren<TileRow>();
        tileCells = GetComponentsInChildren<TileCell>();
    }
    private void Start()
    {
        for (int y = 0; y < tileRows.Length; y++)
            for (int x = 0; x < tileRows[y].tileCells.Length; x++)
            {
                tileRows[y].tileCells[x].coordinates = new Vector2Int(x, y);
            }
    }
    public TileCell GetRandomEmptyCell()
    { 
        int index = Random.Range(0, tileCells.Length);
        int startingIndex = index;
        while(tileCells[index].occupied)
        { 
            index++;
            if(index >= tileCells.Length)
                index = 0;
            if (index == startingIndex)
                return null;
        }
        return tileCells[index];
    }

    public TileCell GetCell(int x,int y)
    {
        if (x >= 0 && x < width && y >= 0 && y < height)
            return tileRows[y].tileCells[x];
        else return null;
    }
    public TileCell GetCell(Vector2Int coordinates)
    { 
        return GetCell(coordinates.x,coordinates.y);
    }
    public TileCell GetAdjacentCell(TileCell tilecell,Vector2Int direction)
    {
        Vector2Int coordinates = tilecell.coordinates;
        coordinates.x += direction.x;
        coordinates.y -= direction.y;
        return GetCell(coordinates);
    }

}
