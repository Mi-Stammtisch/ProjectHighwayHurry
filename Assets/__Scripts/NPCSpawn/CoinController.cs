using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CoinController : MonoBehaviour
{
    
    [SerializeField] private CustomSpline pathCreator;
    [SerializeField] private float distanceTravelled;
    [SerializeField] private float speed;    
    [SerializeField] private bool moving = false;

    public void initializeData(CustomSpline spline, float distanceTravelled, float speed) {
        pathCreator = spline;
        Debug.Log("pathCreatorIsNull: " + (pathCreator == null).ToString());
        this.distanceTravelled = distanceTravelled;
        Debug.Log("distanceTravelled: " + distanceTravelled);
        this.speed = speed;
        Debug.Log("speed: " + speed);
    }

    public void startMoving() {
        moving = true;
        StartCoroutine(customUpdate());
        Debug.Log("started moving");
    }

    // Update is called once per frame
    IEnumerator customUpdate() {
        if (moving && pathCreator.isNull() == false) {
            if (distanceTravelled <= pathCreator.path.length && distanceTravelled > 1f) {
                distanceTravelled -= speed * Time.deltaTime;
                transform.position = pathCreator.path.GetPointAtDistance(distanceTravelled);
                transform.position += new Vector3(0, 0.5f, 0);
            }
            else {
                //TODO: move onto next spline
                //set distanceTravelled to spline.length
                if (pathCreator.previous() != null && pathCreator.previous().isNull() == false) {
                    pathCreator = pathCreator.previous();
                    distanceTravelled = pathCreator.path.length - 1;
                }
                else if (pathCreator.previous() == null){
                    moving = false;
                }
                else {
                    //Debug.Log("Jetzt hat es alles auseinander genommen, du Hurensohn");
                    Destroy(gameObject);
                }
            } 
        }
        else if(pathCreator.isNull() == true) {
            moving = false;
        }

        yield return null;
    }
}
