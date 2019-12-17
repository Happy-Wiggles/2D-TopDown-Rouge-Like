using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UICanvasController : MonoBehaviour
{
    public TextMeshProUGUI healthText;
    public TextMeshProUGUI levelText;
    // Start is called before the first frame update
    void Start()
    {
        GameController.Canvas = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
