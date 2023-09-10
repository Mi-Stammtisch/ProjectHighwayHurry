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
    [SerializeField] GameObject PlayerHud;
    [SerializeField] GameObject EndScoreDisplay;

    

    [SerializeField] private GameObject PlayerCollider;


    public static GameManager Instance;
    public static event Action PlayerDeath;

    public int Health = 1;

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

        if (Health > 0) {
            StartCoroutine(TakeDamage());
            Health--;
            return;
        }
        
        PlayerDeath?.Invoke();
        StartCoroutine(PlayerDied());

        
    }
    IEnumerator TakeDamage() 
    {
        TimeScale = 0.6f;
        PlayerCollider.GetComponent<Collider>().enabled = false;
        yield return new WaitForSeconds(0.5f);
        PlayerCollider.GetComponent<Collider>().enabled = true;
        TimeScale = 1f;        
    }

    IEnumerator PlayerDied() {

        
        Time.timeScale = 0.2f;
        yield return new WaitForSeconds(1.5f); //wait for ragdoll to fall
        PlayerHud.SetActive(false);
        Time.timeScale = 1f;    
        EndScoreDisplay.GetComponent<Text3D>().SetText(Scoreboard.Instance.score.ToString());          

        DeathScreen.transform.SetParent(null);
        DeathScreen.SetActive(true);
        Player.SetActive(false);
        gameObject.SetActive(false);

    }
}
