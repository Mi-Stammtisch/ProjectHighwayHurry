using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnTiles : MonoBehaviour
{
    



    [SerializeField] private GameObject tile;
    [SerializeField] private int tileCount = 10;
    [SerializeField] private List<GameObject> tiles;




    public void spawnNewTile() {
        Transform exitPoint = tiles[tiles.Count - 1].transform;
        GameObject newTile = Instantiate(tile, new Vector3(exitPoint.transform.position.x + (tile.transform.GetChild(0).localScale.x * 2), exitPoint.transform.position.y, exitPoint.transform.position.z), exitPoint.rotation);
        newTile.transform.parent = transform;
        tiles.Add(newTile);
        if (tiles.Count > tileCount) {
            Destroy(tiles[0]);
            tiles.RemoveAt(0);
        }
    }


    void Start() {
        while (tiles.Count < tileCount) {
            spawnNewTile();
        }
    }



}
