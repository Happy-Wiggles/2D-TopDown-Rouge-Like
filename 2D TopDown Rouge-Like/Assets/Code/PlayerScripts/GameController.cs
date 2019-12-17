﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    static GameController instance;

    private static int unspentPoints = 50;
    private static int pointsThisRound = 0;
    private static int pointsInMaxHealth = 0;



    private static float health;
    private static float maxHealth;
    private static float moveSpeed;
    private static string currentLevel;
    private static int currentX;
    private static int currentY;
    private static int currentRoomEnemies;
    private static Room currentRoom;
    private static PlayerController player;
    private static UICanvasController canvas;

    public static float Health { get => health; set => health = value; }
    public static float MaxHealth { get => maxHealth; set => maxHealth = value; }
    public static float MoveSpeed { get => moveSpeed; set => moveSpeed = value; }
    public static string CurrentLevel { get => currentLevel; set => currentLevel = value; }
    public static int UnspentPoints { get => unspentPoints; set => unspentPoints = value; }
    public static int PointsThisRound { get => pointsThisRound; set => pointsThisRound = value; }
    public static int CurrentX { get => currentX; set => currentX = value; }
    public static int CurrentY { get => currentY; set => currentY = value; }
    public static int CurrentRoomEnemies { get => currentRoomEnemies; set => currentRoomEnemies = value; }
    public static Room CurrentRoom { get => currentRoom; set => currentRoom = value; }
    public static PlayerController Player { get => player; set => player = value; }
    public static UICanvasController Canvas { get => canvas; set => canvas = value; }
    public static int PointsInMaxHealth { get => pointsInMaxHealth; set => pointsInMaxHealth = value; }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
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
        if(Player != null) { 
            Canvas.healthText.text = $"Health: {health}";
            Canvas.levelText.text = $"Level: {currentLevel}   Room {CurrentX},{CurrentY}";

            if (health <= 0)
                KillPlayer();

            maxHealth = 100 * (1 + PointsInMaxHealth * 0.005f);
        }
    }

    public static void newGame()
    {
        pointsThisRound = 0;
        maxHealth = 100 * (1 + PointsInMaxHealth * 0.005f);
        health = maxHealth;
        moveSpeed = 8;
        CurrentLevel = "Hub";
        CurrentX = 0;
        CurrentY = 0;
        CurrentRoomEnemies = 0;
    }

    public static void DamagePlayer(float damage)
    {
        health -= damage;
    }

    public static void healPlayer(int healthToAdd)
    {
        health = Mathf.Min(maxHealth, health + healthToAdd);
    }

    public static void KillPlayer()
    {
        unspentPoints = unspentPoints + pointsThisRound - 1;
        SceneManager.LoadScene("Death");
        Destroy(GameObject.Find("UICanvas"));
        Destroy(GameObject.Find("Main Camera"));
        Destroy(GameObject.Find("Player"));
        
    }
}
