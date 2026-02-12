using UnityEngine;
using System.Collections.Generic;

public class Lever : MonoBehaviour
{
    public PlayerController trapper;
    public List<Flamethrower> flamethrowers;

    public void EndLeverAnim()
    {
        trapper = GetTrapper();
        Animator animator = GetComponent<Animator>();
        animator.SetBool("pullingLever", false);
        trapper.SpriteRenderer.enabled = true;
        trapper.canControl = true;
       
    }

    public void TriggerLeverTrap()
    {
        trapper = GetTrapper();
        trapper.leverOnCooldown = true;
        foreach(Flamethrower flamethrower in flamethrowers)
        {
            flamethrower.ActivateFlamethrowers();
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


}
