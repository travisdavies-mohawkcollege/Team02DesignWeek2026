using System;
using UnityEngine;

public enum CarDirection {  up, down, left, right }

[Serializable]
public struct CarLauncherData
{
    public Vector3Int carLauncherPos;
    public CarDirection carDirection;
}

