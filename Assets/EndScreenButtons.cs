using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndScreenButtons : MonoBehaviour
{

    public void ReloadCurrentScene()
    {
        Scene scene = UnityEngine.SceneManagement.SceneManager.GetActiveScene();
        //SpawnTileV2.Instance.ResetInstance();
        UnityEngine.SceneManagement.SceneManager.LoadScene(scene.buildIndex);
        
        //UnityEngine.SceneManagement.SceneManager.LoadScene(scene.buildIndex);
    }

    public void LoadMainMenu()
    {
        //SpawnTileV2.Instance.ResetInstance();
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }
}
