using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class MainMenuButtons : MonoBehaviour
{
    //Instance of the MainMenuButtons
    public static MainMenuButtons Instance;
    [SerializeField] GameObject MainMenuCamera;
    [SerializeField] GameObject CreditsCamera;
    [SerializeField] GameObject OptionsCamera;
    [SerializeField] GameObject PlayCreditsAnim;

    
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
        PlayCreditsAnim.GetComponent<Animation>().Rewind();
        CreditsCamera.GetComponent<CinemachineVirtualCamera>().Priority = 15;
        
        PlayCreditsAnim.GetComponent<Animation>().Play();

        //Load the credits scene
        //Debug.Log("Credits");
    }

    public void OptionsButton()
    {
        OptionsCamera.GetComponent<CinemachineVirtualCamera>().Priority = 15;
        //Load the options scene
       // Debug.Log("Options");
    }

[EButton("Back")]
    public void BackButton()
    {
        PlayCreditsAnim.GetComponent<Animation>().Rewind();
        OptionsCamera.GetComponent<CinemachineVirtualCamera>().Priority = 5;
        CreditsCamera.GetComponent<CinemachineVirtualCamera>().Priority = 5;
        //Load the main menu scene
        //Debug.Log("Back");
    }

    
}
