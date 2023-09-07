using System;
using System.Collections.Generic;
using UnityEngine;


public class SpawnTileV2 : MonoBehaviour
{
    //Instance 
    public static SpawnTileV2 Instance;
    [SerializeField] private TilePool tilePool;
    [SerializeField] private int tileCount = 5;
    [SerializeField] public List<GameObject> tiles;
    [SerializeField] private int turnSpawnCooldownMax;
    [SerializeField] private int maxDistanceFromSpawn = 1;
    [SerializeField] private int numberOfStraightTilesInTurns = 2;
    [SerializeField] private AnimationCurve spawnTurnChance;
    [SerializeField] private bool testRun = false;
    [SerializeField] private List<GameObject> testTiles;
    private int turnSpawnCooldown;
    private List<GameObject> nextTiles = new List<GameObject>();

    private event Action specialTile;
    private float startTime;
    private int tileBasedCounter = 0;

    private void spawnInitialTiles(GameObject tile) {
        GameObject newTile;
        GameObject exitPoint;
        if (tiles.Count > 0) {
            newTile = Instantiate(tile, tiles[tiles.Count - 1].GetComponent<ExitPointDirection>().getExitPoint().transform.position, Quaternion.identity);
            tileBasedCounter++;
            GameObject lastTile = tiles[tiles.Count - 1];

            lastTile.GetComponent<ExitPointDirection>().leftSpline.setNext(newTile.GetComponent<ExitPointDirection>().leftSpline);
            lastTile.GetComponent<ExitPointDirection>().middleSpline.setNext(newTile.GetComponent<ExitPointDirection>().middleSpline);
            lastTile.GetComponent<ExitPointDirection>().rightSpline.setNext(newTile.GetComponent<ExitPointDirection>().rightSpline);


            newTile.GetComponent<ExitPointDirection>().leftSpline.setPrevious(lastTile.GetComponent<ExitPointDirection>().leftSpline);
            newTile.GetComponent<ExitPointDirection>().middleSpline.setPrevious(lastTile.GetComponent<ExitPointDirection>().middleSpline);
            newTile.GetComponent<ExitPointDirection>().rightSpline.setPrevious(lastTile.GetComponent<ExitPointDirection>().rightSpline);

            exitPoint = tiles[tiles.Count - 1].GetComponent<ExitPointDirection>().getExitPoint();
        }
        else {
            newTile = Instantiate(tile, Vector3.zero, Quaternion.identity);
            tileBasedCounter++;

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
        if (tiles.Count > tileCount) {
            Destroy(tiles[0], 1f);
            tiles.RemoveAt(0);
        }
    }

   
    private void Awake()
    {
            
            Instance = this;
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