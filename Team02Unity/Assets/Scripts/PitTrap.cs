using NUnit.Framework;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;

public class PitTrap : MonoBehaviour
{
    public PitTrapData pittrap;
    public Tilemap tilemap;
    public GameObject pitTrapHurtBox;
    public Button button;
    public float timerMax = 3f;
    public float timer = 3f;
    public bool isActive = false;
    public List<GameObject> activeHurtBoxes = new List<GameObject>();

    public void Start()
    {
        tilemap = transform.parent.GetComponent<Tilemap>();
        foreach (Vector3Int trapPos in pittrap.trappedTilePos)
        {
            tilemap.SetTile(trapPos, pittrap.trapDoor);
            Vector3Int cellPos = tilemap.WorldToCell(trapPos);
        }
        button = FindFirstObjectByType<Button>();
        button.pitTrap = this;
    }
    public void ActivatePitTrap()
    {
        if (isActive) return;
        foreach (Vector3Int trapPos in pittrap.trappedTilePos)
        {
            tilemap.SetTile(trapPos, pittrap.trapTile);
            Vector3Int cellPos = tilemap.WorldToCell(trapPos);
            GameObject activeHurtBox = Instantiate(pitTrapHurtBox, tilemap.GetCellCenterWorld(cellPos), transform.rotation);
            activeHurtBoxes.Add(activeHurtBox);
        }
        isActive = true;
    }

    public void DeactivateTrap()
    {
        foreach(GameObject hurtBox in activeHurtBoxes)
        {
            Destroy(hurtBox);
        }
        foreach (Vector3Int trapPos in pittrap.trappedTilePos)
        {
            tilemap.SetTile(trapPos, pittrap.trapDoor);
        }
        activeHurtBoxes.Clear();
    }

    public void Update()
    {
        if(isActive)
        {
            timer -= Time.deltaTime;
        }
        if(timer <= 0)
        {
            isActive = false;
            DeactivateTrap();
            timer = timerMax;
        }
    }
}
