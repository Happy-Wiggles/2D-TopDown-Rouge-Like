using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public Animator animator;
    public float Speed = 5;
    Rigidbody2D PlayerRigidBody;
    public Text CollectedText;
    public static int CollectedAmount = 0;
    private bool isMoving = false;

    // Start is called before the first frame update
    void Start()
    {
        PlayerRigidBody = GetComponent<Rigidbody2D>();
        PlayerRigidBody.freezeRotation = true;
        CollectedText = gameObject.AddComponent<Text>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        
        PlayerRigidBody.velocity = new Vector3(horizontal * Speed, vertical * Speed, 0);
        CollectedText.text = "Number of collected items: " + CollectedAmount;

        if (!(PlayerRigidBody.velocity.x == 0 && PlayerRigidBody.velocity.y == 0))
        {
            isMoving = true;
        }
        else
        {
            isMoving = false;
        }
        
        animator.SetBool("IsMoving", isMoving);
    }
}
