using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroySelfOnTriggerEnter : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        //if the other object is the player
        if (other.CompareTag("Player"))
        {
            //destroy this object
            //Debug.Log("Collected Coin");
            Destroy(gameObject);
        }
    }
}
