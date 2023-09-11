using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaggerController : MonoBehaviour
{

    [SerializeField] private BaggerTrigger baggerTrigger;
    [SerializeField] private GameObject baggerModel;
    private bool drive = false;
    [SerializeField] private float baggerSpeed = 5f;
    [SerializeField] private float driveLength = 5f;


    private Vector3 initialPosition;
    float initialDriveLength;

    // Start is called before the first frame update
    void Start()
    {
        drive = false;
        initialPosition = transform.localPosition;
        initialDriveLength = driveLength;
        if(transform.localPosition.x > 0){
            baggerSpeed = -baggerSpeed;
            baggerModel.transform.rotation = Quaternion.Euler(-89.98f, 180, 0);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(!drive){
            drive = baggerTrigger.getTriggered();
        }
        else if(driveLength > 0){
            transform.Translate(Vector3.right * Time.deltaTime * GameManager.DerBessereTimeScale * baggerSpeed);
            driveLength -= Time.deltaTime * GameManager.DerBessereTimeScale;
        }
    }

    public void resetBagger(){
        drive = false;
        driveLength = initialDriveLength;
        transform.localPosition = initialPosition;
        baggerTrigger.isTriggered = false;
    }
}
