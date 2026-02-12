using NUnit.Framework;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;

public class CarLauncher : MonoBehaviour
{
    public Tile carLauncherTile;
    public GameObject car;
    public List<GameObject> cars;
    public Button button;
    public CarLauncherData launcherData;
    public Tilemap tilemap;
    public GameObject activeSpawn;
    public float spawnPosX, spawnPosY;
    public Vector2 spawnPos;


    //todo: add vertical/horizontal car, create copy for arrow launcher
    public void Start()
    {
        tilemap = transform.parent.GetComponent<Tilemap>();
        button = FindFirstObjectByType<Button>();
        button.carLaunchers.Add(this);
        tilemap.SetTile(launcherData.carLauncherPos, carLauncherTile);
        this.transform.position = tilemap.CellToWorld(launcherData.carLauncherPos);
        switch (launcherData.carDirection)
        {
            case CarDirection.up:
                spawnPosX = transform.position.x;
                spawnPosY = transform.position.y + 1.5f;
                spawnPos.Set(spawnPosX, spawnPosY);
                break;
            case CarDirection.down:
                spawnPosX = transform.position.x;
                spawnPosY = transform.position.y - 1.5f;
                spawnPos.Set(spawnPosX, spawnPosY);
                break;
            case CarDirection.left:
                spawnPosX = transform.position.x - 1.5f;
                spawnPosY = transform.position.y;
                spawnPos.Set(spawnPosX, spawnPosY);
                break;
            case CarDirection.right:
                spawnPosX = transform.position.x + 1.5f;
                spawnPosY = transform.position.y;
                spawnPos.Set(spawnPosX, spawnPosY);
                break;
        }
    }

    public void SpawnCar()
    {
        Debug.Log("Spawning car!");
        GameObject newCar = Instantiate(car, spawnPos, transform.rotation);
        cars.Add(newCar);
        Car newCarScript = newCar.GetComponent<Car>();
        switch (launcherData.carDirection)
        {
            case CarDirection.up:
                newCarScript.SetDirection(CarDirection.up);
                break;
            case CarDirection.down:
                newCarScript.SetDirection(CarDirection.down);
                break;
            case CarDirection.left:
                newCarScript.SetDirection(CarDirection.left);
                break;
            case CarDirection.right:
                newCarScript.SetDirection(CarDirection.right);
                break;
        }
    }



}
