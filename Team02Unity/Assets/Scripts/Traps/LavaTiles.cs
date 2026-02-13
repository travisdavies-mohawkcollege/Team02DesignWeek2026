using UnityEngine;
using UnityEngine.Tilemaps;

public class LavaTiles : MonoBehaviour
{
    public LavaTileData tileData;
    public Tilemap tilemap;
    public Tile lavaTile;
    public Tile lavaTile2;
    public GameObject lavaHurtBox;

    public void Start()
    {
        tilemap = transform.parent.GetComponent<Tilemap>();
        foreach(Vector3Int lavaPos in tileData.lavaTilePos)
        {
            tilemap.SetTile(lavaPos, lavaTile);
            Vector3Int cellPos = tilemap.WorldToCell(lavaPos);
            Instantiate(lavaHurtBox, tilemap.GetCellCenterWorld(cellPos), transform.rotation);
        }
    }
}
