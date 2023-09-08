using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update
    [Range(0, 1)]
    [SerializeField] float TimeScale = 1;
    [SerializeField] GameObject Slider;


    public static GameManager Instance;
    public static event Action PlayerDeath;

    void Awake() {
        Instance = this;
    }

    void Start()
    {
        Application.targetFrameRate = 120;
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

    public void UpdateTimeScale() {
        TimeScale = Slider.GetComponent<UnityEngine.UI.Slider>().value;
    }


    public void playerDeath() {
        TimeScale = 0.2f;
        PlayerDeath?.Invoke();
    }
}
