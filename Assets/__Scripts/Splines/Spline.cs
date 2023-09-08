using System.Collections;
using System.Collections.Generic;
using UnityEditor.EditorTools;
using UnityEngine;
using Unity.Mathematics;

public class Spline : MonoBehaviour
{
    [SerializeField] public bool DrawGizmos = true;
    [SerializeField] public bool DrawNodes = true;
    [SerializeField] public bool DrawNodeWeights = true;
    [SerializeField] public bool DrawSplinePoints = true;
    [SerializeField] public bool DrawSplineNormals = true;
    [SerializeField] public bool DrawDirectPath = true;
    [SerializeField] public bool DrawSpline = true;


    [SerializeField] private bool AutoRenameNodes = true;
    [SerializeField] public float AngleWeightSize = 0.2f;
    [SerializeField] public float NodeSize = 0.3f;

    [Range(0.1f, 1)]
    [SerializeField] public float NormalLength = 0.5f;

    [Tooltip("How many points per 1 units of spline length")]

    [Range(6, 100)]
    [SerializeField] private int SplineResolution = 10;











    private void Start()
    {
        MakeSureAllChildrenHaveSplineNodeComponent();
    }

    private void OnDrawGizmos()
    {

        if (DrawGizmos)
        {
            if (DrawDirectPath)
            {
                //draw Line connecting all children starting from first child to last child
                Gizmos.color = Color.yellow;
                for (int i = 0; i < transform.childCount - 1; i++)
                {
                    Gizmos.DrawLine(transform.GetChild(i).position, transform.GetChild(i + 1).position);
                }
            }


            //draw spline            
            for (int i = 0; i < transform.childCount - 1; i++)
            {
                Gizmos.color = Color.gray;
                List<Vector3> splinePoints = SplinePoints(transform.GetChild(i).gameObject, transform.GetChild(i + 1).gameObject);
                for (int j = 0; j < splinePoints.Count - 1; j++)
                {
                    Gizmos.DrawLine(splinePoints[j], splinePoints[j + 1]);
                }

                //Draw line from last spline point to next node
                Gizmos.DrawLine(splinePoints[splinePoints.Count - 1], transform.GetChild(i + 1).position);

                

                //Draw small purple sphere at each spline point
                Gizmos.color = Color.magenta;
                foreach (Vector3 point in splinePoints)
                {
                    if (DrawSplinePoints)Gizmos.DrawSphere(point, 0.05f); 
                    
                    if (DrawSplineNormals)
                    {
                        Gizmos.color = Color.yellow;
                        Gizmos.DrawLine(point, point + GetpointInAnleDistance(transform.GetChild(i).GetComponent<SplineNode>().angle, NormalLength, transform.GetChild(i).right));
                    }
                }

            }

            MakeSureAllChildrenHaveSplineNodeComponent();

        }
        if (AutoRenameNodes) RenameChildren();

    }



    #region Public Methods
    public int NodeCount()
    {
        return transform.childCount;
    }
    public List<Vector3> SplinePoints(GameObject firstNode, GameObject secondNode)
    {
        List<Vector3> splinePoints = new List<Vector3>();
        float firstNodeForwardWeight = firstNode.GetComponent<SplineNode>().forwardWeight;
        float secondNodeBackwardWeight = secondNode.GetComponent<SplineNode>().backwardWeight;

        //get Vector3 positions first node in forward direction + forwardWeight units
        Vector3 firstNodePosition = firstNode.transform.position;
        Vector3 firstNodeForwardPosition = firstNodePosition + firstNode.transform.forward * firstNodeForwardWeight;

        //get Vector3 positions second node in backward direction + backwardWeight units        
        Vector3 secondNodePosition = secondNode.transform.position;
        Vector3 secondNodeBackwardPosition = secondNodePosition - secondNode.transform.forward * secondNodeBackwardWeight;

        int newSplineResolution = Mathf.RoundToInt(AproximateSplineSegmentLength(firstNode, secondNode) * SplineResolution);

        //return along 4 point bezier curve
        for (int i = 0; i < newSplineResolution; i++)
        {
            float t = (float)i / (float)newSplineResolution;
            Vector3 splinePoint = Vector3.Lerp(Vector3.Lerp(firstNodePosition, firstNodeForwardPosition, t), Vector3.Lerp(secondNodeBackwardPosition, secondNodePosition, t), t);
            splinePoints.Add(splinePoint);
        }
        return splinePoints;
    }

    private float AproximateSplineSegmentLength(GameObject firstNode, GameObject secondNode)
    {

        float firstNodeForwardWeight = firstNode.GetComponent<SplineNode>().forwardWeight;
        float secondNodeBackwardWeight = secondNode.GetComponent<SplineNode>().backwardWeight;

        //get Vector3 positions first node in forward direction + forwardWeight units
        Vector3 firstNodePosition = firstNode.transform.position;
        Vector3 firstNodeForwardPosition = firstNodePosition + firstNode.transform.forward * firstNodeForwardWeight;

        //get Vector3 positions second node in backward direction + backwardWeight units        
        Vector3 secondNodePosition = secondNode.transform.position;
        Vector3 secondNodeBackwardPosition = secondNodePosition - secondNode.transform.forward * secondNodeBackwardWeight;

        //return along 4 point bezier curve
        float splineSegmentLength = 0;
        for (int i = 0; i < SplineResolution; i++)
        {
            float t = (float)i / (float)SplineResolution;
            Vector3 splinePoint = Vector3.Lerp(Vector3.Lerp(firstNodePosition, firstNodeForwardPosition, t), Vector3.Lerp(secondNodeBackwardPosition, secondNodePosition, t), t);
            splineSegmentLength += Vector3.Distance(splinePoint, Vector3.Lerp(Vector3.Lerp(firstNodePosition, firstNodeForwardPosition, t + 0.01f), Vector3.Lerp(secondNodeBackwardPosition, secondNodePosition, t + 0.01f), t + 0.01f));
        }
        return splineSegmentLength;
    }

    public float Length()
    {
        float splineLength = 0;
        for (int i = 0; i < transform.childCount - 1; i++)
        {
            splineLength += AproximateSplineSegmentLength(transform.GetChild(i).gameObject, transform.GetChild(i + 1).gameObject);
        }
        return splineLength;
    }

    public Vector3 GetRotationInDistance(float t)
    {
        //return rotation on curve at distance t
        float distance = 0;
        for (int i = 0; i < transform.childCount - 1; i++)
        {
            float splineSegmentLength = AproximateSplineSegmentLength(transform.GetChild(i).gameObject, transform.GetChild(i + 1).gameObject);
            if (distance + splineSegmentLength > t)
            {
                float tInSegment = (t - distance) / splineSegmentLength;
                return Vector3.Lerp(transform.GetChild(i).eulerAngles, transform.GetChild(i + 1).eulerAngles, tInSegment);
            }
            else
            {
                distance += splineSegmentLength;
            }
        }
        return Vector3.zero;
    }

    public Vector3 GetPointInDistance(float t)
    {
        //return point on curve at distance t
        float distance = 0;
        for (int i = 0; i < transform.childCount - 1; i++)
        {
            float splineSegmentLength = AproximateSplineSegmentLength(transform.GetChild(i).gameObject, transform.GetChild(i + 1).gameObject);
            if (distance + splineSegmentLength > t)
            {
                float tInSegment = (t - distance) / splineSegmentLength;
                return Vector3.Lerp(Vector3.Lerp(transform.GetChild(i).position, transform.GetChild(i).position + transform.GetChild(i).forward * transform.GetChild(i).GetComponent<SplineNode>().forwardWeight, tInSegment), Vector3.Lerp(transform.GetChild(i + 1).position - transform.GetChild(i + 1).forward * transform.GetChild(i + 1).GetComponent<SplineNode>().backwardWeight, transform.GetChild(i + 1).position, tInSegment), tInSegment);
            }
            else
            {
                distance += splineSegmentLength;
            }
        }
        return Vector3.zero;
    }
    public float GetDistanceFromPoint(Vector3 point)
    {
        //find closest point on spline
        Vector3 closestPoint = GetClosestPointOnSpline(point);
        //Approximate distance from closest point to point
        float distance = Vector3.Distance(closestPoint, point);
        //find distance from start of spline to closest point
        float distanceFromStart = 0;
        for (int i = 0; i < transform.childCount - 1; i++)
        {
            float splineSegmentLength = AproximateSplineSegmentLength(transform.GetChild(i).gameObject, transform.GetChild(i + 1).gameObject);
            if (Vector3.Distance(transform.GetChild(i).position, closestPoint) < Vector3.Distance(transform.GetChild(i + 1).position, closestPoint))
            {
                distanceFromStart += Vector3.Distance(transform.GetChild(i).position, closestPoint);
                break;
            }
            else
            {
                distanceFromStart += splineSegmentLength;
            }
        }
        return distanceFromStart + distance;



    }


    public Vector3 GetClosestPointOnSpline(Vector3 point)
    {
        float closestDistance = Mathf.Infinity;
        Vector3 closestPoint = Vector3.zero;
        for (int i = 0; i < transform.childCount - 1; i++)
        {
            List<Vector3> splinePoints = SplinePoints(transform.GetChild(i).gameObject, transform.GetChild(i + 1).gameObject);
            foreach (Vector3 splinePoint in splinePoints)
            {
                float distance = Vector3.Distance(point, splinePoint);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestPoint = splinePoint;
                }
            }
        }
        return closestPoint;
    }

    public List<GameObject> GetSplinePoints()
    {
        List<GameObject> splinePoints = new List<GameObject>();
        foreach (Transform child in transform)
        {
            splinePoints.Add(child.gameObject);
        }
        return splinePoints;
    }


    #endregion






    private void RenameChildren()
    {

        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).name = gameObject.name + "/Node " + i;
        }

    }



    [EButton("Check for duplicate positions")]
    public void CheckIfChildrenAreinSamePosition()
    {
        for (int i = 0; i < transform.childCount - 1; i++)
        {
            if (transform.GetChild(i).position == transform.GetChild(i + 1).position)
            {
                if (transform.GetChild(i + 1).GetComponent<SplineNode>().forwardWeight == 1 && transform.GetChild(i + 1).GetComponent<SplineNode>().backwardWeight == 1)
                {
                    if (Application.isEditor)
                    {
                        DestroyImmediate(transform.GetChild(i + 1).gameObject);
                    }
                    else
                    {
                        Destroy(transform.GetChild(i + 1).gameObject);
                    }
                }
                else
                {
                    Debug.LogWarning("SplineNode " + transform.GetChild(i + 1).name + " is in the same position as " + transform.GetChild(i).name);
                }
            }
        }
    }



    private void MakeSureAllChildrenHaveSplineNodeComponent()
    {
        foreach (Transform child in transform)
        {
            if (child.GetComponent<SplineNode>() == null)
            {
                child.gameObject.AddComponent<SplineNode>();
            }
        }
    }

    private Vector3 GetpointInAnleDistance(float angle, float distance, Vector3 rightDir)
    {
        float newAngle = angle + 90;
       
        //return point in 2d space at angle and distance x and y axis
        float x = Mathf.Cos(newAngle * Mathf.Deg2Rad) * distance;
        float y = Mathf.Sin(newAngle * Mathf.Deg2Rad) * distance;

        return rightDir * x + Vector3.up * y;


        

       

    }

}
