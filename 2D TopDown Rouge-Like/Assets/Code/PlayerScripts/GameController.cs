using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    GameController instance;

    private static float health;
    private static float maxHealth;
    private static float moveSpeed;

    public Text healthText;
    
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
    
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }            
    }

    // Update is called once per frame
    void Update()
    {
        healthText.text = $"Health: {health}";
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
