using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Scoreboard : MonoBehaviour
{
    
    public static Scoreboard Instance;
    [SerializeField] private ScoreboardSettings scoreboardSettings;
    private float time;
    private int coinsCollected = 0;

    public int score = 0;


    public event Action<int> updateScore;



    void Awake() {
        Instance = this;
    }



    public void coinCollect() {
        if (Time.time - time < scoreboardSettings.coinBonusDuration) {
            coinsCollected++;
            score += scoreboardSettings.coinValue;
            score += scoreboardSettings.coinBonusValue * coinsCollected;
            updateScore?.Invoke(score);
        }
        else {
            score += scoreboardSettings.coinValue;
            coinsCollected = 0;
            updateScore?.Invoke(score);
        }
        time = Time.time;
        
    }

    public void closeCall(int value) {
        score += value;
        updateScore?.Invoke(score);
    }
}
