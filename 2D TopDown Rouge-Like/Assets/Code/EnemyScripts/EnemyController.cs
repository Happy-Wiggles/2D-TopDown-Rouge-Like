using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyState
{
    Wander,
    Chase,
    Die,
    Attack
};

public class EnemyController : MonoBehaviour
{

    GameObject player;
    public EnemyState currState = EnemyState.Wander;
    public float seeingRange = 8f;
    public float speed = 3f;
    public float health = 100f;
    public float baseDamage = 10f;
    public float attackRange = 1.5f;
    private bool dead = false;

    private float cooldown = 2f;
    private bool cooldownAttack = false;

    private bool chooseDirection = false;
    private Vector3 randomDirection;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        seeingRange = 3;
    }

    // Update is called once per frame
    void Update()
    {
        try
        {
            if (isPlayerInRange(seeingRange) && currState != EnemyState.Die)
            {
                currState = EnemyState.Chase;
            }
            else if (!isPlayerInRange(seeingRange) && currState != EnemyState.Die)
            {
                currState = EnemyState.Wander;
            }

            if (Vector3.Distance(transform.position, player.transform.position) <= attackRange)
            {
                currState = EnemyState.Attack;
            }

            if (this.health <= 0)
            {
                currState = EnemyState.Die;
            }

            switch (currState)
            {
                case (EnemyState.Wander):
                    Wander();
                    break;
                case (EnemyState.Chase):
                    Chase();
                    break;
                case (EnemyState.Die):
                    Death();
                    break;
                case (EnemyState.Attack):
                    Debug.Log("Attacking player now");
                    AttackPlayer();
                    break;
            }
        }
        catch (System.Exception exc)
        {
            //TODO: Do something when player is dead and therefore null
            Debug.Log(exc.Message.ToString());
        }
    }

    private bool isPlayerInRange(float range)
    {
        return Vector3.Distance(transform.position, player.transform.position) <= range;
    }

    void Wander()
    {
        if (!chooseDirection)
        {
            StartCoroutine(ChooseDirection());
        }

        transform.position += -transform.right * speed * Time.deltaTime;
        
        if (isPlayerInRange(seeingRange))
        {
            currState = EnemyState.Chase;
        }
    }

    void Chase()
    {
        transform.position = Vector2.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
    }

    public void ReceiveDamage(float incomingDamage)
    {
        if (this.health >= incomingDamage)
        {
            this.health -= incomingDamage;
        }
        else
        {
            this.health = 0;
        }
    }

    private void AttackPlayer()
    {
        if (!cooldownAttack)
        {
            GameController.DamagePlayer(this.baseDamage);
        }

        StartCoroutine(CoolDown());
    }

    private IEnumerator CoolDown()
    {
        cooldownAttack = true;
        yield return new WaitForSeconds(cooldown);
        cooldownAttack = false;
    }

    void Death()
    {
        dead = true;
        Destroy(gameObject);
    }

    private IEnumerator ChooseDirection()
    {
        chooseDirection = true;
        
        yield return new WaitForSeconds(Random.Range(2f, 4f));
        
        randomDirection = new Vector3(0, 0, Random.Range(0, 360));
        var nextRotation = Quaternion.Euler(randomDirection);
        transform.rotation = Quaternion.Lerp(transform.rotation, nextRotation, Random.Range(0.5f, 2.5f));
        chooseDirection = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Wall") || collision.CompareTag("DoorE") || 
            collision.CompareTag("DoorS") || collision.CompareTag("DoorN") || 
            collision.CompareTag("DoorW"))
        {
            transform.rotation = Quaternion.AngleAxis(180, transform.forward) * transform.rotation;
        }
    }

}
