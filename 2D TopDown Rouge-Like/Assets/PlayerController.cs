using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{

    public float Speed;
    Rigidbody2D RigidBody;
    public Text CollectedText;
    public static int CollectedAmount = 0;
    // Start is called before the first frame update
    void Start()
    {
        RigidBody = GetComponent<Rigidbody2D>();
        CollectedText = gameObject.AddComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        
        RigidBody.velocity = new Vector3(horizontal * Speed, vertical * Speed, 0);
        CollectedText.text = "Number of collected items: " + CollectedAmount;
    }
}
