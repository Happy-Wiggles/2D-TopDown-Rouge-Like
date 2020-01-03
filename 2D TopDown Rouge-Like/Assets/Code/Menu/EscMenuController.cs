using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EscMenuController : MonoBehaviour
{
    public static bool paused;
    public GameObject escMenu;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (paused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Pause()
    {
        escMenu.SetActive(true);
        Time.timeScale = 0f;
        paused = true;
    }
    public void Resume()
    {
        escMenu.SetActive(false);
        Time.timeScale = 1f;
        paused = false;
    }
    public void EndRun()
    {
        Resume();
        GameController.Health = 0;
    }
    public void MainMenu()
    {

    }
}
