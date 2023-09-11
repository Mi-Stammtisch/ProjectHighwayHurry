using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimTrigger : MonoBehaviour
{
    
    [SerializeField] GameObject Splash;
    [SerializeField] GameObject Dust;



    public void EnableSplash() {
        Debug.Log("Splash");
    }
    public void EnableDust() {
        Debug.Log("Dust");
    }
}
