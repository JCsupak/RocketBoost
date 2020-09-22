using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance = null;
    
    public static GameManager Instance
    {
        get
        {
            if(_instance == null)
            {
                _instance = FindObjectOfType<GameManager>();
                if(_instance == null)
                {
                    GameObject go = new GameObject("Game Manager");
                    _instance = go.AddComponent<GameManager>();
                }
            }
            return _instance;
        }
    }

    // Configuration parameters
    int maxLives = 5;

    // State parameters
    public enum Difficulty { Easy, Medium, Hard};

    Difficulty gameDifficulty;
    int currentLives;

    private void Awake()
    {
        if(_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        ResetGame();
    }

    public void IncreaseLives()
    {
        int addLives;

        switch (gameDifficulty)
        {
            case Difficulty.Easy:
                addLives = 2;
                break;
            case Difficulty.Hard:
                addLives = 0;
                break;
            default:
                addLives = 1;
                break;
        }

        currentLives += addLives;
    }

    public void DecreaseLives()
    {
        currentLives--;

        if (currentLives <= 0)
        {
            FindObjectOfType<LevelLoader>().LoadLoseScreen();
        }
        else
        {
            FindObjectOfType<LevelLoader>().ReloadLevel();
        }
    }

    public int GetCurrentLives()
    {
        return currentLives;
    }

    public void SetGameDifficulty(Difficulty currDiff)
    {
        gameDifficulty = currDiff;
    }

    public Difficulty GetGameDifficulty()
    {
        return gameDifficulty;
    }

    public void ResetGame()
    {
        gameDifficulty = Difficulty.Medium;
        currentLives = maxLives;
    }
}
