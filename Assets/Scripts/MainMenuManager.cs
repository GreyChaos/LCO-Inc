using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{

    public void StartNewGame()
    {
        SceneManager.LoadScene(1);
    }

    void LoadGame()
    {

    }

}
