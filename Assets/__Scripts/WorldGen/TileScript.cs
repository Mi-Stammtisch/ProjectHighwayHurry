using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileScript : MonoBehaviour
{
    private GameObject spawner;

    void Start() {
        spawner = GameObject.Find("TileSpawner");
    }

    void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag == "Player") {
            spawner.GetComponent<SpawnTileV2>().spawnNewTile();
        }
    }

    [EButton("Spawn Tile")]
    public void spawnTile() {
        spawner.GetComponent<SpawnTileV2>().spawnNewTile();
    }
}
