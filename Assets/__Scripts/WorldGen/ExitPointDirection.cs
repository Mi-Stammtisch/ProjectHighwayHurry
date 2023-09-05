using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitPointDirection : MonoBehaviour
{
    [Tooltip("Drag the entryPoint in here")]
    [SerializeField] private GameObject entryPoint;
    [Tooltip("Drag the exitPoint in here")]
    [SerializeField] private GameObject exitPoint;

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        //draw arrow from exitPoint following the direction of the exitPoint
        //arrow is 10 units long
        //arrow has an arrow shape at the top
        //arrow is 3 unit wide
        Gizmos.DrawRay(exitPoint.transform.position, exitPoint.transform.forward * 10);
        Gizmos.DrawRay(exitPoint.transform.position + exitPoint.transform.forward * 10, (exitPoint.transform.forward * -1) + (exitPoint.transform.right * 0.5f));
        Gizmos.DrawRay(exitPoint.transform.position + exitPoint.transform.forward * 10, (exitPoint.transform.forward * -1) + (exitPoint.transform.right * -0.5f));

        //red arrow at the entry point just like at the exit point
        Gizmos.DrawRay(entryPoint.transform.position, entryPoint.transform.forward * 10);
        Gizmos.DrawRay(entryPoint.transform.position + entryPoint.transform.forward * 10, (entryPoint.transform.forward * -1) + (entryPoint.transform.right * 0.5f));
        Gizmos.DrawRay(entryPoint.transform.position + entryPoint.transform.forward * 10, (entryPoint.transform.forward * -1) + (entryPoint.transform.right * -0.5f));
        
        //Debug.Log("exitPointRotation in degrees: " + exitPoint.transform.rotation.eulerAngles);
    }

    public GameObject getEntryPoint() {
        return entryPoint;
    }

    public GameObject getExitPoint() {
        return exitPoint;
    }
}
