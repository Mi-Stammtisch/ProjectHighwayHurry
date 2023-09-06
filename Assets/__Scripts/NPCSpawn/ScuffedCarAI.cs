using System.Collections;
using System.Collections.Generic;
using System.IO;
using PathCreation;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.UIElements;

public class ScuffedCarAI : MonoBehaviour
{
    Coroutine moveCoroutine;
    [SerializeField] private float minSpeed = 3f;
    [SerializeField] private float maxSpeed = 5f;
    [SerializeField] private GameObject tile;
    [SerializeField] private trackType trackType;
    [SerializeField] private PathCreator previousSpline;
    [SerializeField] private float distanceTravelled;
    [SerializeField] private float speed;
    [SerializeField] private bool moving = false;
    [SerializeField] private bool stopMoving = false;

    [Header("Coins")]
    [SerializeField] private GameObject coinPrefab;
    [SerializeField] private int numberOfCoins;
    [Range(0f, 1f)]
    [SerializeField] private float coinSpawnChance;
    [SerializeField] private float coinSpawnDistanceCar;
    [SerializeField] private float coinSpacing;
    private CustomSpline pathCreator;
    private bool initialized = false;
    private bool hasStarted = false;


    void Start() {
        //pathCreator = new CustomSpline(spline.GetComponent<PathCreator>());
        switch (trackType) {
            case trackType.middle:
                pathCreator = tile.GetComponent<ExitPointDirection>().middleSpline;
                break;
            case trackType.left:
                pathCreator = tile.GetComponent<ExitPointDirection>().leftSpline;
                break;
            case trackType.right:
                pathCreator = tile.GetComponent<ExitPointDirection>().rightSpline;
                break;
        }

        distanceTravelled = Vector3.Magnitude(transform.position - pathCreator.path.GetPointAtDistance(0f));
        //Debug.Log("pathLength: " + pathCreator.path.length);

        transform.position = pathCreator.path.GetPointAtDistance(distanceTravelled);
        transform.rotation = pathCreator.path.GetRotationAtDistance(distanceTravelled);
        transform.rotation *= Quaternion.Euler(0, -90, 0);
        transform.position += new Vector3(0, 0.5f, 0);


        if (pathCreator.previous() != null && pathCreator.previous().isNull() == false) {
            previousSpline = pathCreator.previous().spline;
        }
        else {
            speed = 0;
        }


        
        hasStarted = true;
    }

    void OnTriggerStay(Collider other) {
        if (other.gameObject.tag == "Player" && !initialized && hasStarted) {
            //moveCoroutine ??= StartCoroutine(moveCar(Random.Range(minSpeed, maxSpeed)));
            speed = Random.Range(minSpeed, maxSpeed);
            moving = true;

            //spawn coins
            if (Random.Range(0f, 1f) <= coinSpawnChance) {
                for (int i = 0; i < numberOfCoins; i++) {
                    GameObject coin = Instantiate(coinPrefab, transform.position + transform.right * coinSpawnDistanceCar + transform.right * coinSpacing * i, Quaternion.identity);
                    coin.transform.position += new Vector3(0, 0.5f, 0);
                    coin.transform.parent = transform;
                    GameObject coinBehind = Instantiate(coinPrefab, transform.position + -transform.right * coinSpawnDistanceCar + -transform.right * coinSpacing * i, Quaternion.identity);
                    coinBehind.transform.position += new Vector3(0, 0.5f, 0);
                    coinBehind.transform.parent = transform;
                    //Debug.Log("pathCreatorIsNullBeforeInitializeData: " + (pathCreator == null).ToString());
                    //coin.GetComponent<CoinController>().initializeData(pathCreator, distanceTravelled, speed);
                    //coin.GetComponent<CoinController>().startMoving();
                }
            }

            initialized = true;
        }
    }


    void Update() {
        if (pathCreator.previous() != null && pathCreator.previous().isNull() == false) {
            previousSpline = pathCreator.previous().spline;
        }
        else {
            //speed = 0;
        }



        if (moving && !stopMoving && pathCreator.isNull() == false) {
            if (distanceTravelled <= pathCreator.path.length && distanceTravelled > 1f) {
                distanceTravelled -= speed * Time.deltaTime;
                transform.position = pathCreator.path.GetPointAtDistance(distanceTravelled);
                transform.position += new Vector3(0, 0.5f, 0);

                //rotate in move direction
                //first get point at distance - 1 
                Vector3 point1 = pathCreator.path.GetPointAtDistance(distanceTravelled - 1f);               

                //then get direction vector
                Vector3 direction = transform.position - point1;

                Vector3 direction2 = pathCreator.path.GetDirectionAtDistance(distanceTravelled - 1f);

                //isolate y axis
                

                //rotate direction vector by 90 to left
                direction = Quaternion.Euler(0, -90, 0) * direction;

                //then rotate
                transform.rotation = pathCreator.path.GetRotationAtDistance(distanceTravelled);

                transform.rotation *= Quaternion.Euler(0, -90, 0);
                



                



                
               
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
        }
        else if(pathCreator.isNull() == true && !stopMoving) {
            stopMoving = true;
        }
    }



    private IEnumerator moveCar(float speed) {
        /*
        while (true) {
            transform.position += -transform.right * Time.deltaTime * Random.Range(minSpeed, maxSpeed);
            yield return null;
        }
        */

        distanceTravelled += speed * Time.deltaTime;
        transform.position = pathCreator.path.GetPointAtDistance(distanceTravelled);
        yield return null;
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
}


public enum trackType {
    middle,
    left,
    right
}