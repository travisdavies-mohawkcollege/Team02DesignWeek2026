using JetBrains.Annotations;
using NUnit.Framework;
using UnityEngine;
using System;

public class Button : MonoBehaviour
{

    public PlayerController trapper;
    public Sprite buttonSprite;
    public PitTrap pitTrap;
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
        //trapper.trapperInteract = false;
        TriggerTrap();
    }

    public void TriggerTrap()
    {
        trapper = GetTrapper();
        trapper.buttonOnCooldown = true;
        pitTrap.ActivatePitTrap();
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
}
