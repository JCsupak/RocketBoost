using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandingPad : MonoBehaviour
{
    // Configuration parameters
    public GameObject fireworkCelebration;
    public IEnumerator levelWonCoroutine;
    float levelDelayTime = 4.0f;

    private void Start()
    {
        levelWonCoroutine = LevelCompleteCelebration();
    }
    public IEnumerator LevelCompleteCelebration()
    {
        fireworkCelebration.SetActive(true);
        GameManager.Instance.IncreaseLives();
        yield return new WaitForSeconds(levelDelayTime);
        FindObjectOfType<LevelLoader>().LoadNextLevel();
    }

}
