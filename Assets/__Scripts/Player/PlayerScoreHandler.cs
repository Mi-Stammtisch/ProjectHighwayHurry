using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerScoreHandler : MonoBehaviour
{

    [SerializeField] private GameObject fühlerPosition;
    [SerializeField] private ScoreboardSettings scoreboardSettings;
    [SerializeField] private int numberOfSavedCars = 0;
    [SerializeField] AudioClip CloseCallSound;



    private List<int> alreadyHitCars = new List<int>();
    private int addedScore = 0;

    private int timeBonusIndex = 0;
    private bool hasMoreTimeBoni = true;
    private float time;


    void Start() {
        hasMoreTimeBoni = scoreboardSettings.timeBonusLevels.Count > 0;
        time = Time.time;
    }

    void Update() {
        //raycast from fühlerPosition to the left and to the right
        //raycastsettings from scoreboardsettings list of closecalllevels
        //if the raycast hits a car, add the value of the closecalllevel to the score

        Ray rayLeft = new Ray(fühlerPosition.transform.position, fühlerPosition.transform.TransformDirection(Vector3.left));
        Ray rayRight = new Ray(fühlerPosition.transform.position, fühlerPosition.transform.TransformDirection(Vector3.right));

        //RaycastHit hitLeft;
        //RaycastHit hitRight;

        addedScore = 0;

        bool hitLeftBool;
        bool hitRightBool;

        //hitLeftBool = Physics.Raycast(rayLeft, out hitLeft, scoreboardSettings.closeCallLevels[0].range);
        //hitRightBool = Physics.Raycast(rayRight, out hitRight, scoreboardSettings.closeCallLevels[0].range);

        RaycastHit[] hitsLeft = Physics.RaycastAll(rayLeft, scoreboardSettings.closeCallLevels[0].range);
        RaycastHit[] hitsRight = Physics.RaycastAll(rayRight, scoreboardSettings.closeCallLevels[0].range);

        hitLeftBool = hitsLeft.Length > 0;
        hitRightBool = hitsLeft.Length > 0;

        if (hitLeftBool || hitRightBool) {
            if (hitLeftBool) {
                foreach(RaycastHit hitLeft in hitsLeft) {
                    if (hitLeft.transform.gameObject.CompareTag("Car") && alreadyHitCars.Contains(hitLeft.transform.gameObject.GetHashCode()) == false) {
                        //Debug.Log("HitLeft: " + hitLeft.transform.gameObject.name);
                        addedScore = scoreboardSettings.closeCallLevels[0].value;
                        alreadyHitCars.Add(hitLeft.transform.gameObject.GetHashCode());
                        if (alreadyHitCars.Count > numberOfSavedCars) {
                            alreadyHitCars.RemoveAt(0);
                        }
                        hitRightBool = Physics.RaycastAll(rayRight, scoreboardSettings.closeCallLevels[0].range).Length > 0;
                        foreach (RaycastHit hitRight in hitsRight) {
                            if (hitRightBool && hasHitCar(hitRight)) {
                                //Debug.Log("CloseCallBonus");
                                addedScore += scoreboardSettings.closeCallBonusValue;
                            }
                        }
                    }
                }
            }
            if (hitRightBool) {
                foreach (RaycastHit hitRight in hitsRight) {
                    if (hitRight.transform.gameObject.CompareTag("Car") && alreadyHitCars.Contains(hitRight.transform.gameObject.GetHashCode()) == false) {
                        //Debug.Log("HitRight: " + hitRight.transform.gameObject.name);
                        addedScore = scoreboardSettings.closeCallLevels[0].value;
                        alreadyHitCars.Add(hitRight.transform.gameObject.GetHashCode());
                        if (alreadyHitCars.Count > numberOfSavedCars) {
                            alreadyHitCars.RemoveAt(0);
                        }
                        hitLeftBool = Physics.RaycastAll(rayLeft, scoreboardSettings.closeCallLevels[0].range).Length > 0;
                        foreach (RaycastHit hitLeft in hitsLeft) {
                            if (hitLeftBool && hasHitCar(hitLeft)) {
                                //Debug.Log("CloseCallBonus");
                                addedScore += scoreboardSettings.closeCallBonusValue;
                            }
                        }
                    }
                }
            }
            if (addedScore > 0) {
                Scoreboard.Instance.closeCall(addedScore);
                SoundManager.Instance.PlaySound(CloseCallSound);
            }
        }

        //check for timebonus
        if (hasMoreTimeBoni && (Time.time - time) > scoreboardSettings.timeBonusLevels[timeBonusIndex].time) {
            Scoreboard.Instance.timeBonus(scoreboardSettings.timeBonusLevels[timeBonusIndex].value);
            if (scoreboardSettings.timeBonusLevels.Count - 1 <= timeBonusIndex) hasMoreTimeBoni = false;
            timeBonusIndex++;
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

    void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("JumpStartAirtime")) {
            Scoreboard.Instance.jumpBonus();
        }
    }
}
