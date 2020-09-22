using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    // Configuration parameters
    int currentSceneIndex;

    private void Start()
    {
        currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        if(currentSceneIndex == 0)
        {
            StartCoroutine(SplashScreenLoad());
        }
    }

    private IEnumerator SplashScreenLoad()
    {
        yield return new WaitForSeconds(4.0f);
        LoadNextLevel();
    }

    public void LoadNextLevel()
    {
        currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex + 1);
    }

    public void ReloadLevel()
    {
        currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
    }

    public void LoadStartScene()
    {
        GameManager.Instance.ResetGame();
        SceneManager.LoadScene("Start Menu");
    }

    public void LoadWinScreen()
    {
        SceneManager.LoadScene("Win Scene");
    }

    public void LoadLoseScreen()
    {
        SceneManager.LoadScene("Lose Scene");
    }

    public void Quit()
    {
        Application.Quit();
    }
}
