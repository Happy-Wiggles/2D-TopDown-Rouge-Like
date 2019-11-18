using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyState
{
    Wander,
    Chase,
    Die
};

public class EnemyController : MonoBehaviour
{

    GameObject player;
    public EnemyState currState = EnemyState.Wander;
    public float seeingRange = 8f;
    public float speed = 3f;
    public float hp = 100f;

    private bool chooseDirection = false;
    private Vector3 randomDirection;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if (isPlayerInRange(seeingRange) && currState != EnemyState.Die)
        {
            currState = EnemyState.Chase;
        }
        else if (!isPlayerInRange(seeingRange) && currState != EnemyState.Die)
        {
            currState = EnemyState.Wander;
        }
        else if (hp <= 0)
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

    public void Death()
    {
        Destroy(gameObject);
    }

    private IEnumerator ChooseDirection()
    {
        chooseDirection = true;
        
        yield return new WaitForSeconds(Random.Range(2f, 4f));
        
        randomDirection = new Vector3(0, 0, Random.Range(0, 360)); //Random Rotation (Direction)
        var nextRotation = Quaternion.Euler(randomDirection);
        transform.rotation = Quaternion.Lerp(transform.rotation, nextRotation, Random.Range(0.5f, 2.5f));
        chooseDirection = false;
    }

}
