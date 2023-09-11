using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sun : MonoBehaviour
{
   //Instance this
    public static Sun Instance;

    
    private void Awake()
    {
        //Set this to the instance
        Instance = this;
    }

    public void LightToggle(bool state = true)
    {
        //Toggle the light
        GetComponent<Light>().enabled = state;
        //Debug.Log("Light toggled");
    }
}
