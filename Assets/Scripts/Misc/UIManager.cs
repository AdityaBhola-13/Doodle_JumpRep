using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class UIManager : MonoBehaviour
{

    // UI Manager :- Done By Sumedh and adding HighScore :- Done by Aditya
    GameManager gManager;
    public GameObject gameOverPanel;
    public GameObject pausePanel;
    public TextMeshProUGUI score;
    public TextMeshProUGUI highScore;
    private void Start()
    {
        highScore.text = PlayerPrefs.GetInt("HighScore").ToString();
    }
    public void ToLoadScene(int loadScene)
    {
        SceneManager.LoadScene(loadScene);
    }
    public void OnPauseButtonClicked()
    {
        Time.timeScale = 0;
        pausePanel.gameObject.SetActive(true);
    }
    public void OnResumeButtonClicked()
    {
        Time.timeScale = 1;
        pausePanel.gameObject.SetActive(false);
    }
    public void OnGameOver()
    {
        gameOverPanel.SetActive(true);
        score.text = GameObject.Find("GameManager").GetComponent<GameManager>().score.ToString();
        highScore.text = PlayerPrefs.GetInt("HighScore").ToString();
    }
    public void onPlayAgainButton()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void QuitButton()
    {
        Application.Quit();
    }
}
