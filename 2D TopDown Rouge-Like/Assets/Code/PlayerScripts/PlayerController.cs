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
    public GameObject weaponStorage;

    bool portalE = false;
    bool portalEgrey = false;
    bool SkillOMatE = false;
    bool gun = false;


    int Level = 0;

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
        PlayerRigidBody = GetComponent<Rigidbody2D>();
        PlayerRigidBody.freezeRotation = true;
        GameController.Player = this;
        GameController.NewGame();
        GameController.Weapon = this.weaponStorage.GetComponent<WeaponController>();

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        var horizontal = Input.GetAxis("Horizontal");
        var vertical = Input.GetAxis("Vertical");
        var shootHorizontal = Input.GetAxis("ShootHorizontal");
        var shootVertical = Input.GetAxis("ShootVertical");

        //Pfeiltasten
        if ((shootHorizontal != 0 || shootVertical != 0) && Time.time > (GameController.Weapon.lastFire + GameController.Weapon.fireRate))
        {
            GameController.Weapon.Shoot(shootHorizontal, shootVertical);
            GameController.Weapon.lastFire = Time.time;
        }

        //Maus
        if (Input.GetButton("Fire1") && Time.time > (GameController.Weapon.lastFire + GameController.Weapon.fireRate))
        {
            var mouseConverted = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3 shootingDirection = mouseConverted - PlayerRigidBody.transform.position;
            shootingDirection.z = 0;
            GameController.Weapon.Shoot(shootingDirection);
            GameController.Weapon.lastFire = Time.time;
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
                portalE = false;
                Level ++;
                GameController.PointsThisRound++;
                GameController.CurrentLevel = "" + Level;
                SceneManager.LoadScene("NewLevel");
                PlayerRigidBody.position = new Vector3(0, 0);
            }
            if (SkillOMatE)
            {
                GameObject.Find("Room").transform.Find("SkillCanvas").gameObject.SetActive(true);
            }
            if (gun)
            {
                GameController.Weapon = this.weaponStorage.GetComponent<WeaponController>();
                Destroy(this.weaponStorage);
                gun = false;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("DoorN") && GameController.CurrentRoomEnemies == 0)
        {
            PlayerRigidBody.position = new Vector3(PlayerRigidBody.position.x, PlayerRigidBody.position.y + 7);
        }
        if (collision.CompareTag("DoorE") && GameController.CurrentRoomEnemies == 0)
        {
            PlayerRigidBody.position = new Vector3(PlayerRigidBody.position.x + 7, PlayerRigidBody.position.y, 0);
        }
        if (collision.CompareTag("DoorS") && GameController.CurrentRoomEnemies == 0)
        {
            PlayerRigidBody.position = new Vector3(PlayerRigidBody.position.x, PlayerRigidBody.position.y - 7);
        }
        if (collision.CompareTag("DoorW") && GameController.CurrentRoomEnemies == 0)
        {
            PlayerRigidBody.position = new Vector3(PlayerRigidBody.position.x - 7, PlayerRigidBody.position.y, 0);
        }


        if (collision.CompareTag("Portal"))
        {

            if (GameController.CurrentRoomEnemies == 0)
            {
                PlayerRigidBody.transform.Find("PopUpE").gameObject.SetActive(true);
                portalE = true;
            }
            else
            {
                PlayerRigidBody.transform.Find("PopUpEgrey").gameObject.SetActive(true);
                portalEgrey = true;
            }

        }
        if (collision.CompareTag("SkillOMat"))
        {
            PlayerRigidBody.transform.Find("PopUpE").gameObject.SetActive(true);
            SkillOMatE = true;
        }


        if (collision.CompareTag("Gun"))
        {
            PlayerRigidBody.transform.Find("PopUpE").gameObject.SetActive(true);
            gun = true;
            weaponStorage = collision.gameObject;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Portal"))
        {
            if (portalE)
            {
                PlayerRigidBody.transform.Find("PopUpE").gameObject.SetActive(false);
                portalE = false;
            }
            if (portalEgrey)
            {
                PlayerRigidBody.transform.Find("PopUpEgrey").gameObject.SetActive(false);
                portalEgrey = false;
            }

        }
        if (collision.CompareTag("SkillOMat"))
        {
            PlayerRigidBody.transform.Find("PopUpE").gameObject.SetActive(false);
            SkillOMatE = false;
        }
        if (collision.CompareTag("Gun"))
        {
            PlayerRigidBody.transform.Find("PopUpE").gameObject.SetActive(false);
            gun = false;
        }
    }
    
}
