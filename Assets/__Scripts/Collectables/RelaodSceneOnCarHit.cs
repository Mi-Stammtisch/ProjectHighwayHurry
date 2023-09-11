using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class RelaodSceneOnCarHit : MonoBehaviour
{
    [SerializeField] private GameObject Explosion;
    
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Car")
        {


            GameObject explo = Instantiate(Explosion, transform.position, Quaternion.identity);

            CinemachineImpulseSource imp;
            if (gameObject.AddComponent<CinemachineImpulseSource>() == null)
            {
                imp = gameObject.AddComponent<CinemachineImpulseSource>();
            }
            else imp = gameObject.GetComponent<CinemachineImpulseSource>();
            



            imp.m_ImpulseDefinition.m_ImpulseType = CinemachineImpulseDefinition.ImpulseTypes.Dissipating;
            imp.m_ImpulseDefinition.m_ImpulseShape = CinemachineImpulseDefinition.ImpulseShapes.Rumble;

            imp.GenerateImpulse();
            explo.transform.localScale = 5 * Vector3.one;
            Destroy(explo, 5);
            GameManager.Instance.playerDeath();
            //Debug.Log("Car Hit");            
        }
    }

    IEnumerator ReloadScene()
    {



        yield return new WaitForSeconds(0.5f);
        //Scene scene = SceneManager.GetActiveScene();
        //SceneManager.LoadScene(scene.buildIndex);
    }
}
