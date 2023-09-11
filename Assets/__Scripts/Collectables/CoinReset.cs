using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinReset : MonoBehaviour
{
    
    [SerializeField] private ExitPointDirection exitPointDirection;
    [SerializeField] private GameObject coin;

    private Vector3[] initialPosition;
    private Quaternion[] initialRotation;


    void Start() {
        exitPointDirection.onReset += resetCoin;

        initialPosition = new Vector3[transform.childCount];
        initialRotation = new Quaternion[transform.childCount];

        for(int i = 0; i < transform.childCount; i++){
            initialPosition[i] = transform.GetChild(i).localPosition;
            initialRotation[i] = transform.GetChild(i).localRotation;
        }
    }

    void OnDestroy(){
        exitPointDirection.onReset -= resetCoin;
    }


    public void resetCoin(){
        for(int i = 0; i < initialPosition.Length; i++){
            GameObject obj = Instantiate(coin, transform);
            obj.transform.localPosition = initialPosition[i];
            obj.transform.localRotation = initialRotation[i];
        }
    }
}
