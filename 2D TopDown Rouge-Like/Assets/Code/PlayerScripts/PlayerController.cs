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
    private bool isMovingRight = false;
    private bool isMovingLeft = false;

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

        if (!(PlayerRigidBody.velocity.x == 0 && PlayerRigidBody.velocity.y == 0)) //Check if player is moving
        {
            isMoving = true;
            if (PlayerRigidBody.velocity.x > 0) //Check if player moving right
            {
                isMovingRight = true;
                isMovingLeft = false;
            }
            if (PlayerRigidBody.velocity.x < 0) //Check if player moving left
            {
                isMovingRight = false;
                isMovingLeft = true;
            }
        }
        else
        {
            isMoving = false;
            isMovingRight = false;
            isMovingLeft = false;
        }
        
        animator.SetBool("IsMoving", isMoving);
        animator.SetBool("IsMovingRight", isMovingRight);
        animator.SetBool("IsMovingLeft", isMovingLeft);

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "DoorN")
        {
            Debug.Log(collision.tag);
            PlayerRigidBody.position = new Vector3(PlayerRigidBody.position.x, PlayerRigidBody.position.y, +6);
        }
        if (collision.tag == "DoorE")
        {
            Debug.Log(collision.tag);
            PlayerRigidBody.position = new Vector3(PlayerRigidBody.position.x+6, PlayerRigidBody.position.y, 0);
        }
        if (collision.tag == "DoorS")
        {
            Debug.Log(collision.tag);
            PlayerRigidBody.position = new Vector3(PlayerRigidBody.position.x, PlayerRigidBody.position.y, -6);
        }
        if (collision.tag == "DoorW")
        {
            Debug.Log(collision.tag);
            PlayerRigidBody.position = new Vector3(PlayerRigidBody.position.x - 6, PlayerRigidBody.position.y, 0);
        }
    }
}
