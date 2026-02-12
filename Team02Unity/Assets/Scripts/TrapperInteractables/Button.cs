using JetBrains.Annotations;
using NUnit.Framework;
using UnityEngine;
using System;
using System.Collections.Generic;

public class Button : MonoBehaviour
{

    public PlayerController trapper;
    public Sprite buttonSprite;
    public List<PitTrap> pitTraps;
    public List<CarLauncher> carLaunchers;

    //Traps
    //0 for start game. 
    public int activeTrap = 0;
    public List pitTrap;
    public void Start()
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = buttonSprite;
    }
    public void EndAnim()
    {
        trapper = GetTrapper();
        Animator animator = GetComponent<Animator>();
        animator.SetBool("pressingButton", false);
        trapper.SpriteRenderer.enabled = true;
        trapper.canControl = true;
        //trapper.trapperInteract = false;
        //TriggerTrap();
    }

    public void TriggerTrap()
    {
        trapper = GetTrapper();
        trapper.buttonOnCooldown = true;
        foreach (var pitTrap in pitTraps)
        {
            pitTrap.ActivatePitTrap();
        }
        foreach(var carLauncher in carLaunchers)
        {
            carLauncher.SpawnCar();
        }
    }
    public PlayerController GetTrapper()
    {
        PlayerController[] players = FindObjectsByType<PlayerController>(FindObjectsSortMode.None);
        foreach (PlayerController player in players)
        {
            if (player.isTrapper)
            {
                return player;
            }
        }
        return null;
    }

    public void SetTrap(int trapIndex)
    {

    }
}
