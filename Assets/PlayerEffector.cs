using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEffector : MonoBehaviour
{
   
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            //Add force to player
            PlayerForceReciver.Instance.AddForce(transform.up * 5f);
        }
    }
}
