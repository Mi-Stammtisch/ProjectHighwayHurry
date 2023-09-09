using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update
    [Range(0, 1)]
    [SerializeField] float TimeScale = 1;
    [SerializeField] GameObject DeathScreen;
    [SerializeField] GameObject Player;


    public static GameManager Instance;
    public static event Action PlayerDeath;

    void Awake() {
        Instance = this;
    }

    void Start()
    {
        Application.targetFrameRate = 120;
        DeathScreen.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        Time.timeScale = TimeScale;
        //if ESC Quit Game
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }   
    }


    public void playerDeath() {
        TimeScale = 0.2f;
        PlayerDeath?.Invoke();
        StartCoroutine(PlayerDied());

        
    }

    IEnumerator PlayerDied() {

        //Todo: Play death Ragdoll
        //Todo: Play
        Time.timeScale = 0.2f;
        yield return new WaitForSeconds(3f); //wait for ragdoll to fall

        Time.timeScale = 1f;                 

        DeathScreen.transform.SetParent(null);
        DeathScreen.SetActive(true);
        Player.SetActive(false);
        gameObject.SetActive(false);

    }
}
