using UnityEngine;
using System.Collections.Generic;
//using Unity.VisualScripting.Antlr3.Runtime.Tree;
//using Unity.VisualScripting;
//using JetBrains.Annotations;
//using System.Transactions;
using TMPro;
using UnityEngine.UI;
using Unity.VisualScripting;
public class GameManager : MonoBehaviour
{
    public int currentRoom = 0;
    public List<PlayerPoint> playerPoints;
    public List<PlayerController> playerControllers;
    public List<GameObject> cameraPos;
    public SpriteRenderer first, second, third, fourth, fifth;
    public Camera roomCam;
    public Camera texCam;
    public Camera p1Cam;
    public CameraPoints cameraPoints;
    public List<PlayerController> donePlayers;
    public TMP_Text display1Text;
    public TMP_Text display2Text;
    public RawImage display1TextBox;
    public RawImage display2TextBox;
    public RawImage monitorDisplay;
    public bool gameStarted = false;
    public float roomStartTimerMax = 10f;
    public float roomStartTimer = 10f;
    public bool roomStart;
    public AudioSource musicSource;
    public AudioSource sfxSource;
    public AudioClip lobbyMusic;
    public AudioClip gameMusic;
    public AudioClip endGameMusic;

    public void Start()
    {
        musicSource.clip = lobbyMusic;
        musicSource.loop = true;
        musicSource.Play();
    }

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
        musicSource.Stop();
        musicSource.clip = gameMusic;
        musicSource.Play();
        Debug.Log("StartedGame");
    }

    public void NextRoom(int room)
    {
        Vector3 newCamPos = cameraPoints.GetCameraPos(currentRoom);
        roomCam.transform.position = newCamPos;
        texCam.transform.position = newCamPos;
        Debug.Log("Next Room function");
        foreach (PlayerController player in playerControllers)
        {
            if (currentRoom == 6)
            {
                p1Cam.transform.position = newCamPos;
                display1Text.enabled = false;
                display1TextBox.enabled = false;
                display2Text.enabled = false;
                display2TextBox.enabled = false;
                monitorDisplay.enabled = false;
                musicSource.Stop();
                musicSource.clip = endGameMusic;
                musicSource.Play();
                break;
            }
            else if (currentRoom > 6) break;
            player.GoToNextRoom(currentRoom);
        }  
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
        display1Text.gameObject.SetActive(true);
        display2Text.gameObject.SetActive(true);
        display1TextBox.gameObject.SetActive(true);
        display2TextBox.gameObject.SetActive(true);
        display2Text.SetText(roomStartTimer.ToString("F0"));
        display1Text.SetText("Try your traps! \n" + roomStartTimer.ToString("F0") + " seconds remaining!");
        if(!roomStart)
        {
            roomStartTimer -= Time.deltaTime;
        }
        if(roomStartTimer < 0)
        {
            roomStart = true;
            roomStartTimer = 10f;
            foreach (PlayerController playerController in playerControllers)
            {
                playerController.canControl = true;
                display1Text.gameObject.SetActive(false);
                display2Text.gameObject.SetActive(false);
                display1TextBox.gameObject.SetActive(false);
                display2TextBox.gameObject.SetActive(false);
            }
        }
    }

    public void SetWinnerColours()
    {

    }
}
