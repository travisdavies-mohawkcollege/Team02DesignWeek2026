using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerSpawn : MonoBehaviour
{
    [field: SerializeField] public Transform[] SpawnPoints { get; private set; }
    public int PlayerCount { get; private set; }

    public void OnPlayerJoined(PlayerInput playerInput)
    {
        // Prevent adding in more than max number of players
        if (PlayerCount >= SpawnPoints.Length)
            return;

        // Assign spawn transform values
        playerInput.transform.position = SpawnPoints[PlayerCount].position;
        playerInput.transform.rotation = SpawnPoints[PlayerCount].rotation;

        // Increment player count
        PlayerCount++;

        //
        PlayerController playerController = playerInput.gameObject.GetComponent<PlayerController>();
        playerController.AssignPlayerNumber(PlayerCount);
        playerController.InitializePlayer(playerInput);

    }
    
}
