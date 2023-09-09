using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FlowtNr : MonoBehaviour
{
    
    public void SetTextAndRotation(string text)
    {
        gameObject.transform.GetChild(0).GetComponent<TMP_Text>().text = text;
        //rotate the text to face the camera main
        transform.rotation = Quaternion.LookRotation(transform.position - Camera.main.transform.position);

        //move the text up
        LeanTween.moveLocalY(gameObject, 3f, 0.5f).setOnComplete(() =>
        {
            //destroy the text
            Destroy(gameObject);
        });
    }
}
