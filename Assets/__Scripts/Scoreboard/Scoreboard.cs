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

    private GameObject player;



    void Awake() {
        Instance = this;
    }

    void Start() {
        player = GameObject.Find("CoinPopupParent");
    }



    public void coinCollect() {
        //collect coins
        //when coins are collected during the threshold, the bonus is multiplied for every coin collected during the threshold
        coinsCollected++;
        if (coinsCollected == 1) {
            time = Time.time;
            score += scoreboardSettings.coinValue;
            createPopup(scoreboardSettings.coinValue);
        }
        else if (coinsCollected > 1) {
            if (Time.time - time < scoreboardSettings.coinBonusDuration) {
                int bonus = scoreboardSettings.coinBonusValue * (coinsCollected - 1);
                score += bonus;
                createPopup(bonus);
            }
            else {
                time = Time.time;
                coinsCollected = 1;
                score += scoreboardSettings.coinValue;
                createPopup(scoreboardSettings.coinValue);
            }
        }
        updateScore?.Invoke(score);
    }


    private void createPopup(int value) {
        GameObject nr = Instantiate(Resources.Load<GameObject>("Nr/FN").gameObject);
        nr.transform.position = player.transform.position + new Vector3(6, 1, 0);
        nr.transform.parent = player.transform;
        nr.GetComponent<FlowtNr>().SetTextAndRotation("+" + value);
    }


    public void closeCall(int value) {
        score += value;
        updateScore?.Invoke(score);
    }
}
