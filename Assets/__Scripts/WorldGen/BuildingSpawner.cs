using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public class BuildingSpawner : MonoBehaviour
{


    [Header("Building Pool")]
    [SerializeField] private BuildingPool buildingPool;
    [SerializeField] private float firstRowOffset = 1f;
    [SerializeField] private float startXOffset = 14f;
    [SerializeField] private float YOffset = 0.75f;
    [SerializeField] private float ZOffset = 7f;
    [SerializeField] private float buildingColoumnOffset = 1f;
    [SerializeField] private float buildingRowOffset = 1f;
    [SerializeField] private float rowXOffset = 1f;
    [SerializeField] private int layers = 1;


    //public static event Action onBuildingsCached;

    private BuildingCache buildingCache = new BuildingCache();
    List<GameObject> sortedBuildings;


    private void Awake()
    {
        /*
        sortedBuildings = new List<GameObject>();
        foreach(GameObject building in buildingPool.buildings) {
            sortedBuildings.Add(building);
        }
        Debug.Log("sortedBuildings.Count before sort: " + sortedBuildings.Count());
        sortedBuildings.Sort((x, y) => x.GetComponent<BuildingParams>().getBuildingHeight().CompareTo(y.GetComponent<BuildingParams>().getBuildingHeight()));
        Debug.Log("sortedBuildings.Count after sort: " + sortedBuildings.Count());
        */


        //cache buildings
        GameObject obj;
        for (int i = 0; i < buildingPool.buildings.Count(); i++) {
            for (int j = 0; j < buildingPool.poolSize; j++) {
                obj = Instantiate(buildingPool.buildings[i]);
                obj.name = buildingPool.buildings[i].name;
                obj.SetActive(false);
                obj.transform.parent = transform;
                buildingCache.add(obj);
            }
        }

        //onBuildingsCached?.Invoke();
        GameObject player = GameObject.Find("Player");
        player.GetComponent<PlayerMovement>().buildingsCached = true;
    }

    public async Task sortBuildingList() {
        sortedBuildings = new List<GameObject>();
        foreach(GameObject building in buildingPool.buildings) {
            sortedBuildings.Add(building);
        }
        sortedBuildings.Sort((x, y) => x.GetComponent<BuildingParams>().getBuildingHeight().CompareTo(y.GetComponent<BuildingParams>().getBuildingHeight()));
        await Task.Yield();
    }


    public void spawnBuildings(ExitPointDirection exitPointDirection) {

        Tuple<GameObject, GameObject> entryExitPointTuple = new Tuple<GameObject, GameObject>(exitPointDirection.getEntryPoint(), exitPointDirection.getExitPoint());


        //GameObject entryPoint = entryExitPointTuple.Item1;
        GameObject entryPoint = new GameObject();
        entryPoint.transform.position = entryExitPointTuple.Item1.transform.position;
        entryPoint.transform.rotation = entryExitPointTuple.Item1.transform.rotation;

        //GameObject exitPoint = entryExitPointTuple.Item2;
        GameObject exitPoint = new GameObject();
        exitPoint.transform.position = entryExitPointTuple.Item2.transform.position;
        exitPoint.transform.rotation = entryExitPointTuple.Item2.transform.rotation;

        GameObject transformObject = new GameObject();
        Transform startPoint = transformObject.transform;
        startPoint.position = entryPoint.transform.position;
        startPoint.rotation = entryPoint.transform.rotation;
        startPoint.position += entryPoint.transform.up * YOffset;
        startPoint.position += entryPoint.transform.forward * ZOffset;



        float buildingWidth = buildingPool.buildings[0].GetComponent<BuildingParams>().getBuildingWidth();
        for (int i = 0; i < layers; i++)
        {
            generateBuildingLayer(startPoint, entryExitPointTuple.Item2.transform, buildingWidth, i, StreetSide.left, exitPointDirection);
            generateBuildingLayer(startPoint, entryExitPointTuple.Item2.transform, buildingWidth, i, StreetSide.right, exitPointDirection);
        }


        Destroy(entryPoint);
        Destroy(exitPoint);
        Destroy(transformObject);


    }


    private void generateBuildingLayer(Transform startPositionObject, Transform endPositionObject, float buildingWidth, int row, StreetSide side, ExitPointDirection exitPointDirection)
    {
        Vector3 startPositon = startPositionObject.position;
        Vector3 startPosRight = startPositionObject.forward;
        Quaternion startRotation = startPositionObject.rotation;

        startPositon += startPositionObject.forward * (buildingWidth / 2);
        float tileWidth;
        if (side == StreetSide.left) {
            startPositon += startPosRight * rowXOffset * row;
            tileWidth = Vector3.Distance(startPositon, endPositionObject.position);
            startPositon += startPositionObject.right * -startXOffset;
            startPositon += startPositionObject.right * (int)side * (buildingRowOffset + buildingWidth) * row;
            startRotation *= Quaternion.Euler(0, 90, 0);
        }
        else {
            startPositon += startPosRight * rowXOffset * row;
            tileWidth = Vector3.Distance(startPositon, endPositionObject.position);
            startPositon += startPositionObject.right * startXOffset;
            startPositon += startPositionObject.right * (int)side * (buildingRowOffset + buildingWidth) * row;
            startRotation *= Quaternion.Euler(0, -90, 0);
        }


        int numberOfBuildings = calculateNumberOfBuildings(tileWidth, buildingWidth);
        string[] buildings = getBuildingsToSpawn(numberOfBuildings, row);
        //Vector3 stPosRight = startPosRight * (int)side;
        Vector3 stPosRight = startPosRight;
        

        GameObject building;
        for (int i = 0; i < numberOfBuildings; i++)
        {
            building = buildingCache.getTile(buildings[i]);
            building.transform.position = startPositon + (buildingWidth * stPosRight * i) + (stPosRight * buildingColoumnOffset * i);
            building.transform.rotation = startRotation;
            exitPointDirection.addBuildings(building);
        }

    }

    private int calculateNumberOfBuildings(float tileWidth, float buildingWidth)
    {
        int numberOfBuildings = (int)(tileWidth / (buildingWidth + buildingColoumnOffset));
        return numberOfBuildings + 1;
    }

    private string[] getBuildingsToSpawn(int numberOfBuildings, int row) {
        string[] buildingsToSpawn = new string[numberOfBuildings];

        for(int i = 0; i < numberOfBuildings; i++) {
            if (row == 0) {
                buildingsToSpawn[i] = sortedBuildings[UnityEngine.Random.Range(0, sortedBuildings.Count() / 2)].name;
            }
            else {
                buildingsToSpawn[i] = sortedBuildings[UnityEngine.Random.Range(sortedBuildings.Count() / 2, sortedBuildings.Count())].name;
            }
        }
        return buildingsToSpawn;
    }


    public void resetBuildings(List<GameObject> buildings) {
        buildingCache.add(buildings);
    }
}


public enum StreetSide {
    left = -1,
    right = 1
}



public class BuildingCache {

    private List<List<GameObject>> cachedBuildings = new List<List<GameObject>>();
    private Dictionary<string, int> buildingTypeIndex = new Dictionary<string, int>();



    public void add(GameObject building) {
        if (buildingTypeIndex.ContainsKey(building.name)) {
            cachedBuildings[buildingTypeIndex[building.name]].Add(building);
            building.SetActive(false);
        }
        else {
            cachedBuildings.Add(new List<GameObject>());
            cachedBuildings[cachedBuildings.Count - 1].Add(building);
            buildingTypeIndex.Add(building.name, cachedBuildings.Count - 1);
            building.SetActive(false);
        }
    }

    public void add(List<GameObject> buildings) {
        foreach(GameObject building in buildings) {
            add(building);
        }
    }

    public GameObject getTile(string name) {
        if (buildingTypeIndex.ContainsKey(name)) {
            if (cachedBuildings[buildingTypeIndex[name]].Count > 0) {
                GameObject building = cachedBuildings[buildingTypeIndex[name]][0];
                cachedBuildings[buildingTypeIndex[name]].RemoveAt(0);
                building.SetActive(true);
                return building;
            }
            else {
                Debug.LogError("No building of name " + name + " in cache");
                return null;
            }
        }
        else {
            Debug.LogError("BuildingName: " + name + " not registered in cache");
            return null;
        }
    }
}
