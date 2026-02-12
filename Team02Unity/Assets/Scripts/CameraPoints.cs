using NUnit.Framework;
using UnityEngine;

public class CameraPoints : MonoBehaviour
{
    public CameraPointData[] cameraPoints;

    public Vector3 GetCameraPos(int roomNumber)
    {
        Vector3 pos = cameraPoints[roomNumber].position;
        return pos;
    }
}