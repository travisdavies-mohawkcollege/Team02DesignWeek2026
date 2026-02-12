using NUnit.Framework;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;

public class Flamethrower : MonoBehaviour
{
    public FlamethrowerData flamethrowerData;
    public Tilemap tilemap;
    public Tile flamethrowerTile;
    public GameObject flamethrowerHurtBox;
    public Lever lever;
    public float timerMax = 5f;
    public float timer = 5f;
    public bool flamethrowerActive = false;
    public List<GameObject> activeFireBoxes = new List<GameObject>();

    public void Start()
    {
        tilemap = transform.parent.GetComponent<Tilemap>();
        tilemap.SetTile(flamethrowerData.flamethrowerPos, flamethrowerTile);
        lever = FindFirstObjectByType<Lever>();
        lever.flamethrowers.Add(this);
    }

    public void ActivateFlamethrowers()
    {
        switch (flamethrowerData.flamethrowerDirection)
        {
            case FlamethrowerDirection.up:
                for(int i = 1; i <= flamethrowerData.flamethrowerRange; i++)
                {
                    Vector3Int cellPos = flamethrowerData.flamethrowerPos;
                    cellPos.y += i;
                    Vector3Int flamePos = tilemap.WorldToCell(cellPos);
                    GameObject newFlameBox = Instantiate(flamethrowerHurtBox, tilemap.GetCellCenterWorld(flamePos), transform.rotation);
                    activeFireBoxes.Add(newFlameBox); 
                }
                break;
            case FlamethrowerDirection.down:
                for (int i = 1; i <= flamethrowerData.flamethrowerRange; i++)
                {
                    Vector3Int cellPos = flamethrowerData.flamethrowerPos;
                    cellPos.y -= i;
                    Vector3Int flamePos = tilemap.WorldToCell(cellPos);
                    GameObject newFlameBox = Instantiate(flamethrowerHurtBox, tilemap.GetCellCenterWorld(flamePos), transform.rotation);
                    activeFireBoxes.Add(newFlameBox);
                }
                break;
            case FlamethrowerDirection.right:
                for (int i = 1; i <= flamethrowerData.flamethrowerRange; i++)
                {
                    Vector3Int cellPos = flamethrowerData.flamethrowerPos;
                    cellPos.x += i;
                    Vector3Int flamePos = tilemap.WorldToCell(cellPos);
                    GameObject newFlameBox = Instantiate(flamethrowerHurtBox, tilemap.GetCellCenterWorld(flamePos), transform.rotation);
                    activeFireBoxes.Add(newFlameBox);
                }
                break;
            case FlamethrowerDirection.left:
                for (int i = 1; i <= flamethrowerData.flamethrowerRange; i++)
                {
                    Vector3Int cellPos = flamethrowerData.flamethrowerPos;
                    cellPos.x -= i;
                    Vector3Int flamePos = tilemap.WorldToCell(cellPos);
                    GameObject newFlameBox = Instantiate(flamethrowerHurtBox, tilemap.GetCellCenterWorld(flamePos), transform.rotation);
                    activeFireBoxes.Add(newFlameBox);
                }
                break;
        }
        flamethrowerActive = true;
    }

    public void DeactivateFlamethrowers()
    {
        foreach(GameObject flameBox in activeFireBoxes)
        {
            Destroy(flameBox);
        }
        activeFireBoxes.Clear();
    }

    public void Update()
    {
        if (flamethrowerActive)
        {
            timer -= Time.deltaTime;
        }
        if (timer <= 0)
        {
            flamethrowerActive = false;
            DeactivateFlamethrowers();
            timer = timerMax;
        }
    }
}
