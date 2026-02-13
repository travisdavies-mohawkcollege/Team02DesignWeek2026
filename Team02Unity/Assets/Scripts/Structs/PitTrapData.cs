using System;
using UnityEngine;
using UnityEngine.Tilemaps;


[Serializable]
public struct PitTrapData
{
    public Vector3Int[] trappedTilePos;
    public Tile trapTile;
    public Tile trapDoor;
}
