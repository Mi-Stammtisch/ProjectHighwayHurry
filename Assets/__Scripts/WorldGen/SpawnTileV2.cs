using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;


public class SpawnTileV2 : MonoBehaviour
{
    //Instance 
    public static SpawnTileV2 Instance;

    [Header("Tile Pool Settings")]
    [SerializeField] private TilePool tilePool;
    [SerializeField] private int tileCount = 5;
    [SerializeField] public List<GameObject> tiles;
    [SerializeField] private int turnSpawnCooldownMax;
    [SerializeField] private int maxDistanceFromSpawn = 1;
    [SerializeField] private int numberOfStraightTilesInTurns = 2;
    [SerializeField] private AnimationCurve spawnTurnChance;
    [SerializeField] private bool testRun = false;
    [SerializeField] private List<GameObject> testTiles;

    [Header("Car Spawning")]
    [SerializeField] private bool enableCarSpawning = true;
    [SerializeField] private GameObject carCacheParent;
    [SerializeField] private List<GameObject> cars;
    [SerializeField] private int minNumberOfCars = 1;
    [SerializeField] private int maxNumberOfCars = 3;
    [SerializeField] private int carCooldownBegin = 3;
    

    [Header("Tile Caching")]
    [SerializeField] private GameObject tileCacheParent;
    [SerializeField] private List<minTileCache> minCachedTiles;
    private TileCache tileCache = new TileCache();

    private int turnSpawnCooldown;
    private List<GameObject> nextTiles = new List<GameObject>();

    private event Action specialTile;
    private float startTime;
    private int tileBasedCounter = 0;

    public static event Action onTilesCached;

    private CarCache carCache = new CarCache();
    

   
    private void Awake()
    {
        Instance = this;

        //cache tiles
        GameObject obj;
        foreach(minTileCache minTileCache in minCachedTiles) {
            for(int i = 0; i < minTileCache.minTiles; i++) {
                switch(minTileCache.tileType) {
                    case TileType.straight:
                        obj = Instantiate(tilePool.straightTiles[UnityEngine.Random.Range(0, tilePool.straightTiles.Count)]);
                        obj.SetActive(false);
                        obj.transform.parent = tileCacheParent.transform;
                        tileCache.add(obj);
                        break;
                    case TileType.leftTurn:
                        obj = Instantiate(tilePool.leftTurnTiles[UnityEngine.Random.Range(0, tilePool.leftTurnTiles.Count)]);
                        obj.SetActive(false);
                        obj.transform.parent = tileCacheParent.transform;
                        tileCache.add(obj);
                        break;
                    case TileType.rightTurn:
                        obj = Instantiate(tilePool.rightTurnTiles[UnityEngine.Random.Range(0, tilePool.rightTurnTiles.Count)]);
                        obj.SetActive(false);
                        obj.transform.parent = tileCacheParent.transform;
                        tileCache.add(obj);
                        break;
                    case TileType.special:
                        obj = Instantiate(tilePool.specialTiles[UnityEngine.Random.Range(0, tilePool.specialTiles.Count)]);
                        obj.SetActive(false);
                        obj.transform.parent = tileCacheParent.transform;
                        tileCache.add(obj);
                        break;
                }
            }
        }

        //cache cars
        if (enableCarSpawning) {
            for (int i = 0; i < maxNumberOfCars * 20; i++) {
                GameObject car = Instantiate(cars[UnityEngine.Random.Range(0, cars.Count)]);
                car.SetActive(false);
                car.transform.parent = carCacheParent.transform;
                carCache.add(car);
            }
        }
    }



    void Start() {
        switch (tilePool.specialTileSpawning) {
            case SpecialTileSpawning.TimeBased:
                specialTile += timeBased;
                startTime = Time.time;
                break;
            case SpecialTileSpawning.TileBased:
                specialTile += tileBased;
                break;
            case SpecialTileSpawning.DifficultyBased:
                specialTile += diffBased;
                break;
            case SpecialTileSpawning.Random:
                specialTile += randomBased;
                break;
        }


        if (testRun) {
            foreach (GameObject tile in testTiles) {
                nextTiles.Add(tile);
            }
            spawnNewTile();
        }
        while (tiles.Count < tileCount) {
            spawnInitialTiles(tilePool.straightTiles[UnityEngine.Random.Range(0, tilePool.straightTiles.Count)]);
        }
        onTilesCached?.Invoke();
    }


    private void spawnInitialTiles(GameObject tile) {
        GameObject newTile;
        GameObject exitPoint;

        newTile = tileCache.getTile(tile.GetComponent<ExitPointDirection>().getTileType());
        ExitPointDirection newExitPointDirection = newTile.GetComponent<ExitPointDirection>();
        //newTile.GetComponent<ExitPointDirection>().Reset();
        if (carCooldownBegin > 0) carCooldownBegin -= 1;

        if (tiles.Count > 0) {
            //newTile = Instantiate(tile, tiles[tiles.Count - 1].GetComponent<ExitPointDirection>().getExitPoint().transform.position, Quaternion.identity);
            
            newTile.transform.position = tiles[tiles.Count - 1].GetComponent<ExitPointDirection>().getExitPoint().transform.position;
            tileBasedCounter++;
            GameObject lastTile = tiles[tiles.Count - 1];
            ExitPointDirection lastExitPointDirection = lastTile.GetComponent<ExitPointDirection>();

            lastExitPointDirection.leftSpline.setNext(newExitPointDirection.leftSpline);
            lastExitPointDirection.middleSpline.setNext(newExitPointDirection.middleSpline);
            lastExitPointDirection.rightSpline.setNext(newExitPointDirection.rightSpline);


            newExitPointDirection.leftSpline.setPrevious(lastExitPointDirection.leftSpline);
            newExitPointDirection.middleSpline.setPrevious(lastExitPointDirection.middleSpline);
            newExitPointDirection.rightSpline.setPrevious(lastExitPointDirection.rightSpline);

            exitPoint = tiles[tiles.Count - 1].GetComponent<ExitPointDirection>().getExitPoint();
        }
        else {
            //newTile = Instantiate(tile, Vector3.zero, Quaternion.identity);
            tileBasedCounter++;
            newTile.transform.position = Vector3.zero;
            exitPoint = new GameObject();
            exitPoint.transform.rotation = Quaternion.Euler(new Vector3(0, 90, 0));
        }
        for (int i = 0; i <= 4; i++) {
            //
            newTile.transform.rotation = Quaternion.Euler(new Vector3(0, i * 90, 0));
            //Debug.Log("newTileRotation: " + newTile.transform.rotation.eulerAngles);
            //Debug.Log("---");
            //Debug.Log("entryPointRotation: " + newTile.GetComponent<ExitPointDirection>().getEntryPoint().transform.rotation.eulerAngles.y);
            //Debug.Log("exitPointRotation: " + exitPoint.transform.rotation.eulerAngles.y);
            if (Math.Abs(newTile.GetComponent<ExitPointDirection>().getEntryPoint().transform.rotation.eulerAngles.y - exitPoint.transform.rotation.eulerAngles.y) < 5f) {
                //Debug.Log("rotationFound: " + i * 90);
                break;
            }
        }
        //Debug.Log("--------------------");

        newTile.transform.parent = transform;
        tiles.Add(newTile);

        if (enableCarSpawning) {
            int numSpawnCars;
            if (carCooldownBegin > 0) {
                numSpawnCars = 0;
            }
            else {
                numSpawnCars = UnityEngine.Random.Range(minNumberOfCars, maxNumberOfCars + 1);
            }
            List<GameObject> spawnCars = new List<GameObject>();
            for (int i = 0; i < numSpawnCars; i++) {
                GameObject car = carCache.getCar();
                spawnCars.Add(car);
            }
            newTile.GetComponent<ExitPointDirection>().spawnCars(spawnCars);
        }

        if (tiles.Count > tileCount) {
            //Destroy(tiles[0], 1f);
            tileCache.add(tiles[0]);
            tiles.RemoveAt(0);
        }
    }

    public void spawnNewTile() {

        if (!tilePool.noSpecialTiles) specialTile?.Invoke();

        if (nextTiles.Count == 0) {
            float distanceToSpawn = Vector3.Magnitude(tiles[tiles.Count - 1].GetComponent<ExitPointDirection>().getExitPoint().transform.position);
            float ddwd = distanceToSpawn / maxDistanceFromSpawn;
            //Debug.Log("ddwd: " + ddwd);
            float turnChance = spawnTurnChance.Evaluate(ddwd);
            //Debug.Log("turnChance: " + turnChance);
            //Debug.Log("--------------");
            bool spawnTurn = UnityEngine.Random.Range(0f, 1f) < turnChance;
            if (spawnTurn && turnSpawnCooldown <= 0) {
                turnSpawnCooldown = turnSpawnCooldownMax;
                if (UnityEngine.Random.Range(0, 2) == 0) {
                        spawnInitialTiles(tilePool.leftTurnTiles[UnityEngine.Random.Range(0, tilePool.leftTurnTiles.Count)]);
                        for(int i = 0; i <= UnityEngine.Random.Range(0, numberOfStraightTilesInTurns + 1); i++) {
                            nextTiles.Add(tilePool.straightTiles[UnityEngine.Random.Range(0, tilePool.straightTiles.Count)]);
                        }
                        nextTiles.Add(tilePool.leftTurnTiles[UnityEngine.Random.Range(0, tilePool.leftTurnTiles.Count)]);
                        nextTiles.Add(tilePool.straightTiles[UnityEngine.Random.Range(0, tilePool.straightTiles.Count)]);
                        nextTiles.Add(tilePool.straightTiles[UnityEngine.Random.Range(0, tilePool.straightTiles.Count)]);
                }
                else {
                    spawnInitialTiles(tilePool.rightTurnTiles[UnityEngine.Random.Range(0, tilePool.rightTurnTiles.Count)]);
                    for(int i = 0; i <= UnityEngine.Random.Range(0, numberOfStraightTilesInTurns + 1); i++) {
                            nextTiles.Add(tilePool.straightTiles[UnityEngine.Random.Range(0, tilePool.straightTiles.Count)]);
                    }
                    nextTiles.Add(tilePool.rightTurnTiles[UnityEngine.Random.Range(0, tilePool.rightTurnTiles.Count)]);
                    nextTiles.Add(tilePool.straightTiles[UnityEngine.Random.Range(0, tilePool.straightTiles.Count)]);
                    nextTiles.Add(tilePool.straightTiles[UnityEngine.Random.Range(0, tilePool.straightTiles.Count)]);
                }
            }
            else {
                spawnInitialTiles(tilePool.straightTiles[UnityEngine.Random.Range(0, tilePool.straightTiles.Count)]);
                turnSpawnCooldown--;
            }
        }
        else if (nextTiles.Count > 0) {
            spawnInitialTiles(nextTiles[0]);
            nextTiles.RemoveAt(0);
        }
    }

    public void resetCars(List<GameObject> cars) {
        foreach(GameObject car in cars) {
            carCache.add(car);
        }
    }


    private void timeBased() {
        if (Time.time - startTime > tilePool.time) {
            startTime = Time.time;
            nextTiles.Add(tilePool.specialTiles[UnityEngine.Random.Range(0, tilePool.specialTiles.Count)]);
            Debug.Log("Spawned special tile");
        }
    }

    private void tileBased() {
        if (tileBasedCounter > tilePool.tile) {
            nextTiles.Add(tilePool.specialTiles[UnityEngine.Random.Range(0, tilePool.specialTiles.Count)]);
            Debug.Log("Spawned special tile");
            tileBasedCounter = 0;
        }
    }

    private void diffBased() {
        //TODO: Implement difficulty based spawning of special tiles
    }

    private void randomBased() {
        if (UnityEngine.Random.Range(0f, 1f) < tilePool.random) {
            nextTiles.Add(tilePool.specialTiles[UnityEngine.Random.Range(0, tilePool.specialTiles.Count)]);
            Debug.Log("Spawned special tile");
        }
    }
}


[Serializable]
public class minTileCache {
    public TileType tileType;
    public int minTiles;
}


public class TileCache {

    private List<List<GameObject>> cachedTiles = new List<List<GameObject>>();
    private Dictionary<TileType, int> tileTypeIndex = new Dictionary<TileType, int>();



    public void add(GameObject tile) {
        ExitPointDirection exitPointDirection = tile.GetComponent<ExitPointDirection>();
        TileType tileType = exitPointDirection.getTileType();
        exitPointDirection.Reset();

        if (tileTypeIndex.ContainsKey(tileType)) {
            cachedTiles[tileTypeIndex[tileType]].Add(tile);
            tile.SetActive(false);
        }
        else {
            cachedTiles.Add(new List<GameObject>());
            cachedTiles[cachedTiles.Count - 1].Add(tile);
            tileTypeIndex.Add(tileType, cachedTiles.Count - 1);
            tile.SetActive(false);
        }
    }

    public GameObject getTile(TileType type) {
        if (tileTypeIndex.ContainsKey(type)) {
            if (cachedTiles[tileTypeIndex[type]].Count > 0) {
                GameObject tile = cachedTiles[tileTypeIndex[type]][0];
                cachedTiles[tileTypeIndex[type]].RemoveAt(0);
                tile.SetActive(true);
                return tile;
            }
            else {
                Debug.LogError("No tiles of type " + type + " in cache");
                return null;
            }
        }
        else {
            Debug.LogError("TileType: " + type + " not registered in cache");
            return null;
        }
    }
}


public class CarCache {
    private List<GameObject> cachedCars = new List<GameObject>();

    public void add(GameObject car) {
        car.GetComponent<ScuffedCarAI>().Reset();
        cachedCars.Add(car);
        car.SetActive(false);
    }

    public GameObject getCar() {
        if (cachedCars.Count > 0) {
            GameObject car = cachedCars[0];
            cachedCars.RemoveAt(0);
            car.SetActive(true);
            return car;
        }
        else {
            Debug.LogError("No cars in cache");
            return null;
        }
    }
}