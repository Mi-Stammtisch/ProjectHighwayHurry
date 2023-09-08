using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartCarMovement : MonoBehaviour
{
    

    void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("Car")) {
            other.gameObject.GetComponent<ScuffedCarAI>().triggerStayOld();
        }
    }
}
