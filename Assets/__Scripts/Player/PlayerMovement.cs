using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

using PathCreation;
using System.IO;



public class PlayerMovement : MonoBehaviour
{
    [Header("Player Path Interaction")]

    [SerializeField] PathBatch CurrentPathBatch;

    [SerializeField] PathCreator Currentpath;

    public enum PathType { Left, Right, Middle }
    public PathType currentPathType = PathType.Middle;






    [SerializeField] float PlayerConstantSpeed = 15f; 
    


    [Header("Player Strave Interaction")]
    [SerializeField] float PlayerStraveSpeed = 15f;    
    [SerializeField] float PlayerSraveTilt = 15f;
    [SerializeField] private float switchPathDistance = 5f; // at what distance from the end of the path should the player switch to left or right path
    [SerializeField] private bool canStraveSwitchPath = true; //can the player switch paths
    [SerializeField] private float MaxStraveDistance = 5f;
    [SerializeField] public float straveDistanceTravelled;

    








    [SerializeField] float distanceTravelled;

    private GameObject playerModel;


    private Vector3 moveDirection;
    public void OnStrave(InputValue value)
    {
        moveDirection = value.Get<Vector2>();
    }


    private void Start()
    {
        playerModel = transform.GetChild(0).gameObject;
        playerModel.transform.localPosition = Vector3.zero;

    }


    private void Update()
    {
        //Forward Movement
        distanceTravelled += PlayerConstantSpeed * Time.deltaTime;
        transform.position = Currentpath.path.GetPointAtDistance(distanceTravelled);
        transform.rotation = Currentpath.path.GetRotationAtDistance(distanceTravelled);

        //Left and Right Strave
        if (moveDirection.x != 0)
        {
            //move player model left and right smooth clamp to max distance
            straveDistanceTravelled += PlayerStraveSpeed * Time.deltaTime * moveDirection.x;
            straveDistanceTravelled = Mathf.Clamp(straveDistanceTravelled, -MaxStraveDistance, MaxStraveDistance);
            Vector3 thisPlayerModelPosition = playerModel.transform.localPosition;
            playerModel.transform.localPosition = new Vector3(straveDistanceTravelled, thisPlayerModelPosition.y, thisPlayerModelPosition.z);



            //tilt player model in direction of movedirection.x 
            playerModel.transform.localRotation = Quaternion.Lerp(playerModel.transform.localRotation, Quaternion.Euler(0, 0, -moveDirection.x * PlayerSraveTilt), Time.deltaTime * 5f);
        }
        else
        {
            //undo tilt when not straving
            playerModel.transform.localRotation = Quaternion.Lerp(playerModel.transform.localRotation, Quaternion.identity, Time.deltaTime * 5f);
        }


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
        if (distanceTravelled >= Currentpath.path.length - switchPathDistance)
        {
            Debug.Log("Near End");

            distanceTravelled = 0f; //TODO: Implement Next Path Switching

        }
    }

    private void SwitchToLeftPath()
    {
        Vector3 PlayerModelWorldPosition = playerModel.transform.position;
        
        float leftRightStraveFix = -1f;
        if(currentPathType == PathType.Middle)
        {
            leftRightStraveFix = 1f;
        }
            

        distanceTravelled = FindClosesdDistancePoint(CurrentPathBatch.leftPath);
        Currentpath = CurrentPathBatch.leftPath;
        
        straveDistanceTravelled = leftRightStraveFix * GetStraveDistanceTravelled(PlayerModelWorldPosition,Currentpath.path.GetPointAtDistance(distanceTravelled));
        playerModel.transform.position = PlayerModelWorldPosition;

        currentPathType = PathType.Left;

    }

    private void SwitchToRightPath()
    {
        Vector3 PlayerModelWorldPosition = playerModel.transform.position;

        float leftRightStraveFix = 1f;
        if(currentPathType == PathType.Middle)
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
        if(currentPathType == PathType.Right)
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
