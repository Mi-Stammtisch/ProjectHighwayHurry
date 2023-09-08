using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplineNode : MonoBehaviour
{

    public float forwardWeight = 1;
    public float backwardWeight = 1;

    [Range(0, 360)]
    [SerializeField] public float angle = 0;

    Spline spline;

    private void Start()
    {
        spline = GetComponentInParent<Spline>();
    }



    private void OnDrawGizmos()
    {
        if (spline == null) spline = GetComponentInParent<Spline>();
        if (!spline.DrawGizmos) return;



        if (spline.DrawNodeWeights)
        {
            //daw smal blue sphere in forward direction + forwardWeight units
            Gizmos.color = Color.blue;
            Gizmos.DrawSphere(transform.position + transform.forward * forwardWeight, spline.AngleWeightSize);
            //draw line from this object to forward direction + forwardWeight units
            Gizmos.DrawLine(transform.position, transform.position + transform.forward * forwardWeight);

            //daw smal red sphere in backward direction + backwardWeight units
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(transform.position - transform.forward * backwardWeight, spline.AngleWeightSize);
            //draw line from this object to backward direction + backwardWeight units
            Gizmos.DrawLine(transform.position, transform.position - transform.forward * backwardWeight);
        }

        if (spline.DrawNodes)
        {
            //draw small green sphere at this object
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(transform.position, spline.NodeSize);
        }
    }

    public float Normal()
    {
        return angle;
    }

    
   

}
