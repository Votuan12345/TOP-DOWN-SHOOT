using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameGUIManager : MonoBehaviour
{
    public static GameGUIManager instance;

    public Image healthImage;
    public TextMeshProUGUI healthText;
    public GameObject menuPanel;
    public GameObject winPanel;
    public GameObject gameOverPanel;

    public TextMeshProUGUI killText;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        menuPanel.SetActive(false);
        winPanel.SetActive(false);
        gameOverPanel.SetActive(false);
    }

    public void ShowHealthText(int health, int healthOriginal)
    {
        healthText.text = health.ToString() + " / " + healthOriginal.ToString();
    }

    public void ShowHealthImage(int health, int originalHealth)
    {
        healthImage.fillAmount = (float)health / (float)originalHealth;
    }

    public void ShowKillText(int kill)
    {
        killText.text = "x " + kill.ToString() + " / " + GameManager.instance.enemyMax;
    }

    public void MenuBtn()
    {
        menuPanel.SetActive(true);
        GameManager.instance.ResetMouse();
        Time.timeScale = 0;
    }

    public void Resume()
    {
        menuPanel.SetActive(false);
        GameManager.instance.ChangeMouse();
        Time.timeScale = 1;
    }

    public void HomeBtn()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("Home");
    }

    public void RePlay()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void NextLevel()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void ShowWinPanel()
    {
        GameManager.instance.ResetMouse();
        winPanel.SetActive(true);
    }

    public void ShowGameOverPanel()
    {
        GameManager.instance.ResetMouse();
        gameOverPanel.SetActive(true);
    }
}
