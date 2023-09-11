using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

public class BlendScene : MonoBehaviour
{
    [SerializeField] Image image;
    [SerializeField] float speed = 1;

    public void SStart() {
        StartCoroutine(StartBlender());
    }
    IEnumerator StartBlender() {
        while (image.color != Color.black)
        {
            image.color = Color.Lerp(image.color, Color.black, speed * Time.deltaTime);
            yield return null;
        }         
    }
    
    private void Start()
    {
        StartCoroutine(EndBlender());
    }

    IEnumerator EndBlender() {
        yield return new WaitForSeconds(1);
        while (image.color != Color.clear)
        {
            image.color = Color.Lerp(image.color, Color.clear, speed * Time.deltaTime);
            yield return null;
        }         
    }
}
