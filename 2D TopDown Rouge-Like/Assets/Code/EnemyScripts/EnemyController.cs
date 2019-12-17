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
    public bool playerNotInRoom = true;

    private float cooldown = 2f;
    private bool cooldownAttack = false;

    private bool chooseDirection = false;
    private Vector3 randomDirection;

    private Vector3 currentDirection;
    public GameObject FloatingTextPrefab;

    // Start is called before the first frame update
    void Start()
    {

        player = GameController.Player.gameObject;
        seeingRange = 3;
        this.gameObject.GetComponent<Rigidbody2D>().freezeRotation = true;

        StartCoroutine(ChooseDirection());
    }

    // Update is called once per frame
    void FixedUpdate()
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

            Vector3 HealthbarScale=this.gameObject.transform.Find("HealthBar/Background/Padding/green").GetComponent<RectTransform>().localScale;
            HealthbarScale.x =health/100;
            this.gameObject.transform.Find("HealthBar/Background/Padding/green").GetComponent<RectTransform>().localScale = HealthbarScale;
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

        transform.position += currentDirection * speed * Time.deltaTime;

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
        if (FloatingTextPrefab)
        {
            hitText(incomingDamage);
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

    public void hitText(float damage)
    {
        var text = Instantiate(FloatingTextPrefab, transform.position, Quaternion.identity, transform);
        text.GetComponent<TextMesh>().text = ""+damage;
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
        GameController.CurrentRoom.amountOfEnemies--;
        GameController.CurrentRoomEnemies--;
        Destroy(gameObject);
    }

    private IEnumerator ChooseDirection()
    {
        chooseDirection = true;

        yield return new WaitForSeconds(Random.Range(2f, 4f));
        int x = Random.Range(1, 100);
        int y = Random.Range(1, 100);
        float xDir,yDir;
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
