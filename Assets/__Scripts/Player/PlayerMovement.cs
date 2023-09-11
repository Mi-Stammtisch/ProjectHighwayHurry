using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

using PathCreation;
using System.IO;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using System;
using Unity.VisualScripting;



public class PlayerMovement : MonoBehaviour
{
    [Header("Player Path Interaction")]

    private CustomSpline MiddlePath;
    private CustomSpline LeftPath;
    private CustomSpline RightPath;

    [SerializeField] public PathBatch CurrentPathBatch;

    [SerializeField] PathCreator Currentpath;

    public enum PathType { Left, Right, Middle }
    public PathType currentPathType = PathType.Middle;






    public float PlayerConstantStartingSpeed = 15f;
    public float PlayerConstantMaxSpeed = 300f;
    public float currentPlayerSpeed;
    [Range(1, 5)]
    [Tooltip("How much faster should the player be after 1000 meters in percent")]
    public float PlayerSpeedIncreaseOver100Meters = 1.1f;

    public Int64 TotaldistanceTravelledInMeters = 0;
    float currentMeter;

    public Int32 currentSpeedInceses = 0;



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
    [SerializeField] bool resetSraveSpeedOnMaxStraveDistance = true;




    [Range(-2, 1f)]
    [SerializeField] private float ChangeToForWardPatjDistance = 1f;





    [SerializeField] private float wheleSpinSpeed = 100f;
    [SerializeField] private float wheleRadius = 1f;
    [SerializeField] GameObject wheleFront;
    [SerializeField] GameObject wheleBack;

    [SerializeField] public List<GameObject> saveBeforeTeleport;
    private List<Vector3> saveBeforeTeleportPositions = new List<Vector3>();





    [SerializeField] float distanceTravelled;
    [SerializeField] GameObject middleLaneCamRef;

    private GameObject playerModel;

    SpawnTileV2 spawnTileV2;

    public bool tilesCached = false;
    public bool buildingsCached = false;


    //speed milestone variables
    [Header("ScoreboardShit")]
    [SerializeField] private ScoreboardSettings scoreboardSettings;
    bool hasMoreSpeedBoni = true;
    int speedBonusIndex = 0;

    
    private void PlayerDeath() 
    {
        currentPlayerSpeed = 0f;
        currentStraveSpeed = 0f;
        StopAllCoroutines();

    }

    private Vector3 moveDirection;
    public void OnStrave(InputValue value)
    {
        moveDirection = value.Get<Vector2>();
    }
    bool isStunting = false;
    public IEnumerator StuntTimer(float time)
    {
        isStunting = true;
        yield return new WaitForSeconds(time);
        isStunting = false;
        
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

    void Awake() {
        //SpawnTileV2.onTilesCached += () => { tilesCached = true; };
        //BuildingSpawner.onBuildingsCached += () => { buildingsCached = true; };
    }

    IEnumerator Start()
    {
        GameManager.PlayerDeath += PlayerDeath;

        playerModel = transform.GetChild(0).gameObject;
        playerModel.transform.localPosition = Vector3.zero;

        spawnTileV2 = SpawnTileV2.Instance;

        currentPlayerSpeed = PlayerConstantStartingSpeed;


        //Debug Time to wait for SpawnTileV2 to spawn tiles
        float time = Time.time;
        
        //wait unit onTileCached event is fired
        while (!tilesCached || !buildingsCached) {
            yield return null;
        }
       // yield return new WaitForSeconds(0.5f);
        
        if (spawnTileV2.tiles[2] != null)
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
        else {
            Debug.LogError("SpawnTileV2.tiles[1] is null");
        }

        //middleLaneCamRef.transform.parent = null;

        StartCoroutine(Updater());
        StartCoroutine(WheleSpinner());


    }

    void OnDestroy() 
    {
        GameManager.PlayerDeath -= PlayerDeath;
        
    }


    IEnumerator WheleSpinner()
    {
        while (true)
        {
            //calculate new rotation speed based on player speed and wheleRadius
            float newRotationSpeed = currentPlayerSpeed / (2 * Mathf.PI * wheleRadius);
            wheleFront.transform.Rotate(new Vector3(newRotationSpeed * Time.deltaTime * wheleSpinSpeed, 0, 0));
            wheleBack.transform.Rotate(new Vector3(newRotationSpeed * Time.deltaTime * wheleSpinSpeed, 0, 0));
            yield return null;
        }
    }

    IEnumerator Updater()
    {
        while (Currentpath == null)
        {
            yield return null;
        }

        GameManager.Instance.UpdateSpeed(currentPlayerSpeed);
        while (true)
        {
            Vector3 tempCurrentPosition = transform.position;
            Vector3 tempCurrentModelPosition = playerModel.transform.position;

            if (middleLaneCamRef != null)
            {
                middleLaneCamRef.transform.position = CurrentPathBatch.middlePath.path.GetPointAtDistance(CurrentPathBatch.middlePath.path.GetClosestDistanceAlongPath(transform.position));
                middleLaneCamRef.transform.rotation = CurrentPathBatch.middlePath.path.GetRotationAtDistance(CurrentPathBatch.middlePath.path.GetClosestDistanceAlongPath(transform.position));

                //float distanceToMiddleLane = Vector3.Distance(transform.position, CurrentPathBatch.middlePath.path.GetClosestPointOnPath(transform.position));
                //float distanceToMiddleLaneX = Mathf.Abs(CurrentPathBatch.middlePath.path.GetClosestPointOnPath(transform.position).x - transform.position.x);

                //offset middleLaneCamRef.transform.position.x by distanceToMiddleLaneX                
                //middleLaneCamRef.transform.position = new Vector3(transform.position.x + distanceToMiddleLaneX, transform.position.y, transform.position.z);
                
                
                
                //middleLaneCamRef.transform.rotation = CurrentPathBatch.middlePath.path.GetRotationAtDistance(Currentpath.path.GetClosestDistanceAlongPath(transform.position));
            }

            



            //Forward Movement
            distanceTravelled += currentPlayerSpeed * Time.deltaTime;
            transform.position = Currentpath.path.GetPointAtDistance(distanceTravelled);
            transform.rotation = Currentpath.path.GetRotationAtDistance(distanceTravelled);

            //update distanceTravelledInMeters
            currentMeter += currentPlayerSpeed * Time.deltaTime;

            if (currentMeter >= 1f)
            {
                TotaldistanceTravelledInMeters++;
                currentMeter -= 1f;

                //update player speed
                if (TotaldistanceTravelledInMeters / 100 > currentSpeedInceses)
                {
                    SoundManager.Instance.PlayAcellerateSound();
                    currentSpeedInceses++;
                    float boost = PlayerSpeedIncreaseOver100Meters * PlayerConstantStartingSpeed - PlayerConstantStartingSpeed;
                    currentPlayerSpeed += boost;
                    currentPlayerSpeed = Mathf.Clamp(currentPlayerSpeed, PlayerConstantStartingSpeed, PlayerConstantMaxSpeed);

                    GameManager.Instance.UpdateSpeed(currentPlayerSpeed);

                }
            }

            //Left and Right Strave
            if (moveDirection.x != 0 && !isStunting)
            {
                //calculate new strave speed increase with acceleration
                currentStraveSpeed += PlayerStraveAcceleration * Time.deltaTime * moveDirection.x;
                currentStraveSpeed = Mathf.Clamp(currentStraveSpeed, -PlayerStraveSpeed, PlayerStraveSpeed);

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

            //straveDistanceTravelled = Mathf.Clamp(straveDistanceTravelled, -MaxStraveDistance, MaxStraveDistance);
            //if on left or right path clamp straveDistanceTravelled to maxStraveDistance
            if(currentPathType == PathType.Left && straveDistanceTravelled < -MaxStraveDistance) straveDistanceTravelled = -MaxStraveDistance;
            if(currentPathType == PathType.Right && straveDistanceTravelled > MaxStraveDistance) straveDistanceTravelled = MaxStraveDistance;
            
            if((straveDistanceTravelled <= -MaxStraveDistance || straveDistanceTravelled >= MaxStraveDistance) && resetSraveSpeedOnMaxStraveDistance)currentStraveSpeed = 0f;
            
            Vector3 thisPlayerModelPosition = playerModel.transform.localPosition;
            playerModel.transform.localPosition = new Vector3(straveDistanceTravelled, thisPlayerModelPosition.y, 0);


            //Check if straveDistanceTravelled is greater than switchPathDistance
            if (canStraveSwitchPath)
            {
                switch (currentPathType)
                {
                    case PathType.Left:
                        if (straveDistanceTravelled >= switchPathDistance)
                        {
                            SaveObjPositions();
                            SwitchToMiddlePath();
                            TeleportObjPositions();
                        }
                        break;
                    case PathType.Right:
                        if (straveDistanceTravelled <= -switchPathDistance)
                        {
                            SaveObjPositions();
                            SwitchToMiddlePath();
                            TeleportObjPositions();
                        }
                        break;
                    case PathType.Middle:
                        if (straveDistanceTravelled >= switchPathDistance)
                        {
                            SaveObjPositions();
                            SwitchToRightPath();
                            TeleportObjPositions();
                        }
                        else if (straveDistanceTravelled <= -switchPathDistance)
                        {
                            SaveObjPositions();
                            SwitchToLeftPath();
                            TeleportObjPositions();
                        }
                        break;
                }
            }


            //check if path is near end
            if (distanceTravelled >= Currentpath.path.length + ChangeToForWardPatjDistance)
            {

                //Debug.LogError($"Path is near end, currentPathType: {currentPathType}");
                if (MiddlePath.next() != null && MiddlePath.next().isNull() == false)
                {
                    SaveObjPositions();
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
                    distanceTravelled = Currentpath.path.GetClosestDistanceAlongPath(transform.position);

                    TeleportObjPositions();


                }
                else
                {
                    Debug.Log("Jetzt hat es alles auseinander genommen, du Hurensohn");
                }

            }

            
            /*
            if (Vector3.Distance(transform.position, tempCurrentPosition) > 0.45f && Vector3.Distance(transform.position, tempCurrentPosition) < 5f)
            {
                Debug.LogWarning("Player moved: " + Vector3.Distance(transform.position, tempCurrentPosition));
            }

            if (Vector3.Distance(playerModel.transform.position, tempCurrentModelPosition) > 0.45f && Vector3.Distance(playerModel.transform.position, tempCurrentModelPosition) < 5f)
            {
                Debug.LogWarning("PlayerModel moved: " + Vector3.Distance(playerModel.transform.position, tempCurrentModelPosition));
            }
            */


            //speed milestones bonus check for scoreboard
            if (hasMoreSpeedBoni && currentPlayerSpeed > scoreboardSettings.speedMilestoneLevels[speedBonusIndex].speed) {
                Scoreboard.Instance.speedMilestoneBonus(scoreboardSettings.speedMilestoneLevels[speedBonusIndex].value);
                if (scoreboardSettings.speedMilestoneLevels.Count - 1 <= speedBonusIndex) hasMoreSpeedBoni = false;
                speedBonusIndex++;
            }



            yield return null;
        }


    }
   

    private void SaveObjPositions()
    {

        saveBeforeTeleportPositions.Clear();
        foreach (GameObject tgameObject in saveBeforeTeleport)
        {
            saveBeforeTeleportPositions.Add(tgameObject.transform.position);
        }
    }

    
    private void TeleportObjPositions()
    {

        foreach (GameObject tgameObject in saveBeforeTeleport)
        {
            tgameObject.transform.position = saveBeforeTeleportPositions[saveBeforeTeleport.IndexOf(tgameObject)];
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
