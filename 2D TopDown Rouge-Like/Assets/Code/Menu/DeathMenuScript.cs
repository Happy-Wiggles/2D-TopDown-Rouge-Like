using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DeathMenuScript : MonoBehaviour
{
    public TextMeshProUGUI pointsText;
    void Start()
    {
        pointsText.text = "Points earned: " +(GameController.PointsThisRound-1);
    }


    void Update()
    {
        
    }
}
