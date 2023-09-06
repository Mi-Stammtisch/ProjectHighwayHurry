using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Scoreboard : MonoBehaviour
{
    
    public static Scoreboard Instance;
    [SerializeField] private ScoreboardSettings scoreboardSettings;

    public int score = 0;


    public event Action<int> updateScore;



    void Awake() {
        Instance = this;
    }



    public void coinCollect() {
        score += scoreboardSettings.coinValue;
        updateScore?.Invoke(score);
    }

    public void closeCall(int value) {
        score += value;
        updateScore?.Invoke(score);
    }
}
