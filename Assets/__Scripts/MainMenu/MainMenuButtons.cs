using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuButtons : MonoBehaviour
{
    //Instance of the MainMenuButtons
    public static MainMenuButtons Instance;

    
    private void Awake()
    {
        //Set the instance to this
        Instance = this;
    }

    public void PlayButton()
    {
        StartCoroutine(PlayButtonCoroutine());
    }
    IEnumerator PlayButtonCoroutine()
    {
        //Wait for 1 second
        yield return new WaitForSeconds(1f);
        //Load the game scene
        UnityEngine.SceneManagement.SceneManager.LoadScene(1);
    }

    public void QuitButton()
    {
        //Quit the game
        Application.Quit();
    }

    public void CreditsButton()
    {
        //Load the credits scene
        Debug.Log("Credits");
    }

    public void OptionsButton()
    {
        //Load the options scene
        Debug.Log("Options");
    }

    
}
