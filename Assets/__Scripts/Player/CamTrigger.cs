using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamTrigger : MonoBehaviour
{
    
    private void OnTriggerEnter(Collider other)
    {
            
            if (other.gameObject.tag == "Player") {
            
                PlayerCamera.Instance.AktivateRandomCamera();               
                
            }
    }
}
