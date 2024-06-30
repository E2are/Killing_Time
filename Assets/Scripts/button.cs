using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class button : MonoBehaviour
{
    public void Restart()
    {
        if (GameManager.Instance != null)
        {
            if (GameManager.Instance.Score > PlayerPrefs.GetInt("MaxScore"))
            {
                PlayerPrefs.SetInt("MaxScore", GameManager.Instance.Score);
            }
        }
        Time.timeScale = 1;
        SceneManager.LoadScene(1);
    }

    public void tolobby()
    {
        if (GameManager.Instance.Score > PlayerPrefs.GetInt("MaxScore"))
        {
            PlayerPrefs.SetInt("MaxScore", GameManager.Instance.Score);
        }
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
    }

    public void Quitting()
    {
        if (GameManager.Instance.Score > PlayerPrefs.GetInt("MaxScore"))
        {
            PlayerPrefs.SetInt("MaxScore", GameManager.Instance.Score);
        }
        Time.timeScale = 1;
        Application.Quit();
    }
}
