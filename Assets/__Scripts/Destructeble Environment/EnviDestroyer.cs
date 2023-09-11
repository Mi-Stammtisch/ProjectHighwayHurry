using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnviDestroyer : MonoBehaviour
{
    
    [SerializeField] bool DestroyEnv = true;
    [SerializeField] private PlayerMovement playerMovement;

   
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
                
                
                //boost force away and upward from the player camera
                other.gameObject.GetComponent<Rigidbody>().AddForce(Vector3.up * playerMovement.currentPlayerSpeed + (other.gameObject.transform.position - Camera.main.transform.position) * (playerMovement.currentPlayerSpeed/10), ForceMode.Impulse);
                
                Scoreboard.Instance.destroyBonus(Scoreboard.Instance.scoreboardSettings.destroyBonusValue);
                
            }
        }
    }


    
    

   
}
