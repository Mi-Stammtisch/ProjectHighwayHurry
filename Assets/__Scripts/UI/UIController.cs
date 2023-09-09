using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIController : MonoBehaviour
{

    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private GameObject deathScreen;

    void Awake()
    {
        deathScreen.SetActive(false);
    }

    void Start()
    {
        //GameManager.PlayerDeath += onPlayerDeath;


        Scoreboard.Instance.updateScore += (score) =>
        {
            scoreText.text = score.ToString();
        };

    }


    private void onPlayerDeath()
    {
        if (scoreText != null)
        {
            scoreText.gameObject.SetActive(false);
        }
        


        if (deathScreen != null)
        {
            deathScreen.SetActive(true);
            deathScreen.transform.transform.GetChild(2).GetComponent<TMP_Text>().text = Scoreboard.Instance.score.ToString();
        }
        

    }

    public void restartGame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
    }
}