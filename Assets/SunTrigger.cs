using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SunTrigger : MonoBehaviour
{
    
    [SerializeField] bool Suns = true;
    private void OnTriggerEnter(Collider other)
    {
        Sun.Instance.LightToggle(Suns);
    }
}
