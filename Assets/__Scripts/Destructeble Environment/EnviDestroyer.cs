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
                Debug.Log("Destroying " + other.gameObject.name);
                if(other.gameObject.GetComponent<Rigidbody>() == null)
                {
                    other.gameObject.AddComponent<Rigidbody>();
                }
                
                
                //boost force away and upward from the player camera
                other.gameObject.GetComponent<Rigidbody>().AddForce(Vector3.up * 40 + (other.gameObject.transform.position - Camera.main.transform.position) * 4, ForceMode.Impulse);
                
                
            }
        }
    }


    
    

   
}
