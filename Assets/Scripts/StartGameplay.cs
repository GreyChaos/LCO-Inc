using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGameplay : MonoBehaviour
{
    public void StartDay(int sceneID)
    {
        SceneManager.LoadScene(sceneID);
        TimeManager.StartNextDay();
    }
}
