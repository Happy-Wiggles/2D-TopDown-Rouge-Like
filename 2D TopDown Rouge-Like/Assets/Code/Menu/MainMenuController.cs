using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{

    public void Play()
    {
        SceneManager.LoadScene("Start");
        SceneManager.LoadSceneAsync("Hub", LoadSceneMode.Additive);
    }

    public void Load()
    {
        Play();
        SaveSystem.LoadGame();
    }



    public void Quit()
    {
        Debug.Log("QUIT!");
        Application.Quit();
    }
}
