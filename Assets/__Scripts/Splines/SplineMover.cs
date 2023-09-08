using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplineMover : MonoBehaviour
{
    public Spline spline;
    public float speed = 1;
    public float angle = 0;
    float distance = 0;


    

    
    private void Update()
    {
        if (spline == null) return;
        if (spline.NodeCount() < 2) return;

        transform.position = spline.GetPointInDistance(distance);
        distance += speed * Time.deltaTime;
        
        if (distance > spline.Length())
        {
            distance = 0;
        }
        

        
    }


}
