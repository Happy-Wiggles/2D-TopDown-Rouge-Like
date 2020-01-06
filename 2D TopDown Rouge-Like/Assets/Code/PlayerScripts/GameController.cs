using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    static GameController instance;

    #region Points
    private static int unspentPoints = 50;
    private static int pointsThisRound = 0;
    private static int pointsInMaxHealth = 0;
    private static int pointsInDamage = 0;
    #endregion

    #region PlayerStats
    private static float health;
    private static float maxHealth;
    private static float playerDamage;
    private static float moveSpeed;
    #endregion
    
    #region CurrentInfos
    private static string currentLevel;
    private static int currentX_PlayerPosition;
    private static int currentY_PlayerPosition;
    private static int currentRoomEnemies;
    private static Room currentRoom;
    #endregion

    private static PlayerController player;
    private static UICanvasController canvas;
    private static WeaponController weapon;
    
    #region PublicPoints
    public static int UnspentPoints { get => unspentPoints; set => unspentPoints = value; }
    public static int PointsThisRound { get => pointsThisRound; set => pointsThisRound = value; }
    public static int PointsInMaxHealth { get => pointsInMaxHealth; set => pointsInMaxHealth = value; }
    public static int PointsInDamage { get => pointsInDamage; set => pointsInDamage = value; }
    #endregion

    #region PublicPlayerStats
    public static float Health { get => health; set => health = value; }
    public static float MaxHealth { get => maxHealth; set => maxHealth = value; }
    public static float MoveSpeed { get => moveSpeed; set => moveSpeed = value; }
    public static float PlayerDamage { get => playerDamage; set => playerDamage = value; }
    #endregion

    #region PublicCurrentInfos
    public static string CurrentLevel { get => currentLevel; set => currentLevel = value; }
    public static int CurrentX_PlayerPosition { get => currentX_PlayerPosition; set => currentX_PlayerPosition = value; }
    public static int CurrentY_PlayerPosition { get => currentY_PlayerPosition; set => currentY_PlayerPosition = value; }
    public static int CurrentRoomEnemies { get => currentRoomEnemies; set => currentRoomEnemies = value; }
    public static Room CurrentRoom { get => currentRoom; set => currentRoom = value; }
    #endregion

    public static PlayerController Player { get => player; set => player = value; }
    public static UICanvasController Canvas { get => canvas; set => canvas = value; }
    public static WeaponController Weapon { get => weapon; set => weapon = value; }

    //Other player variables
    private float vulnerabilityTimer;
    private float timeUntilVulnerable = 1f;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Destroy(this);
        }
    }
    private void Start()
    {
        DontDestroyOnLoad(instance);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Player != null)
        {
            Canvas.healthText.text = "" + health + "/" + maxHealth;
            Canvas.levelText.text = $"Level: {currentLevel}";

            Vector3 HealthbarScale = canvas.gameObject.transform.Find("HealthBar/Background/Padding/green").GetComponent<RectTransform>().localScale;
            HealthbarScale.x = health / maxHealth;
            canvas.gameObject.transform.Find("HealthBar/Background/Padding/green").GetComponent<RectTransform>().localScale = HealthbarScale;

            if (health <= 0)
                KillPlayer();
            
            instance.vulnerabilityTimer -= Time.fixedDeltaTime;

            maxHealth = 100 * (1 + PointsInMaxHealth * 0.005f);
            playerDamage = weapon.weaponBaseDamage * (1 + PointsInDamage * 0.1f);
        }
    }

    public static void NewGame()
    {
        pointsThisRound = 0;
        maxHealth = 100 * (1 + PointsInMaxHealth * 0.005f);
        health = maxHealth;
        moveSpeed = 8;
        CurrentLevel = "Hub";
        CurrentX_PlayerPosition = 0;
        CurrentY_PlayerPosition = 0;
        CurrentRoomEnemies = 0;
    }

    public static void DamagePlayer(float damage)
    {
        if (instance.PlayerIsVulnerable())
        {
            Health -= damage;
        }
    }

    private bool PlayerIsVulnerable()
    {
        var timer = instance.vulnerabilityTimer;
        if (timer < 0)
        {
            instance.vulnerabilityTimer = instance.timeUntilVulnerable;
            return true;
        }
        return false;
    }

    public static void HealPlayer(int healthToAdd)
    {
        Health = Mathf.Min(maxHealth, health + healthToAdd);
    }

    public static void KillPlayer()
    {
        unspentPoints = unspentPoints + pointsThisRound - 1;
        SceneManager.LoadScene("Death");
        Destroy(GameObject.Find("UICanvas"));
        Destroy(GameObject.Find("Main Camera"));
        Destroy(GameObject.Find("Player"));
        Destroy(GameObject.Find("EscCanvas"));
    }
}
