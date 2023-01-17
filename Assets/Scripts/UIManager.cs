using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public PlayerController playerController;
    public bool gameOver;
    public Text ScoreTXT;

    public int score = 0;

    private bool hasAlreadyTriggered = false;

    public GameObject gameOverOverlay;
    private Animator _gameOverAnimator;

    private void Awake()
    {
        _gameOverAnimator = gameOverOverlay.GetComponent<Animator>();
    }

    private void Update()
    {
        if(playerController == null && !hasAlreadyTriggered)
        {
            gameOver = true;
        }
        else
        {
            gameOver = false;
        }

        if (gameOver)
        {
            gameOverOverlay.SetActive(true);
            GameOver();
        }
    }

    public void GameOver()
    {
        ScoreTXT.text = "KILLS : " + playerController.myScore.ToString();
        hasAlreadyTriggered = true;
        _gameOverAnimator.Play("GameOver_DropDown");
        gameOver = false;
    }

    public void RestartLevel()
    {
        string levelName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(levelName);
    }

}
