using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConeReset : MonoBehaviour
{
    Vector3 initialPosition;
    Quaternion initialRotation;

    void Start() {
        transform.parent.GetComponent<TileReferenceHolder>().exitPointDirection.onReset += resetCone;

        initialPosition = transform.localPosition;
        initialRotation = transform.localRotation;

    }


    public void resetCone(){
        try {
            Destroy(GetComponent<Rigidbody>());
        }
        catch {}


        transform.localPosition = initialPosition;
        transform.localRotation = initialRotation;
    }


    void OnDestroy(){
        transform.parent.GetComponent<TileReferenceHolder>().exitPointDirection.onReset -= resetCone;
    }


}
