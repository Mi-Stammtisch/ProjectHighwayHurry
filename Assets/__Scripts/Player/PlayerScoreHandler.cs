using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerScoreHandler : MonoBehaviour
{

    [SerializeField] private GameObject fühlerPosition;
    [SerializeField] private ScoreboardSettings scoreboardSettings;
    [SerializeField] private int numberOfSavedCars = 0;



    private List<int> alreadyHitCars = new List<int>();
    private int addedScore = 0;



    void Update() {
        //raycast from fühlerPosition to the left and to the right
        //raycastsettings from scoreboardsettings list of closecalllevels
        //if the raycast hits a car, add the value of the closecalllevel to the score

        Ray rayLeft = new Ray(fühlerPosition.transform.position, fühlerPosition.transform.TransformDirection(Vector3.left));
        Ray rayRight = new Ray(fühlerPosition.transform.position, fühlerPosition.transform.TransformDirection(Vector3.right));

        RaycastHit hitLeft;
        RaycastHit hitRight;

        addedScore = 0;

        /*
        foreach (CloseCallLevels closeCallLevel in scoreboardSettings.closeCallLevels) {
            if (Physics.Raycast(rayLeft, out hitLeft, closeCallLevel.range)) {
                if (hitLeft.transform.gameObject.CompareTag("Car") && alreadyHitCars.Contains(hitLeft.transform.gameObject) == false) {
                    Debug.Log("HitLeft: " + hitLeft.transform.gameObject.name);
                    //Scoreboard.Instance.closeCall(closeCallLevel.value);
                    if (addedScore < closeCallLevel.value) {
                        addedScore = closeCallLevel.value;
                    }
                    alreadyHitCars.Add(hitLeft.transform.gameObject);
                    if (alreadyHitCars.Count > numberOfSavedCars) {
                        alreadyHitCars.RemoveAt(0);
                    }

                }
            }
            if (Physics.Raycast(rayRight, out hitRight, closeCallLevel.range)) {
                if (hitRight.transform.gameObject.CompareTag("Car") && alreadyHitCars.Contains(hitRight.transform.gameObject) == false) {
                    Debug.Log("HitRight: " + hitRight.transform.gameObject.name);
                    //Scoreboard.Instance.closeCall(closeCallLevel.value);
                    if (addedScore < closeCallLevel.value) {
                        addedScore = closeCallLevel.value;
                    }
                    alreadyHitCars.Add(hitRight.transform.gameObject);
                    if (alreadyHitCars.Count > numberOfSavedCars) {
                        alreadyHitCars.RemoveAt(0);
                    }
                }
            }
        }
        Scoreboard.Instance.closeCall(addedScore);
        */

        bool hitLeftBool;
        bool hitRightBool;

        hitLeftBool = Physics.Raycast(rayLeft, out hitLeft, scoreboardSettings.closeCallLevels[0].range);
        hitRightBool = Physics.Raycast(rayRight, out hitRight, scoreboardSettings.closeCallLevels[0].range);

        if (hitLeftBool || hitRightBool) {
            if (hitLeftBool) {
                if (hitLeft.transform.gameObject.CompareTag("Car") && alreadyHitCars.Contains(hitLeft.transform.gameObject.GetHashCode()) == false) {
                    Debug.Log("HitLeft: " + hitLeft.transform.gameObject.name);
                    //Scoreboard.Instance.closeCall(closeCallLevel.value);
                    addedScore = scoreboardSettings.closeCallLevels[0].value;
                    alreadyHitCars.Add(hitLeft.transform.gameObject.GetHashCode());
                    if (alreadyHitCars.Count > numberOfSavedCars) {
                        alreadyHitCars.RemoveAt(0);
                    }
                    hitRightBool = Physics.Raycast(rayRight, out hitRight, scoreboardSettings.closeCallLevels[0].range);
                    if (hitRightBool && hasHitCar(hitRight)) {
                        Debug.Log("CloseCallBonus");
                        addedScore += scoreboardSettings.closeCallBonusValue;
                    }
                }
            }
            if (hitRightBool) {
                if (hitRight.transform.gameObject.CompareTag("Car") && alreadyHitCars.Contains(hitRight.transform.gameObject.GetHashCode()) == false) {
                    Debug.Log("HitRight: " + hitRight.transform.gameObject.name);
                    //Scoreboard.Instance.closeCall(closeCallLevel.value);
                    addedScore = scoreboardSettings.closeCallLevels[0].value;
                    alreadyHitCars.Add(hitRight.transform.gameObject.GetHashCode());
                    if (alreadyHitCars.Count > numberOfSavedCars) {
                        alreadyHitCars.RemoveAt(0);
                    }
                    hitLeftBool = Physics.Raycast(rayLeft, out hitLeft, scoreboardSettings.closeCallLevels[0].range);
                    if (hitLeftBool && hasHitCar(hitLeft)) {
                        Debug.Log("CloseCallBonus");
                        addedScore += scoreboardSettings.closeCallBonusValue;
                    }
                }
            }
            if (addedScore > 0) {
                Scoreboard.Instance.closeCall(addedScore);
            }
        }

        
    }

    private bool hasHitCar(RaycastHit hit) {
        return hit.transform.gameObject.CompareTag("Car");
    }


    private void OnDrawGizmos() {
        //draw gizmos for the raycasts with the range of the closecalllevels
        int i = 0;
        foreach (CloseCallLevels closeCallLevel in scoreboardSettings.closeCallLevels) {
            Gizmos.color = closeCallLevel.color;
            Gizmos.DrawRay(fühlerPosition.transform.position, fühlerPosition.transform.TransformDirection(Vector3.left) * closeCallLevel.range + new Vector3(0, i * 0.1f, 0));
            Gizmos.DrawRay(fühlerPosition.transform.position, fühlerPosition.transform.TransformDirection(Vector3.right) * closeCallLevel.range + new Vector3(0, i * 0.1f, 0));
            i++;
        }
    }
}
