using System;
using UnityEngine;
using UnityEngine.Tilemaps;

public enum BeltDirection {  up , down, left, right }

[Serializable]
public struct ConveyerBeltData
{
    public Tile conveyerTile;
    public Vector3Int[] beltPos;
    public BeltDirection beltDirection;
}

