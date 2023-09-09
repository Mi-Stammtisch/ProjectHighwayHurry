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

            
            GameObject explo = Instantiate(Explosion, transform.position, Quaternion.identity);
            explo.transform.localScale = 5 * Vector3.one;
            Destroy(explo, 5);
            GameManager.Instance.playerDeath();
            Debug.Log("Car Hit");            
        }
    }

    IEnumerator ReloadScene()
    {
        
        

        yield return new WaitForSeconds(0.5f);
        //Scene scene = SceneManager.GetActiveScene();
        //SceneManager.LoadScene(scene.buildIndex);
    }
}
