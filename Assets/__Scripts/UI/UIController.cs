using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour
{
    
    [SerializeField] private GameObject scoreText;
    [SerializeField] private GameObject deathScreen;

    void Awake() {
        deathScreen.SetActive(false);
    }

    void Start() {
        GameManager.PlayerDeath += onPlayerDeath;
        Scoreboard.Instance.updateScore += (score) => {
            scoreText.GetComponent<TMPro.TextMeshProUGUI>().text = score.ToString();
        };
    }


    private void onPlayerDeath() {
        if (scoreText != null) {
            scoreText.SetActive(false);
        }
        else {
            Debug.LogError("Was ist denn hiiiiiiiiieeeeeeeeeeeeer los?");
        }
        
        deathScreen.SetActive(true);
        deathScreen.transform.transform.GetChild(2).GetComponent<TMPro.TextMeshProUGUI>().text = Scoreboard.Instance.score.ToString();
    }

    public void restartGame() {
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
    }
}