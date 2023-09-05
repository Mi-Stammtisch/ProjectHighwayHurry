using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RelaodSceneOnCarHit : MonoBehaviour
{
   [SerializeField] private GameObject Explosion;
    private void OnCollisionEnter(Collision other)
    {
        if(other.gameObject.tag == "Car")
        {

            StartCoroutine(ReloadScene());
        }
    }

    IEnumerator ReloadScene()
    {
        GameObject explo = Instantiate(Explosion, transform.position, Quaternion.identity);
        explo.transform.localScale = 5 * Vector3.one;

        yield return new WaitForSeconds(0.5f);
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.buildIndex);
    }
}
