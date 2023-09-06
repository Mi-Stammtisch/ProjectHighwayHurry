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
    private int turnSpawnCooldown;
    private List<GameObject> nextTiles = new List<GameObject>();

    private void spawnInitialTiles(GameObject tile) {
        GameObject newTile;
        GameObject exitPoint;
        if (tiles.Count > 0) {
            newTile = Instantiate(tile, tiles[tiles.Count - 1].GetComponent<ExitPointDirection>().getExitPoint().transform.position, Quaternion.identity);
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
        while (tiles.Count < tileCount) {
            spawnInitialTiles(tilePool.straightTiles[UnityEngine.Random.Range(0, tilePool.straightTiles.Count)]);
        }
    }


    public void spawnNewTile() {
        if (nextTiles.Count == 0) {
            float distanceToSpawn = Vector3.Magnitude(tiles[tiles.Count - 1].GetComponent<ExitPointDirection>().getExitPoint().transform.position);
            //Debug.Log("distanceToSpawn: " + distanceToSpawn);
            
            /*
            if (distanceToSpawn <= 500) {
                spawnInitialTiles(tilePool.straightTiles[UnityEngine.Random.Range(0, tilePool.straightTiles.Count)]);
            }
            else {
                if (UnityEngine.Random.Range(0, 2) == 0) {
                    spawnInitialTiles(tilePool.leftTurnTiles[UnityEngine.Random.Range(0, tilePool.leftTurnTiles.Count)]);
                    nextTiles.Add(tilePool.leftTurnTiles[UnityEngine.Random.Range(0, tilePool.leftTurnTiles.Count)]);
                    nextTiles.Add(tilePool.straightTiles[UnityEngine.Random.Range(0, tilePool.straightTiles.Count)]);
                    nextTiles.Add(tilePool.straightTiles[UnityEngine.Random.Range(0, tilePool.straightTiles.Count)]);
                }
                else {
                    spawnInitialTiles(tilePool.rightTurnTiles[UnityEngine.Random.Range(0, tilePool.rightTurnTiles.Count)]);
                    nextTiles.Add(tilePool.rightTurnTiles[UnityEngine.Random.Range(0, tilePool.rightTurnTiles.Count)]);
                    nextTiles.Add(tilePool.straightTiles[UnityEngine.Random.Range(0, tilePool.straightTiles.Count)]);
                    nextTiles.Add(tilePool.straightTiles[UnityEngine.Random.Range(0, tilePool.straightTiles.Count)]);
                }
            }
        }
        else {
            spawnInitialTiles(nextTiles[0]);
            nextTiles.RemoveAt(0);
        }
        */
        //spawnInitialTiles(tilePool.straightTiles[UnityEngine.Random.Range(0, tilePool.straightTiles.Count)]);


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
        else {
            spawnInitialTiles(nextTiles[0]);
            nextTiles.RemoveAt(0);
        }

    }
}