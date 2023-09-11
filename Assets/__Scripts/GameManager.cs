using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update
    [Range(0, 1)]
    [SerializeField] float TimeScale = 1;
    [SerializeField] public static float DerBessereTimeScale = 0f;
    [SerializeField] GameObject DeathScreen;
    [SerializeField] GameObject Player;
    [SerializeField] GameObject PlayerHud;
    [SerializeField] GameObject EndScoreDisplay;
    [SerializeField] GameObject BikeBlinker;
    [SerializeField] AudioClip PlayerHit;
    [SerializeField] TMP_Text hptext;
    [SerializeField] TMP_Text Speedtext;



    [Header("StartGame")]
    [SerializeField] GameObject StartScreen;
    [SerializeField] GameObject TileScreen;
    [SerializeField] GameObject PlayerPrefab;
    [SerializeField] GameObject EnviSpawner;

    [SerializeField] private bool immortality = false;




    

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
        DerBessereTimeScale = 0f;
        
        //hide cursor
        //Cursor.visible = false;
        StartCoroutine(StartGame());
    }

    IEnumerator StartGame() {
        DeathScreen.SetActive(false);
        StartScreen.SetActive(true);
        yield return new WaitForSeconds(6f);
        DerBessereTimeScale = 1f;  
        StartScreen.SetActive(false);
        //PlayerPrefab.SetActive(true);
        //TileScreen.SetActive(true);
        //EnviSpawner.SetActive(true);
        
        
        
        
       
    }





    [EButton("Add Health")]
    public void AddHealth() {
        Health++;
    }
    public void UpdateSpeed(float speed) {
        Speedtext.text = speed.ToString();
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
        hptext.text = Health.ToString();  
    }

    [EButton("Player Damage")]
    public void playerDeath() {

        SoundManager.Instance.PlaySound(PlayerHit);
        if (Health > 0) {
            StartCoroutine(TakeDamage());

            if (immortality) return;
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
        
        for (int i = 0; i < 3; i++)
        {
            BikeBlinker.SetActive(true);
            yield return new WaitForSeconds(0.1f);
            BikeBlinker.SetActive(false);
            yield return new WaitForSeconds(0.1f);            
        }

        
        
        PlayerCollider.GetComponent<Collider>().enabled = true;
        TimeScale = 1f;        
    }

    IEnumerator PlayerDied() {

        
        Time.timeScale = 0.2f;
        yield return new WaitForSeconds(1.5f); //wait for ragdoll to fall
        PlayerHud.SetActive(false);
        Time.timeScale = 1f;    
        //display cursor
        Cursor.visible = true;
        EndScoreDisplay.GetComponent<Text3D>().SetText(Scoreboard.Instance.score.ToString());          

        DeathScreen.transform.SetParent(null);
        DeathScreen.SetActive(true);
        Player.SetActive(false);
        gameObject.SetActive(false);

    }
}
