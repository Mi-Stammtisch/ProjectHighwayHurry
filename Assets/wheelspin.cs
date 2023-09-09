using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class wheelspin : MonoBehaviour
{
    [SerializeField] private GameObject wheel1;
    [SerializeField] private GameObject wheel2;
    [SerializeField] private float speed = 1f;
    
    IEnumerator Start()
    {
        while(true)
        {
            //rotate wheels on x axis in which speed is multiplied by time and wheel size
            wheel1.transform.Rotate(speed * Time.deltaTime, 0, 0);
            wheel2.transform.Rotate(speed * Time.deltaTime, 0, 0);
            yield return null;
            
        }
    }
}
