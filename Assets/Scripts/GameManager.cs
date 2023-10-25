using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : PersistentMonobehaviourSingleton<GameManager>
{
    int matchDuration;
    float startGameplayTime = 0;
    int maxIceBergSize;
    public void GameOver(int newMaxIceBergSize)
    {
        matchDuration = (int) (Time.time - startGameplayTime);
        maxIceBergSize = newMaxIceBergSize;
        GoToEndscreenScene();    
    }

    public void GoToGameplayScene()
    {
        SceneManager.LoadScene("Gameplay");
        startGameplayTime = Time.time;
    }
    public void GoToMainMenuScene()
    {
        SceneManager.LoadScene("MainMenu");
    }
    public void GoToEndscreenScene()
    {
        SceneManager.LoadScene("EndGame");
    }
    public int GetSecondsSurvived()
    {
        return matchDuration;
    }
    public int GetMaxIceBergSize()
    {
        return maxIceBergSize;
    }
}
