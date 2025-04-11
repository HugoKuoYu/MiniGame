using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

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
