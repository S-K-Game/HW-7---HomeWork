using UnityEngine;
using UnityEngine.Tilemaps;

/**
 * This component allows the player to move by clicking the arrow keys,
 * but only if the new position is on an allowed tile.
 */
public class KeyboardMoverByTile: KeyboardMover {
    [SerializeField] Tilemap tilemap = null;
    [SerializeField] AllowedTiles allowedTiles = null;


     //============ My change =============//

    [SerializeField] TileBase tileNeedToChange = null;
    [SerializeField] TileBase newTile = null;

    private TileBase TileOnPosition(Vector3 worldPosition) {
        Vector3Int cellPosition = tilemap.WorldToCell(worldPosition);
        return tilemap.GetTile(cellPosition);
    }

    void Update()  {
        Vector3 newPosition = NewPosition();
        TileBase tileOnNewPosition = TileOnPosition(newPosition);
        if (allowedTiles.Contain(tileOnNewPosition)) {
            transform.position = newPosition;
        }

        //============ My change =============//
        else if (!allowedTiles.Contain(tileOnNewPosition) && tileOnNewPosition.Equals(tileNeedToChange) &&
            ((Input.GetKey(KeyCode.LeftArrow) && Input.GetKeyDown(KeyCode.X)) || (Input.GetKeyDown(KeyCode.LeftArrow) && Input.GetKey(KeyCode.X))))
        {
            Debug.Log("-----------> ");
            tilemap.SetTile(tilemap.WorldToCell(newPosition), newTile);
        }


        else {
            Debug.Log("You cannot walk on " + tileOnNewPosition + "!");
        }
    }
}
