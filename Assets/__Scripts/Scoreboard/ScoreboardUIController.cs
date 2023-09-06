using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreboardUIController : MonoBehaviour
{
    
    [SerializeField] private TMPro.TextMeshProUGUI scoreText;


    void Start() {
        Scoreboard.Instance.updateScore += (score) => {
            scoreText.text = score.ToString();};
    }
}