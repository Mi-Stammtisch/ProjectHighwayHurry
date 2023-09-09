using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class DestroySelfOnTriggerEnter : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        //if the other object is the player
        if (other.CompareTag("Player"))
        {
            //destroy this object
            //Debug.Log("Collected Coin");
            Scoreboard.Instance.coinCollect();

            //load from the Resources folder
            GameObject nr = Instantiate(Resources.Load<GameObject>("Nr/FN").gameObject);
            nr.transform.position = new Vector3(transform.position.x, transform.position.y + 1, transform.position.z);
            nr.GetComponent<FlowtNr>().SetTextAndRotation("+1");

            Destroy(gameObject);
        }
    }
}
