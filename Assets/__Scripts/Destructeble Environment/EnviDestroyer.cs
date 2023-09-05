using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnviDestroyer : MonoBehaviour
{
    
    [SerializeField] bool DestroyEnv = true;

   
    private void OnTriggerEnter(Collider other)
    {
        if (DestroyEnv)
        {
            
            if (other.gameObject.CompareTag("Destructible"))
            {
                if(other.gameObject.GetComponent<Rigidbody>() == null)
                {
                    other.gameObject.AddComponent<Rigidbody>();
                }
                
                
                //bost force away from the player camera
                other.gameObject.GetComponent<Rigidbody>().AddForce((other.transform.position - Camera.main.transform.position) * 200f);
                
            }
        }
    }


    
    

   
}
