using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultCamMoveComposer : MonoBehaviour
{
    

    
    [System.Serializable]
    public struct Obj
    {
        public GameObject ComposerObj;
        [Range(1, 100)]
        public int Weight ;
    }
    [SerializeField] List<Obj> ComposerObjs;


    
    

    // Update is called once per frame
   
        
    private void LateUpdate()
    {
        //find average position of all objects in lane
        Vector3 avgPos = Vector3.zero;
        int totalWeight = 0;
        foreach (Obj obj in ComposerObjs)
        {
            avgPos += obj.ComposerObj.transform.position * obj.Weight;
            totalWeight += obj.Weight;
        }
        avgPos /= totalWeight;

        //move this object to average position
        transform.position = avgPos; 

        //look avrage forward direction of all objects in lane
        Vector3 avgForward = Vector3.zero;
        foreach (Obj obj in ComposerObjs)
        {
            avgForward += obj.ComposerObj.transform.forward * obj.Weight;
        }
        avgForward /= totalWeight;

        //look in average forward direction
        transform.forward = avgForward;
        
    }

    
    private void OnDrawGizmos()
    {
        //draw lines from this object to all objects in ComposerObjs
        
        
        

        //Draw yellow arrow from this object forward direction 1 unit
        Gizmos.color = Color.yellow;
        Gizmos.DrawRay(transform.position, transform.forward);


        
    }

}


