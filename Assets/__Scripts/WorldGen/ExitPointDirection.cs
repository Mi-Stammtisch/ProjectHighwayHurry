using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation;

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
    [SerializeField] private GameObject spawnPointsLeftParent;
    [SerializeField] private GameObject spawnPointsMiddleParent;
    [SerializeField] private GameObject spawnPointsRightParent;

    public CustomSpline leftSpline;
    public CustomSpline middleSpline;
    public CustomSpline rightSpline;

    [SerializeField] private PathCreator nextSpline;
    [SerializeField] private PathCreator previousSpline;

    private List<GameObject> cars = new List<GameObject>();
    private SpawnTileV2 spawnTileV2;

    void Awake() {
        leftSpline = new CustomSpline(leftPath.GetComponent<PathCreator>());
        middleSpline = new CustomSpline(middlePath.GetComponent<PathCreator>());
        rightSpline = new CustomSpline(rightPath.GetComponent<PathCreator>());

        spawnTileV2 = GameObject.Find("TileSpawner").GetComponent<SpawnTileV2>();
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

    public void spawnCars(List<GameObject> spawnCars) {
        if (tileType != TileType.straight) return;
        
        int track = Random.Range(0, 3);
        GameObject spawnPoint;
        foreach (GameObject car in spawnCars) {
            ScuffedCarAI scuffedCarAI = car.GetComponent<ScuffedCarAI>();
            switch(track) {
                case 0:
                    spawnPoint = spawnPointsLeftParent.transform.GetChild(Random.Range(0, spawnPointsLeftParent.transform.childCount)).gameObject;
                    scuffedCarAI.init(TrackType.left, gameObject, spawnPoint);
                    cars.Add(car);
                    break;
                case 1:
                    spawnPoint = spawnPointsMiddleParent.transform.GetChild(Random.Range(0, spawnPointsMiddleParent.transform.childCount)).gameObject;
                    scuffedCarAI.init(TrackType.middle, gameObject, spawnPoint);
                    cars.Add(car);
                    break;
                case 2:
                    spawnPoint = spawnPointsRightParent.transform.GetChild(Random.Range(0, spawnPointsRightParent.transform.childCount)).gameObject;
                    scuffedCarAI.init(TrackType.right, gameObject, spawnPoint);
                    cars.Add(car);
                    break;
            }
        }
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
    }

    public void addCars(GameObject car) {
        cars.Add(car);
    }
    public void addCars(List<GameObject> cars) {
        this.cars.AddRange(cars);
    }
}


public enum TileType {
    straight,
    leftTurn,
    rightTurn,
    special
}