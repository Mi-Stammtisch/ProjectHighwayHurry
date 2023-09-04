using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnTiles : MonoBehaviour
{
    
    //TODO: spawn next tile when player gets to close to the end...
    //TODO: unload tiles the player has passed...


    [SerializeField] private GameObject tile;
    [SerializeField] private int tileCount = 10;
    private List<GameObject> tiles;




    public void spawnNewTile() {
        Transform exitPoint = tiles[tiles.Count - 1].transform;
        GameObject newTile = Instantiate(tile, new Vector3(exitPoint.transform.position.x + tile.transform.GetChild(0).localScale.x, exitPoint.transform.position.y, exitPoint.transform.position.z), exitPoint.rotation);
        newTile.transform.parent = transform;
        tiles.Add(newTile);
        if (tiles.Count > tileCount) {
            Destroy(tiles[0]);
            tiles.RemoveAt(0);
        }
    }


    void Start() {
        tiles = new List<GameObject>();
        for (int i = 0; i < tileCount; i++) {
            GameObject newTile = Instantiate(tile, new Vector3(i * tile.transform.GetChild(0).localScale.x, 0, 0), Quaternion.identity);
            Debug.Log(new Vector3(i * tile.transform.GetChild(0).localScale.x, 0, 0));
            newTile.transform.parent = transform;
            tiles.Add(newTile);
        }
    }



}
