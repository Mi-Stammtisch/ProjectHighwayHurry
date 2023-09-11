using System.Collections;
using System.Collections.Generic;
using PathCreation;
using UnityEngine;

public class ScuffedCarAI : MonoBehaviour
{
    [SerializeField] private CarSettingsScriptable carSettings;
    //[SerializeField] private GameObject tile;
    [SerializeField] private PathCreator previousSpline;
    [SerializeField] private float distanceTravelled;
    [SerializeField] private float speed;
    
    [SerializeField] private bool stopMoving = false;


    private CustomSpline pathCreator;
    private bool initialized = false;
    private bool hasStarted = false;

    private float updateCooldown;
    private Coroutine moveCoroutine;
    private List<GameObject> coins = new List<GameObject>();
    float offset = 0;


    public void init(TrackType trackType, GameObject tile, GameObject spawnPoint) {
        //pathCreator = new CustomSpline(spline.GetComponent<PathCreator>());
        switch (trackType) {
            case TrackType.middle:
                pathCreator = tile.GetComponent<ExitPointDirection>().middleSpline;
                break;
            case TrackType.left:
                pathCreator = tile.GetComponent<ExitPointDirection>().leftSpline;
                offset = 1.5f;
                break;
            case TrackType.right:
                pathCreator = tile.GetComponent<ExitPointDirection>().rightSpline;
                offset = -1.5f;
                break;
        }

        transform.position = spawnPoint.transform.position;

        if (pathCreator == null) Debug.LogError("pathCreator is null");
        distanceTravelled = Vector3.Magnitude(transform.position - pathCreator.path.GetPointAtDistance(0f));
        //Debug.Log("pathLength: " + pathCreator.path.length);

        transform.position = pathCreator.path.GetPointAtDistance(distanceTravelled);
        transform.rotation = pathCreator.path.GetRotationAtDistance(distanceTravelled);
        //transform.rotation *= Quaternion.Euler(0, -90, 0);
        transform.rotation *= Quaternion.Euler(0, 180, 0);
        transform.position += new Vector3(0, 2.5f, 0);
        
        //get offset to place car 1m to the right, based on the cars rotation
        //transform.position += offset;
        transform.position += transform.right * offset;

        

        


        if (pathCreator.previous() != null && pathCreator.previous().isNull() == false) {
            previousSpline = pathCreator.previous().spline;
        }
        else {
            speed = 0;
        }

        //spawn coins
        if (Random.Range(0f, 1f) <= carSettings.coinSpawnChance) {
            for (int i = 0; i < carSettings.numberOfCoins; i++) {
                GameObject coin = Instantiate(carSettings.coinPrefab, transform.position + transform.forward * carSettings.coinSpawnDistanceCar + transform.forward * carSettings.coinSpacing * i, Quaternion.identity);
                coin.transform.position += new Vector3(0, 1f, 0);
                coin.transform.rotation = transform.rotation;
                coin.transform.parent = transform;
                coins.Add(coin);
                GameObject coinBehind = Instantiate(carSettings.coinPrefab, transform.position + -transform.forward * carSettings.coinSpawnDistanceCar + -transform.forward * carSettings.coinSpacing * i, Quaternion.identity);
                coinBehind.transform.position += new Vector3(0, 1f, 0);
                coinBehind.transform.rotation = transform.rotation;
                coinBehind.transform.parent = transform;
                coins.Add(coinBehind);
                //Debug.Log("pathCreatorIsNullBeforeInitializeData: " + (pathCreator == null).ToString());
                //coin.GetComponent<CoinController>().initializeData(pathCreator, distanceTravelled, speed);
                //coin.GetComponent<CoinController>().startMoving();
            }
        }
        
        initialized = true;

        updateCooldown = Random.Range(0.001f, 0.01f);
    }

    public void triggerStayOld() {
        if (initialized && !hasStarted) {
            //moveCoroutine ??= StartCoroutine(moveCar(Random.Range(minSpeed, maxSpeed)));
            speed = Random.Range(carSettings.minSpeed, carSettings.maxSpeed);
            hasStarted = true;
            moveCoroutine ??= StartCoroutine(customUpdate());
        }
    }


    private IEnumerator customUpdate() {

        while(!stopMoving && pathCreator.isNull() == false) {
        //if (moving && !stopMoving && pathCreator.isNull() == false) {
            if (distanceTravelled <= pathCreator.path.length && distanceTravelled > 1f) {
                distanceTravelled -= speed * Time.deltaTime;
                transform.position = pathCreator.path.GetPointAtDistance(distanceTravelled);
                transform.position += new Vector3(0, 0.5f, 0);
                

                //then rotate
                transform.rotation = pathCreator.path.GetRotationAtDistance(distanceTravelled);

                transform.rotation *= Quaternion.Euler(0, 180, 0);
                transform.position += transform.right * offset;
                
            }
            else {
                //TODO: move onto next spline
                //set distanceTravelled to spline.length
                if (pathCreator.previous() != null && pathCreator.previous().isNull() == false) {
                    pathCreator = pathCreator.previous();
                    distanceTravelled = pathCreator.path.length - 1;
                }
                else if (pathCreator.previous() == null){
                    stopMoving = true;
                }
                else {
                    //Debug.Log("Jetzt hat es alles auseinander genommen, du Hurensohn");
                    Destroy(gameObject);
                }
            } 
        //else if(pathCreator == null || pathCreator.isNull() == true && !stopMoving) {
        //    stopMoving = true;
        //}
            yield return new WaitForSeconds(updateCooldown);
        }
    }

    public void Reset() {
        if (moveCoroutine != null) StopCoroutine(moveCoroutine);
        initialized = false;
        hasStarted = false;
        stopMoving = false;
        offset = 0;
        moveCoroutine = null;
        //pathCreator = null;
        foreach (GameObject coin in coins) {
            Destroy(coin);
        }
    }
    
}


[System.Serializable]
public class CustomSpline {

    [SerializeField] public PathCreator spline;
    [SerializeField] public CustomSpline nextSpline;
    [SerializeField] public CustomSpline previousSpline;


    public VertexPath path {
        get {
            if (spline != null) {
                return spline.path;
            }
            else {
                return null;
            }    
        }    
    }

    public CustomSpline(CustomSpline cSpline) {
        this.spline = cSpline.spline;
    }

    public CustomSpline(PathCreator cSpline) {
        this.spline = cSpline;
    }

    public CustomSpline(CustomSpline cSpline, CustomSpline nextSpline) {
        this.spline = cSpline.spline;
        this.nextSpline = nextSpline;
    }

    public CustomSpline(CustomSpline cSpline, CustomSpline nextSpline, CustomSpline previousSpline) {
        this.spline = cSpline.spline;
        this.nextSpline = nextSpline;
        this.previousSpline = previousSpline;
    }

    public void setNext(CustomSpline nextSpline) {
        this.nextSpline = nextSpline;
        //Debug.Log("UNITY STINKT ZU HÖLLE, WIDERLICH");
    }

    public void setPrevious(CustomSpline previousSpline) {
        this.previousSpline = previousSpline;
    }

    public CustomSpline next() {
        return nextSpline;
    }

    public CustomSpline previous() {
        return previousSpline;
    }

    public Vector3 GetPointAtDistance(float distance) {
        return spline.path.GetPointAtDistance(distance);
    }

    public bool isNull() {
        return spline == null;
    }

    public void Reset() {
        //spline = null;
        nextSpline = null;
        previousSpline = null;
    }
}


public enum TrackType {
    middle,
    left,
    right
}