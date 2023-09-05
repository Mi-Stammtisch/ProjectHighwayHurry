using System;
using System.Collections.Generic;
using UnityEngine;

public class SpawnTileV2 : MonoBehaviour
{
    [SerializeField] private TilePool tilePool;
    [SerializeField] private int tileCount = 5;
    [SerializeField] private List<GameObject> tiles;
    private List<GameObject> nextTiles = new List<GameObject>();




    private void spawnInitialTiles(GameObject tile) {
        GameObject newTile = Instantiate(tile, tiles[tiles.Count - 1].GetComponent<ExitPointDirection>().getExitPoint().transform.position, Quaternion.identity);
        GameObject exitPoint = tiles[tiles.Count - 1].GetComponent<ExitPointDirection>().getExitPoint();
        GameObject entryPoint = newTile.GetComponent<ExitPointDirection>().getEntryPoint();


        for (int i = 0; i <= 4; i++) {
            //
            newTile.transform.rotation = Quaternion.Euler(new Vector3(0, i * 90, 0));
            //Debug.Log("newTileRotation: " + newTile.transform.rotation.eulerAngles);
            //Debug.Log("---");
            //Debug.Log("entryPointRotation: " + newTile.GetComponent<ExitPointDirection>().getEntryPoint().transform.rotation.eulerAngles.y);
            //Debug.Log("exitPointRotation: " + exitPoint.transform.rotation.eulerAngles.y);
            if (Math.Abs(newTile.GetComponent<ExitPointDirection>().getEntryPoint().transform.rotation.eulerAngles.y - exitPoint.transform.rotation.eulerAngles.y) < 5f) {
                //Debug.Log("rotationFound: " + i * 90);
                break;
            }
        }
        //Debug.Log("--------------------");

        newTile.transform.parent = transform;
        tiles.Add(newTile);
        if (tiles.Count > tileCount) {
            Destroy(tiles[0], 1f);
            tiles.RemoveAt(0);
        }
    }


    void Start() {
        while (tiles.Count < tileCount) {
            spawnInitialTiles(tilePool.straightTiles[UnityEngine.Random.Range(0, tilePool.straightTiles.Count)]);
        }
    }


    public void spawnNewTile() {
        if (nextTiles.Count == 0) {
            float distanceToSpawn = Vector3.Magnitude(tiles[tiles.Count - 1].GetComponent<ExitPointDirection>().getExitPoint().transform.position);
            Debug.Log("distanceToSpawn: " + distanceToSpawn);
            
            if (distanceToSpawn <= 500) {
                spawnInitialTiles(tilePool.straightTiles[UnityEngine.Random.Range(0, tilePool.straightTiles.Count)]);
            }
            else {
                if (UnityEngine.Random.Range(0, 2) == 0) {
                    spawnInitialTiles(tilePool.leftTurnTiles[UnityEngine.Random.Range(0, tilePool.leftTurnTiles.Count)]);
                    nextTiles.Add(tilePool.leftTurnTiles[UnityEngine.Random.Range(0, tilePool.leftTurnTiles.Count)]);
                    nextTiles.Add(tilePool.straightTiles[UnityEngine.Random.Range(0, tilePool.straightTiles.Count)]);
                    nextTiles.Add(tilePool.straightTiles[UnityEngine.Random.Range(0, tilePool.straightTiles.Count)]);
                }
                else {
                    spawnInitialTiles(tilePool.rightTurnTiles[UnityEngine.Random.Range(0, tilePool.rightTurnTiles.Count)]);
                    nextTiles.Add(tilePool.rightTurnTiles[UnityEngine.Random.Range(0, tilePool.rightTurnTiles.Count)]);
                    nextTiles.Add(tilePool.straightTiles[UnityEngine.Random.Range(0, tilePool.straightTiles.Count)]);
                    nextTiles.Add(tilePool.straightTiles[UnityEngine.Random.Range(0, tilePool.straightTiles.Count)]);
                }
            }
        }
        else {
            spawnInitialTiles(nextTiles[0]);
            nextTiles.RemoveAt(0);
        }
        //spawnInitialTiles(tilePool.straightTiles[UnityEngine.Random.Range(0, tilePool.straightTiles.Count)]);
    }
}
