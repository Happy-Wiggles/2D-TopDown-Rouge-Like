using System.Collections;
using UnityEngine;

public enum EnemyState
{
    Idle,
    Wander,
    Chase,
    Die,
    Attack
};

public class EnemyController : MonoBehaviour
{
    GameObject player;
    public EnemyState currState = EnemyState.Idle;
    public float seeingRange = 8f;
    public float speed = 3f;
    public float health = 100f;
    public float baseDamage = 10f;
    public float attackRange = 1.5f;
    private bool dead = false;
    public bool playerNotInRoom = true;

    private float cooldown = 2f;
    private bool cooldownAttack = false;

    private bool chooseDirection = false;
    private Vector3 randomDirection;

    // Start is called before the first frame update
    void Start()
    {
        player = GameController.Player.gameObject;
        seeingRange = 3;
    }

    // Update is called once per frame
    void Update()
    {
        if (player != null)
        {
            switch (currState)
            {
                case (EnemyState.Idle):
                    Idle();
                    break;
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
                    AttackPlayer();
                    break;
            }

            if (!playerNotInRoom) //Player is in room
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
            }
            else
            {
                currState = EnemyState.Idle;
            }
        }

    }

    private bool isPlayerInRange(float range)
    {
        return Vector3.Distance(transform.position, player.transform.position) <= range;
    }

    void Idle()
    {
        //Enemy does literally nothing
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
        GameController.CurrentRoom.amountOfEnemies--;
        GameController.CurrentRoomEnemies--;
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
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Wall"))
        {
            transform.rotation = Quaternion.AngleAxis(180, transform.forward) * transform.rotation;
        }
        if (collision.collider.CompareTag("DoorN"))
        {
            transform.rotation = Quaternion.AngleAxis(180, transform.forward) * transform.rotation;
        }
        if (collision.collider.CompareTag("DoorS"))
        {
            transform.rotation = Quaternion.AngleAxis(180, transform.forward) * transform.rotation;
        }
        if (collision.collider.CompareTag("DoorW"))
        {
            transform.rotation = Quaternion.AngleAxis(180, transform.forward) * transform.rotation;
        }
        if (collision.collider.CompareTag("DoorE"))
        {
            transform.rotation = Quaternion.AngleAxis(180, transform.forward) * transform.rotation;
        }

        if (collision.collider.CompareTag("Enemy"))
        {
            transform.rotation = Quaternion.AngleAxis(180, transform.forward) * transform.rotation;
        }
    }

}
