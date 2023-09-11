using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class DestroySelfOnTriggerEnter : MonoBehaviour
{

    [SerializeField] AudioClip CoinCollectSound;
    private void OnTriggerEnter(Collider other)
    {
        //if the other object is the player
        if (other.gameObject.name == "PlayerModel")
        {
            //destroy this object
            //Debug.Log("Collected Coin");
            SoundManager.Instance.PlaySound(CoinCollectSound);
            Scoreboard.Instance.coinCollect();

            Destroy(gameObject);
        }
    }
}
