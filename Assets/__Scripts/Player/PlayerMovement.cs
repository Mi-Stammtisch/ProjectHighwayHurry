using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

using PathCreation;
using System.IO;



public class PlayerMovement : MonoBehaviour
{
    [Header("Player Path Interaction")]

    private CustomSpline MiddlePath;
    private CustomSpline LeftPath;
    private CustomSpline RightPath;

    [SerializeField] PathBatch CurrentPathBatch;

    [SerializeField] PathCreator Currentpath;

    public enum PathType { Left, Right, Middle }
    public PathType currentPathType = PathType.Middle;






    public float PlayerConstantSpeed = 15f;



    [Header("Player Strave Interaction")]
    [SerializeField] float PlayerStraveSpeed = 30f;
    [SerializeField] float PlayerStraveAcceleration = 10f;
    [SerializeField] float PlayerSraveTilt = 15f;
    [SerializeField] private float switchPathDistance = 5f; // at what distance from the end of the path should the player switch to left or right path
    [SerializeField] private bool canStraveSwitchPath = true; //can the player switch paths
    [SerializeField] private float MaxStraveDistance = 5f;
    [SerializeField] public float straveDistanceTravelled;
    [SerializeField] public float overShootRecoverSpeed = 0.5f;

    private float currentStraveSpeed;




    [Range(-1, 1f)]
    [SerializeField] private float ChangeToForWardPatjDistance = 1f;




    [SerializeField] private float wheleSpinSpeed = 100f;
    [SerializeField] private float wheleRadius = 1f;
    [SerializeField] GameObject wheleFront;
    [SerializeField] GameObject wheleBack;





    [SerializeField] float distanceTravelled;
    [SerializeField] GameObject middleLaneCamRef;

    private GameObject playerModel;

    SpawnTileV2 spawnTileV2;

    


    private Vector3 moveDirection;
    public void OnStrave(InputValue value)
    {
        moveDirection = value.Get<Vector2>();
    }


    private void OnDrawGizmosSelected()
    {
        if (wheleFront == null || wheleBack == null)
        {
            Debug.LogError("wheleFront or wheleBack is null");
            return;
        }
        //drawCircle around wheleFront
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(wheleFront.transform.position, wheleRadius);

        //drawCircle around wheleBack
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(wheleBack.transform.position, wheleRadius);
    }

    IEnumerator Start()
    {



        playerModel = transform.GetChild(0).gameObject;
        playerModel.transform.localPosition = Vector3.zero;

        spawnTileV2 = SpawnTileV2.Instance;


        //Debug Time to wait for SpawnTileV2 to spawn tiles
        float time = Time.time;
        Debug.Log("Waiting for SpawnTileV2 to spawn tiles");

        yield return new WaitForSeconds(0.1f);
        Debug.Log("SpawnTileV2 finished spawning tiles after " + (Time.time - time) + " seconds");


        if (spawnTileV2.tiles[1] != null)
        {
            GameObject tile = spawnTileV2.tiles[1];
            MiddlePath = tile.GetComponent<ExitPointDirection>().middleSpline;
            LeftPath = tile.GetComponent<ExitPointDirection>().leftSpline;
            RightPath = tile.GetComponent<ExitPointDirection>().rightSpline;

            currentPathType = PathType.Middle;
            CurrentPathBatch = new PathBatch(LeftPath.spline, RightPath.spline, MiddlePath.spline);


            Currentpath = MiddlePath.spline;

            //distanceTravelled = Vector3.Magnitude(transform.position - pathCreator.path.GetPointAtDistance(0f));
            //Debug.Log("pathLength: " + pathCreator.path.length);

            //transform.position = pathCreator.path.GetPointAtDistance(distanceTravelled);
            //transform.position += new Vector3(0, 0.5f, 0);


        }

        //middleLaneCamRef.transform.parent = null;


        StartCoroutine(WheleSpinner());


    }


    IEnumerator WheleSpinner()
    {
        while (true)
        {
            //calculate new rotation speed based on player speed and wheleRadius
            float newRotationSpeed = PlayerConstantSpeed / (2 * Mathf.PI * wheleRadius);
            wheleFront.transform.Rotate(new Vector3(newRotationSpeed * Time.deltaTime * wheleSpinSpeed, 0, 0));
            wheleBack.transform.Rotate(new Vector3(newRotationSpeed * Time.deltaTime * wheleSpinSpeed, 0, 0));
            yield return null;
        }
    }

    private void Update()
    {
        if (Currentpath == null)
        {
            return;
        }

        if (middleLaneCamRef != null)
        {
            middleLaneCamRef.transform.position = CurrentPathBatch.middlePath.path.GetPointAtDistance(Currentpath.path.GetClosestDistanceAlongPath(transform.position));
            middleLaneCamRef.transform.rotation = CurrentPathBatch.middlePath.path.GetRotationAtDistance(Currentpath.path.GetClosestDistanceAlongPath(transform.position));

        }

        else Debug.LogError("middleLaneCamRef is null");

        //Forward Movement
        distanceTravelled += PlayerConstantSpeed * Time.deltaTime;
        transform.position = Currentpath.path.GetPointAtDistance(distanceTravelled);
        transform.rotation = Currentpath.path.GetRotationAtDistance(distanceTravelled);

        //Left and Right Strave
        if (moveDirection.x != 0)
        {
            //calculate new strave speed increase with acceleration
            currentStraveSpeed += PlayerStraveAcceleration * Time.deltaTime * moveDirection.x;
            currentStraveSpeed = Mathf.Clamp(currentStraveSpeed, -PlayerStraveSpeed, PlayerStraveSpeed);



            /*
             //move player model left and right smooth clamp to max distance
            straveDistanceTravelled += PlayerStraveSpeed * Time.deltaTime * moveDirection.x;
            straveDistanceTravelled = Mathf.Clamp(straveDistanceTravelled, -MaxStraveDistance, MaxStraveDistance);
            Vector3 thisPlayerModelPosition = playerModel.transform.localPosition;
            playerModel.transform.localPosition = new Vector3(straveDistanceTravelled, thisPlayerModelPosition.y, thisPlayerModelPosition.z);

            */

            //tilt player model in direction of movedirection.x 
            playerModel.transform.localRotation = Quaternion.Lerp(playerModel.transform.localRotation, Quaternion.Euler(0, 0, -moveDirection.x * PlayerSraveTilt), Time.deltaTime * 5f);
        }
        else
        {
            //decrease strave speed
            currentStraveSpeed = Mathf.Lerp(currentStraveSpeed, 0f, Time.deltaTime * overShootRecoverSpeed);
            //undo tilt when not straving            
            playerModel.transform.localRotation = Quaternion.Lerp(playerModel.transform.localRotation, Quaternion.identity, Time.deltaTime * 5f);
        }
        //move player model left and right smooth clamp to max distance
        straveDistanceTravelled += currentStraveSpeed * Time.deltaTime;
        straveDistanceTravelled = Mathf.Clamp(straveDistanceTravelled, -MaxStraveDistance, MaxStraveDistance);
        Vector3 thisPlayerModelPosition = playerModel.transform.localPosition;
        playerModel.transform.localPosition = new Vector3(straveDistanceTravelled, thisPlayerModelPosition.y, thisPlayerModelPosition.z);


        //Check if straveDistanceTravelled is greater than switchPathDistance
        if (canStraveSwitchPath)
        {


            switch (currentPathType)
            {
                case PathType.Left:
                    if (straveDistanceTravelled >= switchPathDistance)
                    {
                        SwitchToMiddlePath();
                    }
                    break;
                case PathType.Right:
                    if (straveDistanceTravelled <= -switchPathDistance)
                    {
                        SwitchToMiddlePath();
                    }
                    break;
                case PathType.Middle:
                    if (straveDistanceTravelled >= switchPathDistance)
                    {
                        SwitchToRightPath();
                    }
                    else if (straveDistanceTravelled <= -switchPathDistance)
                    {
                        SwitchToLeftPath();
                    }
                    break;

            }

        }







        //check if path is near end
        if (distanceTravelled >= Currentpath.path.length - ChangeToForWardPatjDistance)
        {

            //Debug.LogError($"Path is near end, currentPathType: {currentPathType}");
            if (MiddlePath.next() != null && MiddlePath.next().isNull() == false)
            {
                MiddlePath = MiddlePath.next();
                LeftPath = LeftPath.next();
                RightPath = RightPath.next();


                CurrentPathBatch = new PathBatch(LeftPath.spline, RightPath.spline, MiddlePath.spline);

                switch (currentPathType)
                {
                    case PathType.Left:
                        Currentpath = CurrentPathBatch.leftPath;
                        break;
                    case PathType.Right:
                        Currentpath = CurrentPathBatch.rightPath;
                        break;
                    case PathType.Middle:
                        Currentpath = CurrentPathBatch.middlePath;
                        break;
                }
                distanceTravelled = 0f;

            }
            else
            {
                Debug.Log("Jetzt hat es alles auseinander genommen, du Hurensohn");
            }

        }

    }

    public IEnumerator Hop(List<Vector3> points)
    {
        //follow path with playermodel.transform.position.y
        Debug.Log("Hop");

        List<Vector3> pointsOnParabola = points;

        Vector3 closestPoint = pointsOnParabola[0];
            float closestDistance = Vector3.Distance(playerModel.transform.position, closestPoint);

        while (closestDistance < 5f)
        {
            //find closest point in list to playermodel.transform.position
            float tempDistance = Vector3.Distance(playerModel.transform.position, closestPoint);
            foreach (Vector3 point in pointsOnParabola)
            {
                float distance = Vector3.Distance(playerModel.transform.position, point);
                if (distance < tempDistance)
                {
                    tempDistance = distance;
                    closestPoint = point;
                    closestDistance = distance;
                }
            }

            
            

            
            foreach (Vector3 point in pointsOnParabola)
            {
                float distance = Vector3.Distance(playerModel.transform.position, point);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestPoint = point;
                }
            }

            //move player model y position to closest point on parabola
            Vector3 thisPlayerModelPosition = playerModel.transform.position;
            playerModel.transform.position = new Vector3(thisPlayerModelPosition.x, closestPoint.y, thisPlayerModelPosition.z);

            
            yield return null;


        }


        
        //while playermodel.transform.position.y > 0f move playermodel.transform.position.y down
        while (playerModel.transform.position.y > 0f)
        {
            Vector3 thisPlayerModelPosition = playerModel.transform.position;
            playerModel.transform.position = new Vector3(thisPlayerModelPosition.x, thisPlayerModelPosition.y - 0.1f, thisPlayerModelPosition.z);
            yield return null;
        }


        Debug.Log("Hop finished");    
        

    }

    

    private void SwitchToLeftPath()
    {
        Vector3 PlayerModelWorldPosition = playerModel.transform.position;

        float leftRightStraveFix = -1f;
        if (currentPathType == PathType.Middle)
        {
            leftRightStraveFix = 1f;
        }


        distanceTravelled = FindClosesdDistancePoint(CurrentPathBatch.leftPath);
        Currentpath = CurrentPathBatch.leftPath;

        straveDistanceTravelled = leftRightStraveFix * GetStraveDistanceTravelled(PlayerModelWorldPosition, Currentpath.path.GetPointAtDistance(distanceTravelled));
        playerModel.transform.position = PlayerModelWorldPosition;

        currentPathType = PathType.Left;

    }

    private void SwitchToRightPath()
    {
        Vector3 PlayerModelWorldPosition = playerModel.transform.position;

        float leftRightStraveFix = 1f;
        if (currentPathType == PathType.Middle)
        {
            leftRightStraveFix = -1f;
        }

        distanceTravelled = FindClosesdDistancePoint(CurrentPathBatch.rightPath);
        Currentpath = CurrentPathBatch.rightPath;

        straveDistanceTravelled = leftRightStraveFix * GetStraveDistanceTravelled(PlayerModelWorldPosition, Currentpath.path.GetPointAtDistance(distanceTravelled));
        playerModel.transform.position = PlayerModelWorldPosition;

        currentPathType = PathType.Right;
    }

    private void SwitchToMiddlePath()
    {
        Vector3 PlayerModelWorldPosition = playerModel.transform.position;

        float leftRightStraveFix = -1f;
        if (currentPathType == PathType.Right)
        {
            leftRightStraveFix = 1f;
        }

        distanceTravelled = FindClosesdDistancePoint(CurrentPathBatch.middlePath);
        Currentpath = CurrentPathBatch.middlePath;
        straveDistanceTravelled = leftRightStraveFix * GetStraveDistanceTravelled(PlayerModelWorldPosition, Currentpath.path.GetPointAtDistance(distanceTravelled));
        playerModel.transform.position = PlayerModelWorldPosition;

        currentPathType = PathType.Middle;
    }

    private float FindClosesdDistancePoint(PathCreator newPath)
    {
        return newPath.path.GetClosestDistanceAlongPath(Currentpath.path.GetPointAtDistance(distanceTravelled));
    }

    private float GetStraveDistanceTravelled(Vector3 PlayerModelWorldPosition, Vector3 NewAnkerPoint)
    {
        //return distance between playermodel and newankerpoint
        return Vector3.Distance(PlayerModelWorldPosition, NewAnkerPoint);
    }





    #region Environment MoveInteractions
    public void SetCanSwitchPath(bool canSwitch)
    {
        canStraveSwitchPath = canSwitch;
    }
    public void SetPathSwitchDistance(float distance = 5f)
    {
        switchPathDistance = distance;
    }
    public void SetMaxStraveDistance(float distance = 5f)
    {
        MaxStraveDistance = distance;
    }



    #endregion



}

[System.Serializable]
public class PathBatch
{
    public PathCreator leftPath;
    public PathCreator rightPath;
    public PathCreator middlePath;

    public PathBatch(PathCreator leftPath, PathCreator rightPath, PathCreator middlePath)
    {
        this.leftPath = leftPath;
        this.rightPath = rightPath;
        this.middlePath = middlePath;
    }
}
