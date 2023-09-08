using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplineMover : MonoBehaviour
{
    public Spline spline;
    public float speed = 1;
    
    float distance = 0;


    

    
    private void Update()
    {
        if (spline == null) return;
        if (spline.NodeCount() < 2) return;

        transform.position = spline.GetPointInDistance(distance);
        Vector3 rot = spline.GetRotationInDistance(distance);
        transform.rotation = Quaternion.Euler(rot);
        distance += speed * Time.deltaTime;
        
        if (distance > spline.Length())
        {
            distance = 0;
        }
        

        
    }


}
