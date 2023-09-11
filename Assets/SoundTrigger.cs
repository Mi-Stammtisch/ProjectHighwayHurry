using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundTrigger : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {


        if (other.gameObject.CompareTag("Player"))
        {


            gameObject.GetComponent<AudioSource>().loop = true;
            gameObject.GetComponent<AudioSource>().Play();


        }
    }

    private void OnTriggerExit(Collider other)
    {

        if (other.gameObject.CompareTag("Player"))
        {
            gameObject.GetComponent<AudioSource>().loop = false;
            gameObject.GetComponent<AudioSource>().Stop();
        }


    }
}
