using UnityEngine;
using System;
public class PlayerPoint : MonoBehaviour
{
    public PlayPointData[] pointData;
    public GameManager gameManager;

    public void Awake()
    {
        gameManager = FindFirstObjectByType<GameManager>();
    }

    public Vector3 GetNextRoomPos(int roomNumber)
    {
        Vector3 pos = pointData[roomNumber].roomStartPos;
        return pos;
    }
}
