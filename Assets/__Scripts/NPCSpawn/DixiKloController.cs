using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DixiKloController : MonoBehaviour
{

    [SerializeField] private BaggerTrigger dixiTrigger;
    private bool drive = false;
    [SerializeField] private float dixiSpeed = 5f;
    [SerializeField] private float driveLength = 10f;

    private Vector3 initialPosition;
    float initialDriveLength;

    // Start is called before the first frame update
    void Start()
    {
        drive = false;
        initialPosition = transform.localPosition;
        initialDriveLength = driveLength;

        transform.parent.GetComponent<TileReferenceHolder>().exitPointDirection.onReset += resetDixiKlo;
    }

    // Update is called once per frame
    void Update()
    {
        if(!drive){
            drive = dixiTrigger.getTriggered();
        }
        else if(driveLength > 0){
            transform.Translate(Vector3.forward * Time.deltaTime * dixiSpeed);
            driveLength -= Time.deltaTime;
        }
    }

        public void resetDixiKlo(){
        drive = false;
        driveLength = initialDriveLength;
        transform.localPosition = initialPosition;
        dixiTrigger.isTriggered = false;
    }

    void OnDestroy(){
        transform.parent.GetComponent<TileReferenceHolder>().exitPointDirection.onReset -= resetDixiKlo;
    }
}
