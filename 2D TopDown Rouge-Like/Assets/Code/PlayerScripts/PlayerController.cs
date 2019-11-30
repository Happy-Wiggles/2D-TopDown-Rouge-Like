using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public Animator animator;
    public float Speed = 5;
    Rigidbody2D PlayerRigidBody;
    public static int CollectedAmount = 0;
    private bool isMoving = false;
    private bool isMovingRight = false;
    private bool isMovingLeft = false;
    public WeaponController weapon;

    // Start is called before the first frame update
    void Start()
    {
        PlayerRigidBody = GetComponent<Rigidbody2D>();
        PlayerRigidBody.freezeRotation = true;
        
        if (weapon == null)
        {
            weapon = new WeaponController();
            weapon.fireRate = 2;
            weapon.bulletSpeed = 3;
        }

        GameController.Health = 100;
        GameController.MaxHealth = 100;
        GameController.MoveSpeed = 8;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        var horizontal = Input.GetAxis("Horizontal");
        var vertical = Input.GetAxis("Vertical");
        var shootHorizontal = Input.GetAxis("ShootHorizontal"); //TODO: Change input to mouseButtons and direction of pointer relative to the player
        var shootVertical = Input.GetAxis("ShootVertical");
        
        if ((shootHorizontal != 0 || shootVertical != 0) && Time.time > (weapon.lastFire + weapon.fireRate))
        {
            weapon.Shoot(shootHorizontal, shootVertical);
            weapon.lastFire = Time.time;
        }

        PlayerRigidBody.velocity = new Vector3(horizontal * Speed, vertical * Speed, 0);
        
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
        if (collision.CompareTag("DoorN"))
        {
            Debug.Log(collision.tag);
            PlayerRigidBody.position = new Vector3(PlayerRigidBody.position.x, PlayerRigidBody.position.y +6);
        }
        if (collision.CompareTag("DoorE"))
        {
            Debug.Log(collision.tag);
            PlayerRigidBody.position = new Vector3(PlayerRigidBody.position.x+6, PlayerRigidBody.position.y, 0);
        }
        if (collision.CompareTag("DoorS"))
        {
            Debug.Log(collision.tag);
            PlayerRigidBody.position = new Vector3(PlayerRigidBody.position.x, PlayerRigidBody.position.y -6);
        }
        if (collision.CompareTag("DoorW"))
        {
            Debug.Log(collision.tag);
            PlayerRigidBody.position = new Vector3(PlayerRigidBody.position.x - 6, PlayerRigidBody.position.y, 0);
        }

    }
}
