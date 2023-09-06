using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerScoreHandler : MonoBehaviour
{

    [SerializeField] private GameObject fühlerPosition;
    [SerializeField] private ScoreboardSettings scoreboardSettings;
    [SerializeField] private int numberOfSavesCars = 0;



    private List<GameObject> alredyHitCars = new List<GameObject>();
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

        foreach (CloseCallLevels closeCallLevel in scoreboardSettings.closeCallLevels) {
            if (Physics.Raycast(rayLeft, out hitLeft, closeCallLevel.range)) {
                if (hitLeft.transform.gameObject.CompareTag("Car") && alredyHitCars.Contains(hitLeft.transform.gameObject) == false) {
                    Debug.Log("HitLeft: " + hitLeft.transform.gameObject.name);
                    //Scoreboard.Instance.closeCall(closeCallLevel.value);
                    if (addedScore < closeCallLevel.value) {
                        addedScore = closeCallLevel.value;
                    }
                    alredyHitCars.Add(hitLeft.transform.gameObject);
                    if (alredyHitCars.Count > numberOfSavesCars) {
                        alredyHitCars.RemoveAt(0);
                    }

                }
            }
            if (Physics.Raycast(rayRight, out hitRight, closeCallLevel.range)) {
                if (hitRight.transform.gameObject.CompareTag("Car") && alredyHitCars.Contains(hitRight.transform.gameObject) == false) {
                    Debug.Log("HitRight: " + hitRight.transform.gameObject.name);
                    //Scoreboard.Instance.closeCall(closeCallLevel.value);
                    if (addedScore < closeCallLevel.value) {
                        addedScore = closeCallLevel.value;
                    }
                    alredyHitCars.Add(hitRight.transform.gameObject);
                    if (alredyHitCars.Count > numberOfSavesCars) {
                        alredyHitCars.RemoveAt(0);
                    }
                }
            }
        }
        Scoreboard.Instance.closeCall(addedScore);
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
