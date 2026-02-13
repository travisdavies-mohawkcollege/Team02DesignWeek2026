using NUnit.Framework;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;

public class ConveyerBelt : MonoBehaviour
{
    public ConveyerBeltData beltData;
    public Tilemap tilemap;
    public GameObject beltPrefab;
    public Switch trapperSwitch;
    public List<GameObject> activeBelts = new List<GameObject>();
    public Vector3 beltRot;
    public Vector3 rot;
    public float timerMax = 3f;
    public float timer = 3f;
    public bool isSwapped = false;

    public void Start()
    {
        trapperSwitch = FindFirstObjectByType<Switch>();
        trapperSwitch.beltList.Add(this);
        tilemap = transform.parent.GetComponent<Tilemap>();
        switch (beltData.beltDirection)
        {
            case BeltDirection.up:
                beltRot.Set(0, 0, 0);
                break;
            case BeltDirection.down:
                beltRot.Set(0, 0, 180);
                break;
            case BeltDirection.left:
                beltRot.Set(0, 0, 90);
                break;
            case BeltDirection.right:
                beltRot.Set(0, 0, -90);
                break;

        }
        
        foreach(Vector3Int beltPos in beltData.beltPos)
        {
            tilemap.SetTile(beltPos, beltData.conveyerTile);
            Vector3Int cellPos = tilemap.WorldToCell(beltPos);
            GameObject belt = Instantiate(beltPrefab, tilemap.GetCellCenterWorld(cellPos), Quaternion.Euler(beltRot));
            activeBelts.Add(belt);
            belt.transform.SetParent(this.transform, true);
        }
        trapperSwitch = FindFirstObjectByType<Switch>();  
    }

    public void SwitchBeltDirection()
    {
        Debug.Log(activeBelts);
        Debug.Log("RotatedBelts");
        foreach(GameObject belt in activeBelts)
        {
            belt.transform.Rotate(0, 0, 180);
            }
        switch (beltData.beltDirection)
        {
            case BeltDirection.up:
                beltData.beltDirection = BeltDirection.down;
                break;
            case BeltDirection.down:
                beltData.beltDirection = BeltDirection.up;
                break;
            case BeltDirection.left:
                beltData.beltDirection = BeltDirection.right;
                break;
            case BeltDirection.right:
                beltData.beltDirection = BeltDirection.left;
                break;
            default:
                break;
        }
        isSwapped = true;
    }

    public void UndoSwitchBeltDirection()
    {
        Debug.Log(activeBelts);
        Debug.Log("RotatedBelts");
        foreach (GameObject belt in activeBelts)
        {
            belt.transform.Rotate(0, 0, 180);       
        }
        switch (beltData.beltDirection)
        {
            case BeltDirection.up:
                beltData.beltDirection = BeltDirection.down;
                break;
            case BeltDirection.down:
                beltData.beltDirection = BeltDirection.up;
                break;
            case BeltDirection.left:
                beltData.beltDirection = BeltDirection.right;
                break;
            case BeltDirection.right:
                beltData.beltDirection = BeltDirection.left;
                break;
            default:
                break;
        }
    }

    public Vector3 GetRot()
    {
        switch (beltData.beltDirection)
        {           
            case BeltDirection.up:
                rot.Set(0, 1, 0);
                return rot;                
            case BeltDirection.down:
                rot.Set(0, -1, 0);
                return rot;
            case BeltDirection.left:
                rot.Set(-1, 0, 0);
                return rot;
            case BeltDirection.right:
                rot.Set(1, 0, 0);
                return rot;
            default:
                beltRot.Set(0, 0, 0);
                return rot;
        }
    }

    public void Update()
    {
        if (!isSwapped) return;
        else if (isSwapped)
        {
            timer -= Time.deltaTime;
        }
        if (timer <= 0 && isSwapped)
        {
            Debug.Log("timer rotated belts");
            isSwapped = false;
            UndoSwitchBeltDirection();
            timer = timerMax;
            trapperSwitch.GetComponent<Animator>().SetBool("switchPulled", false);
        }
        
    }
}
