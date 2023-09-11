using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScuffedLod : MonoBehaviour
{
    [Tooltip("Distance from camera to object, when object will be scaled to zero")]
    private float Distance = 100f;

    [Tooltip("How often to check distance to camera")]
    [SerializeField] float ckeckRate = 0.5f;

    [Tooltip("How long to scale object to zero or to present scale")]
    [SerializeField] private float ScaleTimeToComplete = 0.5f;
    private Vector3 PresentScale;


    private void Start()
    {
        PresentScale = transform.localScale;
        ChekDistance();
    }



    IEnumerator ChekDistance()
    {
        while (true)
        {
            yield return new WaitForSeconds(ckeckRate);
            if (Vector3.Distance(transform.position, Camera.main.transform.position) > Distance)
            {
                LeanTween.cancel(gameObject);
                LeanTween.scale(gameObject, Vector3.zero, ScaleTimeToComplete);
                yield return new WaitForSeconds(ScaleTimeToComplete);
                for (int i = 0; i < transform.childCount; i++)
                {
                    transform.GetChild(i).gameObject.SetActive(false);
                }
                
                
            }
            else
            {
                LeanTween.cancel(gameObject);
                for (int i = 0; i < transform.childCount; i++)
                {
                    transform.GetChild(i).gameObject.SetActive(true);
                }
                LeanTween.scale(gameObject, PresentScale, ScaleTimeToComplete).setEaseInBounce();
            }
        }
    }
}
