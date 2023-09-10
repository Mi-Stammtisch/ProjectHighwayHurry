using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StuntEnabler : MonoBehaviour
{
    [SerializeField] private bool canStunt = false;
    [Tooltip("If you want to add a stunt clip, add it here")]
    [SerializeField] private AnimationClip stuntClip;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StuntController.Instance.canStunt = canStunt;
            if (stuntClip != null)
            {
                StuntController.Instance.stuntClip = stuntClip;
            }
            else
            {
                StuntController.Instance.stuntClip = null;
            }
            
        }
    }
}
