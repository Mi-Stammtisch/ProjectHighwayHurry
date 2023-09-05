using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerForceReciver : MonoBehaviour
{
    //Instance
    public static PlayerForceReciver Instance;

    
    private void Awake()
    {
        //Set Instance
        Instance = this;
    }

    public void AddForce(Vector3 force)
    {
        //Add force to player
        GetComponent<Rigidbody>().AddForce(force, ForceMode.Impulse);
    }

}
