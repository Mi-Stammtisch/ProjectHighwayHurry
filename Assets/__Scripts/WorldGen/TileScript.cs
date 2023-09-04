using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileScript : MonoBehaviour
{
    private GameObject spanwer;

    void Start() {
        spanwer = transform.parent.parent.gameObject;
    }

    void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag == "Player") {
            spanwer.GetComponent<SpawnTiles>().spawnNewTile();
            Destroy(gameObject);
        }
    }
}
