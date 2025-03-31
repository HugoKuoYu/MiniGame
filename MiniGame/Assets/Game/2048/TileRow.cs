using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileRow : MonoBehaviour
{
    public TileCell[] tileCells { get; private set; }
    private void Awake()
    {
        tileCells = GetComponentsInChildren<TileCell>();
    }

}
