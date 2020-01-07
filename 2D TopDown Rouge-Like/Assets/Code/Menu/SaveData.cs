using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveData
{

    public int unspentPoints;
    public int pointsInMaxHealth;
    public int pointsInDamage;


    public SaveData(){
        unspentPoints = GameController.UnspentPoints;
        pointsInDamage = GameController.PointsInDamage;
        pointsInMaxHealth = GameController.PointsInMaxHealth;
    }
}
