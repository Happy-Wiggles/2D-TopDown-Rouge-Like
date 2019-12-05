using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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
    bool portalE = false;
    bool SkillOMatE = false;
    int Level = 0;

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
        GameController.CurrentLevel = "Hub";
        GameController.CurrentX = 0;
        GameController.CurrentY = 0;

        DontDestroyOnLoad(this.gameObject);
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
        if (Input.GetButton("Fire1") && Time.time > (weapon.lastFire + weapon.fireRate))
        {
            var mouseConverted = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3 shootingDirection = mouseConverted - PlayerRigidBody.transform.position;
            shootingDirection.z = 0;
            weapon.Shoot(shootingDirection);
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

        if (Input.GetKeyDown("e"))
        {
            if (portalE)
            {
                PlayerRigidBody.transform.Find("PopUpE").gameObject.SetActive(false);
                Level = Level + 1;
                GameController.CurrentLevel = ""+Level;
                SceneManager.LoadScene("NewLevel");
                PlayerRigidBody.position = new Vector3(0, 0);
            }
            if (SkillOMatE)
            {
                GameObject.Find("Room").transform.Find("SkillCanvas").gameObject.SetActive(true);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("DoorN"))
        {
            PlayerRigidBody.position = new Vector3(PlayerRigidBody.position.x, PlayerRigidBody.position.y + 7);
        }
        if (collision.CompareTag("DoorE"))
        {
            PlayerRigidBody.position = new Vector3(PlayerRigidBody.position.x + 7, PlayerRigidBody.position.y, 0);
        }
        if (collision.CompareTag("DoorS"))
        {
            PlayerRigidBody.position = new Vector3(PlayerRigidBody.position.x, PlayerRigidBody.position.y - 7);
        }
        if (collision.CompareTag("DoorW"))
        {
            PlayerRigidBody.position = new Vector3(PlayerRigidBody.position.x - 7, PlayerRigidBody.position.y, 0);
        }

        if (collision.CompareTag("Portal"))
        {
            PlayerRigidBody.transform.Find("PopUpE").gameObject.SetActive(true);
            portalE = true;
        }
        if (collision.CompareTag("SkillOMat"))
        {
            PlayerRigidBody.transform.Find("PopUpE").gameObject.SetActive(true);
            SkillOMatE = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Portal"))
        {
            PlayerRigidBody.transform.Find("PopUpE").gameObject.SetActive(false);
            portalE = false;
        }
        if (collision.CompareTag("SkillOMat"))
        {
            PlayerRigidBody.transform.Find("PopUpE").gameObject.SetActive(false);
            SkillOMatE = false;
        }
    }
}
