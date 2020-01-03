using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DeathMenuScript : MonoBehaviour
{
    public TextMeshProUGUI pointsText;
    void Start()
    {
        if (GameController.PointsThisRound > 0)
            pointsText.text = "Points earned: " + (GameController.PointsThisRound - 1);
        else
            pointsText.text = "Points earned: 0";
    }


    void Update()
    {
        
    }
}
