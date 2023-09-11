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

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    public void LoadMainMenu()
    {
        //SpawnTileV2.Instance.ResetInstance();
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }
}
