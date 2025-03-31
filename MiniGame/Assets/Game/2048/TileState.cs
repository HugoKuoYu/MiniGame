using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEditor;

[CreateAssetMenu(fileName = "Tile State",menuName = "ScriptableObjects/TileStates")]
public class TileState : ScriptableObject
{
    public Color backgroundColor;
    public Color textColor;
}
