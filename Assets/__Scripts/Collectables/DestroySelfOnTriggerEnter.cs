using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class DestroySelfOnTriggerEnter : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        //if the other object is the player
        if (other.gameObject.name == "PlayerModel")
        {
            //destroy this object
            //Debug.Log("Collected Coin");
            Scoreboard.Instance.coinCollect();

            Destroy(gameObject);
        }
    }
}
