using System.Collections;
using System.Collections.Generic;
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
    public bool playerInRoom = true;
    public GameObject FloatingTextPrefab;
    private float lastTimeDamaged;

    #region Movement
    private Transform homePosition;
    private Transform currentPosition;
    private Vector3 currentDirection;
    private Vector3 randomDirection;
    private List<Vector3> movingField;

    private bool choosingDirection = false;
    private bool isDoneDecreasingSpeed = false;
    private bool isDoneIncreasingSpeed = false;
    private bool isRemainingStill = false;
    #endregion

    #region Stats
    public float seeingRange = 200f;
    public float speed = 1f;
    public float maxSpeed = 3f;
    private float minSpeed = 1f;
    public float health = 100f;

    public float baseDamage = 10f;
    public float attackRange = 1f;
    private float attackCooldown = 1f;
    #endregion

    void Start()
    {
        homePosition = transform;
        movingField = new List<Vector3>();
        movingField.Add(homePosition.position);
       
        player = GameController.Player.gameObject;
        this.gameObject.GetComponent<Rigidbody2D>().freezeRotation = true;
        lastTimeDamaged = 0.0f;

        if (playerInRoom)
        {
            Wander();
        }
    }

    private void FixedUpdate()
    {
        currentPosition = transform;

        if (player != null)
        {
            //Checks if player is in range and therefore stop being dynamic (Kills the pushing physics)
            if (Vector3.Distance(player.transform.position, transform.position) <= (attackRange + 0.5))
            {
                transform.gameObject.GetComponent<Rigidbody2D>().isKinematic = true;
            }
            else
            {
                transform.gameObject.GetComponent<Rigidbody2D>().isKinematic = false;
            }

            //Next move being executed depending on the state
            switch (currState)
            {
                case (EnemyState.Idle):
                    Idle();
                    break;
                case (EnemyState.Wander):
                    if (!isRemainingStill)
                        Wander();
                    break;
                case (EnemyState.Chase):
                    ChasePlayer();
                    break;
                case (EnemyState.Die):
                    Death();
                    break;
                case (EnemyState.Attack):
                    AttackPlayer();
                    break;
            }

            //If the Player is in room, the next state of the enemy is being decided
            if (playerInRoom) 
            {
                if (IsPlayerInAttackRange(attackRange) && currState != EnemyState.Die)
                {
                    currState = EnemyState.Attack;
                }
                else if (IsPlayerInSeeingRange(seeingRange) && currState != EnemyState.Die)
                {
                    currState = EnemyState.Chase;
                }
                else if (!IsPlayerInSeeingRange(seeingRange) && currState != EnemyState.Die)
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

    private bool IsPlayerInSeeingRange(float seeingRange)
    {
        return Vector3.Distance(transform.position, player.transform.position) <= seeingRange;
    }
    private bool IsPlayerInAttackRange(float attackRange)
    {
        return Vector3.Distance(transform.position, player.transform.position) <= attackRange;
    }

    void Idle()
    {
        //Enemy does literally nothing
        StartCoroutine(DecreaseSpeed(maxSpeed));
    }

    void Wander()
    {
        //TODO: Maybe create a tiny field in which the enemy is allowed to move in
        
        if (!choosingDirection)
        {
            StartCoroutine(ChooseDirection());
        }
        else
        {
            if (isDoneDecreasingSpeed)
            {
                StartCoroutine(DecreaseSpeed(Random.Range(maxSpeed / 3, maxSpeed / 2)));
            }
        }

        if (isDoneIncreasingSpeed)
        {
            StartCoroutine(IncreaseSpeed(Random.Range(maxSpeed / 3, maxSpeed)));
        }

        var nextPosition = currentDirection * speed * Time.deltaTime;
        transform.position += nextPosition;
    }

    private IEnumerator ChooseDirection()
    {
        choosingDirection = true;

        //Let enemy stay still for a random time 0.25s - 1.25s
        isRemainingStill = true;
        StopMovement();
        yield return new WaitForSeconds(Random.Range(0.25f, 1.25f));
        isRemainingStill = false;

        int x = Random.Range(1, 100);
        int y = Random.Range(1, 100);
        float xDir, yDir;
        if (x >= y)
        {
            xDir = x / (float)x;
            yDir = y / (float)x;
        }
        else
        {
            xDir = x / (float)y;
            yDir = y / (float)y;
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
        
        yield return new WaitForSeconds(Random.Range(1f, 2f));

        choosingDirection = false;
    }

    private IEnumerator DecreaseSpeed(float decreaseSpeedBy)
    {
        //Check if speed of 0 is requested:
        bool doFullStop = (this.maxSpeed - decreaseSpeedBy) <= 0 ? true : false;
        var decreaseStep = decreaseSpeedBy / 6;
        this.isDoneDecreasingSpeed = false;
        var randBreak = Random.Range(0.5f, 2f);
        yield return new WaitForSeconds(randBreak);
        
        //Slowly decrease speed
        while (this.speed - decreaseSpeedBy > this.minSpeed)
        {
            if (doFullStop)
                this.speed = 0;
    
            this.speed -= decreaseStep;
            yield return new WaitForSeconds(0.4f);
        }

        if (this.speed < this.minSpeed)
            this.speed = this.minSpeed;

        this.isDoneDecreasingSpeed = true;
    }

    private IEnumerator IncreaseSpeed(float increaseSpeedBy)
    {
        var increaseStep = increaseSpeedBy / 6;
        var randBreak = Random.Range(0.5f, 2);
        this.isDoneIncreasingSpeed = false;
        yield return new WaitForSeconds(randBreak); //Wait a bit
        
        //Slowly increase speed 
        while (this.speed < this.maxSpeed)
        {
            this.speed += increaseStep;
            yield return new WaitForSeconds(0.4f);
        }

        if (this.speed - this.maxSpeed < 1)
            this.speed = this.maxSpeed;

        this.isDoneIncreasingSpeed = true;
    }

    private void StopMovement()
    {    
        StartCoroutine(DecreaseSpeed(maxSpeed));
    }

    void ChasePlayer()
    {
        //Make enemy go maxSpeed before starting to chase player
        if (speed < maxSpeed)
            StartCoroutine(IncreaseSpeed(maxSpeed));
        transform.position = Vector2.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
    }

    public void ReceiveDamage(float incomingDamage)
    {
        if (FloatingTextPrefab && this.health > 0)
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
            lastTimeDamaged = Time.time + attackCooldown;
        }
    }

    private void Death()
    {
        GameController.CurrentRoom.amountOfEnemies--;
        GameController.CurrentRoomEnemies--;
        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Wander();
        }
        else
        {
            currentDirection = -currentDirection;
        }
    }
}
