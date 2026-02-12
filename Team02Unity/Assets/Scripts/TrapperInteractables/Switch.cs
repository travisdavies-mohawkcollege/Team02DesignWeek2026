using UnityEngine;
using System.Collections.Generic;

public class Switch : MonoBehaviour
{

    public PlayerController trapper;

    public List<ConveyerBelt> beltList;

    public void Start()
    {
      
    }

    public void EndSwitchAnim()
    {
        trapper = GetTrapper();
        Animator animator = GetComponent<Animator>();
        animator.SetBool("pullingSwitch", false);
        animator.SetBool("switchPulled", true);
        trapper.SpriteRenderer.enabled = true;
        trapper.canControl = true;
    }

    public void ToggleSwitchTraps()
    {
        trapper = GetTrapper();
        trapper.switchOnCooldown = true;
        Debug.Log("Toggled Switch Traps");
        foreach(var belt in beltList)
        {
            belt.SwitchBeltDirection();
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
