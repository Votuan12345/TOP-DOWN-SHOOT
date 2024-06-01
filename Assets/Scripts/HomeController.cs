using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class HomeController : MonoBehaviour
{
    public GameObject homeSelect;
    public GameObject levelSelect;

    private void Start()
    {
        if(Time.timeScale == 0) Time.timeScale = 1;

        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
        ShowLevelSelect(false);
    }

    public void StartBtn()
    {
        ShowLevelSelect(true);
    }

    public void QuitBtn()
    {
        Application.Quit();
    }

    public void BackBtn()
    {
        ShowLevelSelect(false);
    }

    public void ShowLevelSelect(bool value)
    {
        if(homeSelect != null)
        {
            homeSelect.SetActive(!value);
        }
        if(levelSelect != null)
        {
            levelSelect.SetActive(value);
        }
    }

    public void LevelBtn(int i)
    {
        SceneManager.LoadScene("Level" + i);
    }
}
