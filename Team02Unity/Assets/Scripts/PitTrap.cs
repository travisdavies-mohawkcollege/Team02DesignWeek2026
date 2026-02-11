using UnityEngine;
using UnityEngine.Tilemaps;

public class PitTrap : MonoBehaviour
{
    public PitTrapData pittrap;
    public Tilemap tilemap;
    public Tile trapTile;
    public void ActivatePitTrap()
    {
        foreach (Vector3Int trapPos in pittrap.trappedTilePos)
        {
            tilemap.SetTile(trapPos, trapTile);
        }
    }
    public void LinkToButton()
    {

    }
}
