using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SkillCanvasScript : MonoBehaviour
{
    public TextMeshProUGUI pointsUnspent;
    public TextMeshProUGUI pointsInMaxHealth;
    public TextMeshProUGUI pointsInDamage;

    void FixedUpdate()
    {
        pointsUnspent.text = "" + GameController.UnspentPoints;
        pointsInMaxHealth.text = "" + GameController.PointsInMaxHealth;
        pointsInDamage.text = "" + GameController.PointsInDamage;
    }

    public void addMaxHealth()
    {
        if (GameController.UnspentPoints >= 1)
        {
            GameController.UnspentPoints--;
            GameController.PointsInMaxHealth++;
        }
    }
    public void addDamage()
    {
        if (GameController.UnspentPoints >= 1)
        {
            GameController.UnspentPoints--;
            GameController.PointsInDamage++;
        }
    }

    public void backToGame()
    {
        GameController.Health = GameController.MaxHealth;
        this.gameObject.SetActive(false);
    }
}
