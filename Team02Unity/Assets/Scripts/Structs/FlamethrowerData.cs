using System;
using UnityEngine;

public enum FlamethrowerDirection { up, down, left, right }

[Serializable]
public struct FlamethrowerData
{
    public Vector3Int flamethrowerPos;
    public int flamethrowerRange;
    public FlamethrowerDirection flamethrowerDirection;
}
