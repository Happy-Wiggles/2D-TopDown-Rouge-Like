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
    public float attackRange = 1f;
    public bool playerInRoom = true;

    private float cooldown = 2f;
    private bool coolingDown = false;
    private float lastTimeDamaged;

    private bool chooseDirection = false;
    private Vector3 randomDirection;

    private Vector3 currentDirection;
    public GameObject FloatingTextPrefab;


    void Start()
    {
        player = GameController.Player.gameObject;
        seeingRange = 3;
        this.gameObject.GetComponent<Rigidbody2D>().freezeRotation = true;
        lastTimeDamaged = 0.0f;

        if (playerInRoom)
        {
            Wander();
            StartCoroutine(ChooseDirection());
        }
        StartCoroutine(ChooseDirection());
    }

    private void FixedUpdate()
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

            if (playerInRoom) //Player is in room
            {
                if (IsPlayerInSeeingRange(seeingRange) && currState != EnemyState.Die)
                {
                    currState = EnemyState.Chase;
                }
                else if (!IsPlayerInSeeingRange(seeingRange) && currState != EnemyState.Die)
                {
                    currState = EnemyState.Wander;
                }

                if (IsPlayerInAttackRange(attackRange) && currState != EnemyState.Die)
                {
                    currState = EnemyState.Attack;
                }
                else if (!IsPlayerInAttackRange(attackRange) && currState != EnemyState.Die)
                {
                    currState = EnemyState.Wander;
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

            //Handle change of healthbar
            Vector3 HealthbarScale = this.gameObject.transform.Find("HealthBar/Background/Padding/green").GetComponent<RectTransform>().localScale;
            HealthbarScale.x = health / 100;
            this.gameObject.transform.Find("HealthBar/Background/Padding/green").GetComponent<RectTransform>().localScale = HealthbarScale;
        }

    }

    private bool IsPlayerInSeeingRange(float range)
    {
        return Vector3.Distance(transform.position, player.transform.position) <= range;
    }
    private bool IsPlayerInAttackRange(float range)
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

        transform.position += currentDirection * speed * Time.deltaTime;

        if (IsPlayerInAttackRange(seeingRange))
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
        if (FloatingTextPrefab)
        {
            HitText(incomingDamage);
        }

        if (this.health >= incomingDamage)
        {
            this.health -= incomingDamage;
        }
        else
        {
            this.health = 0;
        }
    }

    public void HitText(float damage)
    {
        var text = Instantiate(FloatingTextPrefab, transform.position, Quaternion.identity, transform);
        text.GetComponent<TextMesh>().text = "" + damage;
    }

    private void AttackPlayer()
    {
        if (Time.time > lastTimeDamaged)
        {
            GameController.DamagePlayer(this.baseDamage);
            lastTimeDamaged = Time.time + cooldown;
        }
    }

    private void Death()
    {
        GameController.CurrentRoom.amountOfEnemies--;
        GameController.CurrentRoomEnemies--;
        Destroy(gameObject);
    }

    private IEnumerator ChooseDirection()
    {
        chooseDirection = true;

        yield return new WaitForSeconds(Random.Range(1f, 5f));
        int x = Random.Range(1, 100);
        int y = Random.Range(1, 100);
        float xDir, yDir;
        if (x >= y)
        {
            xDir = (float)x / (float)x;
            yDir = (float)y / (float)x;
        }
        else
        {
            xDir = (float)x / (float)y;
            yDir = (float)y / (float)y;
        }

        if (Random.Range(0, 2) == 0)
        {
            xDir = -xDir;
        }
        if (Random.Range(0, 2) == 0)
        {
            yDir = -yDir;
        }
        currentDirection = new Vector3(xDir, yDir, 0);

        chooseDirection = false;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        currentDirection = -currentDirection;

    }
}
