using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Game : MonoBehaviour
{
    public enum GameState
    {
        Ready,
        Playing,
        Win,
        Lose
    }
    protected GameState m_gameState;

    [SerializeField]
    private DatabaseManager dbManager;

    protected int score = 0;

    [SerializeField]
    GameObject gameElements;
    [SerializeField]
    GameObject gameoverElements;
    [SerializeField]
    Text scoreText;

    public void Start()
    {
        Time.timeScale = 0;
    }

    public void StartGame()
    {
        if (m_gameState == GameState.Ready)
        {
            m_gameState = GameState.Playing;
            Time.timeScale = 1;
            Debug.Log("Game state set to playing");
        }
        else
        {
            Debug.LogError("Game state is not in ready: " + m_gameState);
        }
    }

    public void WinGame(int score)
    {
        Debug.Log("wingame called");
        if (m_gameState == GameState.Playing)
        {
            StartCoroutine(dbManager.SaveScore(PlayerPrefs.GetInt("worldid"), PlayerPrefs.GetInt("levelid"), PlayerPrefs.GetString("username"), score, SaveScoreCallback));
            m_gameState = GameState.Win;
            Debug.Log("Game state set to Win");
            scoreText.text = "Score: " + score;
            gameElements.SetActive(false);
            gameoverElements.SetActive(true);
        }
        else
        {
            Debug.LogError("Game state is not in playing: " + m_gameState);
        }
    }

    public void LoseGame(int score)
    {
        if (m_gameState == GameState.Playing)
        {
            StartCoroutine(dbManager.SaveScore(PlayerPrefs.GetInt("worldid"), PlayerPrefs.GetInt("levelid"), PlayerPrefs.GetString("username"), score, SaveScoreCallback));
            m_gameState = GameState.Lose;
            Debug.Log("Game state set to Lose");
        }
        else
        {
            Debug.LogError("Game state is not in playing: " + m_gameState);
        }
    }

    
    
    protected void SaveScoreCallback(bool success, string message) {
        Debug.Log(message);

        if(success) {
        }
    }

    public void ReturnToMenu()
    {
        PlayerPrefs.SetInt("State", (int)MainMenuController.MenuState.Level);
        SceneManager.LoadScene("MainMenu");
    }
}
