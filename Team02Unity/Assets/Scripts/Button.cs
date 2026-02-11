using JetBrains.Annotations;
using NUnit.Framework;
using UnityEngine;

public class Button : MonoBehaviour
{

    public PlayerController trapper;
    public Sprite buttonSprite;
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
        trapper.trapperInteract = false;
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
