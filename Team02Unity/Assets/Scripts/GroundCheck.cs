using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    public PlayerController player;

    public void Start()
    {
        player = transform.parent.GetComponent<PlayerController>();
    }
    public void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Collided with " + collision.gameObject);
        if (collision.gameObject.CompareTag("PitTrap"))
        {
            if (player.grounded)
            {
                //Take Damage
                player.RunnerDie();
            }
        }
    }
}
