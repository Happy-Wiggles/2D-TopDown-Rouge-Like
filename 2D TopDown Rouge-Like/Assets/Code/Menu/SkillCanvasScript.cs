using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SkillCanvasScript : MonoBehaviour
{
    public TextMeshProUGUI pointsUnspent;
    public TextMeshProUGUI pointsInMaxHealth;

    void FixedUpdate()
    {
        pointsUnspent.text = "" + GameController.UnspentPoints;
        pointsInMaxHealth.text = "" + GameController.PointsInMaxHealth;
    }

    public void addMaxHealth()
    {
        if (GameController.UnspentPoints >= 1)
        {
            GameController.UnspentPoints--;
            GameController.PointsInMaxHealth++;
        }
    }

    public void backToGame()
    {
        GameController.Health = GameController.MaxHealth;
        this.gameObject.SetActive(false);
    }
}
