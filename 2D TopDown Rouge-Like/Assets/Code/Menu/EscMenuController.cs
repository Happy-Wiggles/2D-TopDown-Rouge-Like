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

    public void Save()
    {
        SaveSystem.SaveGame();
    }
    
    public void MainMenu()
    {
        if(GameController.PointsThisRound>0)
            GameController.UnspentPoints = GameController.UnspentPoints + GameController.PointsThisRound - 1;
        Destroy(GameObject.Find("UICanvas"));
        Destroy(GameObject.Find("Main Camera"));
        Destroy(GameObject.Find("Player"));
        Destroy(GameObject.Find("EscCanvas"));
        SaveSystem.SaveGame();
        Destroy(GameObject.Find("GameController"));
        SceneManager.LoadScene("Menu");
    }
}
