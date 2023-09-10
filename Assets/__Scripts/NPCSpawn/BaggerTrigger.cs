using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BaggerTrigger : MonoBehaviour
{

    public bool isTriggered = false;

    // Start is called before the first frame update
    void Start()
    {
        isTriggered = false;
    }


    public bool getTriggered()
    {
        return isTriggered;
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            isTriggered = true;
        }
    }
}
