using System.Collections.Generic;
using UnityEngine;
using PathCreation;
using System.Linq;
using System;
using UnityEngine.Events;

public class ExitPointDirection : MonoBehaviour
{
    [Header("Tile Settings")]
    [SerializeField] private TileType tileType;

    [Header("Entry/Exit Points")]
    [Tooltip("Drag the entryPoint in here")]
    [SerializeField] private GameObject entryPoint;
    [Tooltip("Drag the exitPoint in here")]
    [SerializeField] private GameObject exitPoint;

    [Header("Splines")]
    [SerializeField] public GameObject leftPath;
    [SerializeField] public GameObject middlePath;
    [SerializeField] public GameObject rightPath;

    [Header("Car Spawning")]
    [SerializeField] private GameObject carSpawnPoints;
    [SerializeField] public bool canSpawnCars;
    private List<List<GameObject>> spawnPoints;
    private List<List<int>> usedSpawnPoints;
    private int usedSpawnPointsInt = 0;

    public CustomSpline leftSpline;
    public CustomSpline middleSpline;
    public CustomSpline rightSpline;

    [Header("Reseting")]
    [SerializeField] private List<FunctionReference> functionList;

    [SerializeField] private PathCreator nextSpline;
    [SerializeField] private PathCreator previousSpline;

    [Header("Scaling in Distance")]
    [SerializeField] public GameObject scaleObject;

    private List<GameObject> cars = new List<GameObject>();
    private List<GameObject> buildings = new List<GameObject>();
    private SpawnTileV2 spawnTileV2;
    private BuildingSpawner buildingSpawner;
    private bool initialized = false;

    void Awake() {
        leftSpline = new CustomSpline(leftPath.GetComponent<PathCreator>());
        middleSpline = new CustomSpline(middlePath.GetComponent<PathCreator>());
        rightSpline = new CustomSpline(rightPath.GetComponent<PathCreator>());

        spawnTileV2 = GameObject.Find("TileSpawner").GetComponent<SpawnTileV2>();
        buildingSpawner = GameObject.Find("EnvironmentSpawner").GetComponent<BuildingSpawner>();

        if (canSpawnCars) {
            spawnPoints = new List<List<GameObject>>();
            usedSpawnPoints = new List<List<int>>();
            for (int i = 0; i < carSpawnPoints.transform.childCount; i++) {
                spawnPoints.Add(new List<GameObject>());
                usedSpawnPoints.Add(new List<int>());
                for (int j = 0; j < carSpawnPoints.transform.GetChild(i).childCount; j++) {
                    spawnPoints[i].Add(carSpawnPoints.transform.GetChild(i).GetChild(j).gameObject);
                    usedSpawnPoints[i].Add(0);
                }
            }
        }
    }


    void Update() {
        if (leftSpline.next() != null && leftSpline.next().isNull() == false) {
            nextSpline = leftSpline.next().spline;
        }
        if (leftSpline.previous() != null && leftSpline.previous().isNull() == false) {
            previousSpline = leftSpline.previous().spline;
        }
    }

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

        //draw 2x2 cube at the entry point and exit point
        Gizmos.color = Color.blue;
        Gizmos.DrawCube(entryPoint.transform.position, new Vector3(2, 2, 2));
        Gizmos.DrawCube(exitPoint.transform.position, new Vector3(2, 2, 2));
        
        
        //Debug.Log("exitPointRotation in degrees: " + exitPoint.transform.rotation.eulerAngles);
    }

    public GameObject getEntryPoint() {
        return entryPoint;
    }

    public GameObject getExitPoint() {
        return exitPoint;
    }

    public TileType getTileType() {
        return tileType;
    }

    public List<GameObject> spawnCars(List<GameObject> spawnCars) {
        initialized = true;
        if (!canSpawnCars) return spawnCars;

        GameObject spawnPoint;
        Tuple<int, int> spawnIndex;
        GameObject car;
        List<GameObject> carHashesToRemove = new List<GameObject>();
        for(int i = 0; i < spawnCars.Count; i++) {
            car = spawnCars[i];
            spawnIndex = getRandomSpawnIndex();
            ScuffedCarAI scuffedCarAI = car.GetComponent<ScuffedCarAI>();
            switch(spawnIndex.Item1) {
                case 0:
                    spawnPoint = spawnPoints[0][spawnIndex.Item2];
                    scuffedCarAI.init(TrackType.left, gameObject, spawnPoint);
                    cars.Add(car);
                    carHashesToRemove.Add(car);
                    break;
                case 1:
                    spawnPoint = spawnPoints[1][spawnIndex.Item2];
                    scuffedCarAI.init(TrackType.middle, gameObject, spawnPoint);
                    cars.Add(car);
                    carHashesToRemove.Add(car);
                    break;
                case 2:
                    spawnPoint = spawnPoints[2][spawnIndex.Item2];
                    scuffedCarAI.init(TrackType.right, gameObject, spawnPoint);
                    cars.Add(car);
                    carHashesToRemove.Add(car);
                    break;
                case -1:
                    Debug.LogWarning("No more spawnpoints available");
                    foreach(GameObject carHash in carHashesToRemove) {
                        spawnCars.Remove(carHash);
                    }
                    return spawnCars;
            }
        }
        foreach(GameObject carHash in carHashesToRemove) {
            spawnCars.Remove(carHash);
        }
        return spawnCars;
    }

    private Tuple<int, int> getRandomSpawnIndex() {
        int totalSpawnPoints = spawnPoints.SelectMany(list => list).Count();

        if (totalSpawnPoints / 2 <= usedSpawnPointsInt) {
            Debug.LogWarning("No more spawnpoints available");
            return new Tuple<int, int>(-1, -1);
        }


        bool notFound = true;
        int index = 0;
        int lane = 0;
        int iterations = 0;
        int neighboursFound = 0;

        while (notFound && iterations <= totalSpawnPoints * 3) {
            lane = UnityEngine.Random.Range(0, 3);
            index = UnityEngine.Random.Range(0, spawnPoints[lane].Count);
            if (usedSpawnPoints[lane][index] == 0) {
                //top
                if (index - 1 >= 0) {
                    if (usedSpawnPoints[lane][index - 1] == 1) {
                        neighboursFound++;
                    }
                }
                //bottom
                if (index + 1 < spawnPoints[lane].Count) {
                    if (usedSpawnPoints[lane][index + 1] == 1) {
                        neighboursFound++;
                    }
                }
                //left
                if (lane - 1 >= 0) {
                    if (usedSpawnPoints[lane - 1][index] == 1) {
                        neighboursFound++;
                    }
                }
                //right
                if (lane + 1 < spawnPoints.Count) {
                    if (usedSpawnPoints[lane + 1][index] == 1) {
                        neighboursFound++;
                    }
                }

                if (neighboursFound == 0) {
                    notFound = false;
                    usedSpawnPoints[lane][index] = 1;
                    usedSpawnPointsInt++;
                }
                else {
                    neighboursFound = 0;
                    iterations++;
                }
            }
            else {
                iterations++;              
            }
        }


        if (notFound) {
            lane = -1;
            Debug.LogWarning("No spawnpoints found");
        }

        return new Tuple<int, int>(lane, index);
    }

    public void Reset() {
        if (leftSpline.nextSpline != null) {
            leftSpline.nextSpline.previousSpline = null;
            middleSpline.nextSpline.previousSpline = null;
            rightSpline.nextSpline.previousSpline = null;

            leftSpline.Reset();
            middleSpline.Reset();
            rightSpline.Reset();

            if (leftSpline.next() != null && leftSpline.next().isNull() == false) {
                nextSpline = leftSpline.next().spline;
            }
            else{
                nextSpline = null;
            }
            if (leftSpline.previous() != null && leftSpline.previous().isNull() == false) {
                previousSpline = leftSpline.previous().spline;
            }
            else{
                previousSpline = null;
            }            
        }
        spawnTileV2.resetCars(cars);
        cars.Clear();
        buildingSpawner.resetBuildings(buildings);
        buildings.Clear();
        usedSpawnPointsInt = 0;
        if (initialized && canSpawnCars) {
            for (int i = 0; i < usedSpawnPoints.Count(); i++) {
                for (int j = 0; j < usedSpawnPoints[i].Count(); j++) {
                    usedSpawnPoints[i][j] = 0;
                }
            }
        }
        foreach (FunctionReference function in functionList) {
            //function.targetScript.Invoke(function.functionName, 0);
            function.SelectEvent.Invoke();
        }
    }

    public void addCars(GameObject car) {
        cars.Add(car);
    }
    public void addCars(List<GameObject> cars) {
        this.cars.AddRange(cars);
    }

    public void addBuildings(GameObject building) {
        buildings.Add(building);
    }

    public void addBuildings(List<GameObject> buildings) {
        this.buildings.AddRange(buildings);
    }


    public void scaleObjects() {

        //tile
        Vector3 initialPosition = scaleObject.transform.position;
        scaleObject.transform.position += new Vector3(0, -10, 0);
        LeanTween.moveLocalY(scaleObject, initialPosition.y, 0.2f).setEaseOutQuad();

        //cars
        foreach (GameObject car in cars) {
            Vector3 initialCarPosition = car.transform.position;
            car.transform.position += new Vector3(0, -10, 0);
            LeanTween.moveLocalY(car, initialCarPosition.y, 0.2f).setEaseOutQuad();
        }

    }

}


public enum TileType {
    straight,
    leftTurn,
    rightTurn,
    special
}

[System.Serializable]
public class FunctionReference
{
    [SerializeField] public UnityEvent SelectEvent;

    // Constructor to initialize the FunctionReference
    public FunctionReference(UnityEvent script)
    {
        SelectEvent = script;
    }
}