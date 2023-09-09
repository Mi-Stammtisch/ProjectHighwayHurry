using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathCamMover : MonoBehaviour
{
    private Vector3 startPosition;
    
    void Start()
    {
        startPosition = transform.position;
        //move cam forward 1
        Vector3 endPosition = startPosition + Vector3.right;
        LeanTween.move(gameObject, endPosition, 10f).setLoopPingPong().setEaseInOutSine();
       
    }


}
