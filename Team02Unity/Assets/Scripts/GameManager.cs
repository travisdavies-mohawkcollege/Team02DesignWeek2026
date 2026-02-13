using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using Unity.VisualScripting;
using JetBrains.Annotations;
using System.Transactions;
public class GameManager : MonoBehaviour
{
    public int currentRoom = 0;
    int playerIndex = 0;
    public List<PlayerPoint> playerPoints;
    public List<PlayerController> playerControllers;
    public List<GameObject> cameraPos;
    public Camera roomCam;
    public Camera texCam;
    public CameraPoints cameraPoints;
    public List<PlayerController> donePlayers;
    public bool gameStarted = false;
    public float roomStartTimerMax = 3f;
    public float roomStartTimer = 3f;
    public bool roomStart;


    public void Update()
    {
        if (!gameStarted) return;
        switch (AllPlayersDone())
        {
            case false:
                break;
            case true:
                NextRoom(currentRoom);
                break;
        }
        switch(roomStart)
        {
            case true:
                break;
            case false:
                NextRoomTimer();
                break;
        }
    }

    public void StartGame()
    {
        gameStarted = true;
        currentRoom = 1;
        NextRoom(currentRoom);
        Debug.Log("StartedGame");
    }

    public void NextRoom(int room)
    {
        Debug.Log("Next Room function");
        foreach (PlayerController player in playerControllers)
        {
            player.GoToNextRoom(currentRoom);
        }  
        Vector3 newCamPos = cameraPoints.GetCameraPos(currentRoom);
        roomCam.transform.position = newCamPos;
        texCam.transform.position = newCamPos;
        roomStart = false;
    }

    public bool AllPlayersDone()
    {     
        foreach (PlayerController playerController in playerControllers)
        {
            if(playerController.doneRoom)
            {
                donePlayers.Add(playerController);
            }
        }
        if (donePlayers.Count == playerControllers.Count - 1)
        {
            currentRoom++;
            donePlayers.Clear();
            return true;
        }
        else
        {
            donePlayers.Clear();
            return false;
        }
    }

    public void NextRoomTimer()
    {
        if(!roomStart)
        {
            roomStartTimer -= Time.deltaTime;
        }
        if(roomStartTimer < 0)
        {
            roomStart = true;
            roomStartTimer = 3f;
            foreach (PlayerController playerController in playerControllers)
            {
                playerController.canControl = true;
            }
        }
    }
}
