using System.Collections;
using System.Collections.Generic;
using PathCreation;
using UnityEngine;

public class MuddlePointMover : MonoBehaviour
{
    [SerializeField] GameObject middlePoint;
    private Vector3 targetPosition;
    [SerializeField] float speed = 0.1f;
    PathCreator pathCreator;

    

    IEnumerator Start()
    {
        pathCreator = GetComponent<PlayerMovement>().CurrentPathBatch.middlePath;
        while(true)
        {
            targetPosition = pathCreator.path.GetClosestPointOnPath(transform.position);

            
            middlePoint.transform.LeanMove(targetPosition, speed);

            yield return new WaitForSeconds(speed);
       }
    }
}
