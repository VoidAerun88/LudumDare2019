using UnityEngine;
using UnityEngine.SceneManagement;

public class RootObject : MonoBehaviour
{
    public string Game;
    public string Credits;

    public void LoadGame()
    {
        SceneManager.LoadScene(Game);
    }

    public void LoadCredits()
    {
        SceneManager.LoadScene(Credits);
    }

    public void Exit()
    {
        Application.Quit();
    }
}
