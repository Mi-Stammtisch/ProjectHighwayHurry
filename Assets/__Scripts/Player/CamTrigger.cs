using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamTrigger : MonoBehaviour
{
    [SerializeField] bool RandomCam= false;
    
    private void OnTriggerEnter(Collider other)
    {

            
            if (other.gameObject.tag == "Player" && RandomCam) {
            
                PlayerCamera.Instance.AktivateRandomCamera();               
                
            }
    }
}
