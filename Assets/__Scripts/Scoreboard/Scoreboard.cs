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
    private GameObject playerModle;


    void Awake() {
        Instance = this;
    }

    void Start() {
        player = GameObject.Find("CoinPopupParent");
        playerModle = GameObject.Find("PlayerModel");
    }



    public void coinCollect() {
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
        //spawn popup in front of player relative to player position 6, 1, 0
        nr.transform.position = player.transform.position + playerModle.transform.forward * 6 + transform.up * 3;
        nr.transform.SetParent(player.transform);
        nr.GetComponent<FlowtNr>().SetTextAndRotation("+" + value);
    }

    
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        if(player != null)
        Gizmos.DrawWireCube(player.transform.position + playerModle.transform.forward * 6 + transform.up * 3, new Vector3(0.3f, 0.3f, 0.3f));
    }


    public void closeCall(int value) {
        score += value;
        updateScore?.Invoke(score);
    }
}
