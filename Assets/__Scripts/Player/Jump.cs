using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Jump : MonoBehaviour
{
    

    
    [Range(0, 100)]
    [SerializeField] float RampLength;
    [Range(0, 90)]
    [SerializeField] float RampAngle; 
    [Range(50, 200)]
    [SerializeField] int resolution;
    [Range(0, 100)]
    [SerializeField] float parabolaLength;
    
    [Range(0, 100)]
    [SerializeField] float ObjectSpeed;

    [SerializeField] bool UsePlayerSpeed = true;



    private Vector3 RampEndPos()
    {
        //Return position in angle position transform.forward und angle RampAngle and length RampLength
        return transform.position + transform.right * RampLength + Vector3.up * Mathf.Tan(RampAngle * Mathf.Deg2Rad) * RampLength;    
    }

    


    private List<Vector3> PointsOnParabola()
    {
        //Return list of points on parabola starting at RampEndPos with RampAngle and ObjectSpeed in direction of transform.forward
        List<Vector3> points = new List<Vector3>();

        float g = Mathf.Abs(Physics.gravity.y);
        float angle = RampAngle * Mathf.Deg2Rad;
        float speed = ObjectSpeed;
        

    

        
        float stepSize =  parabolaLength / resolution;

        for (int i = 0; i < resolution; i++)
        {
            float stepDistance = i * stepSize;
            float yOffset = stepDistance * Mathf.Tan(angle) - ((g * stepDistance * stepDistance) / (2 * speed * speed * Mathf.Cos(angle) * Mathf.Cos(angle)));
            Vector3 point = RampEndPos() + transform.right * stepDistance + Vector3.up * yOffset;
            points.Add(point);
        }

        return points;
    }

    private List<Vector3> pointsOnRamp()
    {
        //Return list of points on ramp starting at transform.position with RampAngle and RampLength
        List<Vector3> points = new List<Vector3>();

        float angle = RampAngle * Mathf.Deg2Rad;
        float stepSize = RampLength / resolution;

        for (int i = 0; i < resolution; i++)
        {
            float stepDistance = i * stepSize;
            float yOffset = stepDistance * Mathf.Tan(angle);
            Vector3 point = transform.position + transform.right * stepDistance + Vector3.up * yOffset;
            points.Add(point);
        }

        return points;
    }

    private List<Vector3> MergedList()
    {
        //Return list of points on ramp and parabola
        List<Vector3> points = new List<Vector3>();

        points.AddRange(pointsOnRamp());
        points.AddRange(PointsOnParabola());

        return points;
    }


    
    private void OnDrawGizmos()
    {
        //Draw Line Between transform.position and RampEnd.position
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, RampEndPos());

        //calculate parabola points
        List<Vector3> points = PointsOnParabola();

        //Draw parabola points
        Gizmos.color = Color.green;
        foreach (Vector3 point in points)
        {
            Gizmos.DrawSphere(point, 0.1f);
        }

    }


    
    private void OnTriggerEnter(Collider other)
    {
        //if other is player
        if (other.gameObject.CompareTag("Player"))
        {
            //calculate parabola points
            List<Vector3> points = MergedList();

            //Try get component PlayerMovement

            if(other.gameObject.TryGetComponent<PlayerMovement>(out PlayerMovement playerMovement))
            {
                if (UsePlayerSpeed)
                {
                    ObjectSpeed = playerMovement.PlayerConstantStartingSpeed / 3f;
                }
                
                //Set playerMovement to parabola points

                StartCoroutine(playerMovement.Hop(points));
            }
            else
            {
                Debug.LogWarning("PlayerMovement not found");
            }
        }
        
    }

}
