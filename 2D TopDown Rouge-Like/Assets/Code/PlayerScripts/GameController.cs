using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameController : MonoBehaviour
{
    GameController instance;

    private static float health;
    private static float maxHealth;
    private static float moveSpeed;
    private static string currentLevel;
    private static int currentX;
    private static int currentY;
    private static int currentRoomEnemies;
    private static Room currentRoom;
    private static PlayerController player;
    public TextMeshProUGUI healthText;
    public TextMeshProUGUI levelText;
    public static float Health
    {
        get => Health = health;
        set => health = value;
    }
    public static float MaxHealth
    {
        get => MaxHealth = maxHealth;
        set => maxHealth = value;
    }
    public static float MoveSpeed
    {
        get => MoveSpeed = moveSpeed;
        set => moveSpeed = value;
    }
    public static string CurrentLevel
    {
        get => CurrentLevel = currentLevel;
        set => currentLevel = value;
    }

    public static int CurrentX
    {
        get => CurrentX = currentX;
        set => currentX = value;
    }

    public static int CurrentY
    {
        get => CurrentY = currentY;
        set => currentY = value;
    }

    public static int CurrentRoomEnemies
    {
        get => CurrentRoomEnemies = currentRoomEnemies;
        set => currentRoomEnemies = value;
    }

    public static Room CurrentRoom
    {
        get => CurrentRoom = currentRoom;
        set => currentRoom = value;
    }
    public static PlayerController Player
    {
        get => Player = player;
        set => player = value;
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }            
    }
    private void Start()
    {
        DontDestroyOnLoad(instance);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        healthText.text = $"Health: {health}";
        levelText.text = $"Level: {currentLevel}   Room {currentX},{currentY}";
        Debug.Log(""+player.portalE);

    }

    public static void DamagePlayer(float damage)
    {
        
        
        if (!(health <= 0))
        {
            health -= damage;
        }
        else
        {
            health = 0;
            KillPlayer();
        }
    }

    public static void healPlayer(int healthToAdd)
    {
        health = Mathf.Min(maxHealth, health + healthToAdd); 
    }

    public static void KillPlayer()
    {
        //TODO:
        //Show death screen and block all inputs except for mouse clicks on retry/back_to_menu/...
    }
}
