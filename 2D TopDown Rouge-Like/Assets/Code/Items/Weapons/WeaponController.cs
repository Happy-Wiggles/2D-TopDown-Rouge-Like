using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{

    public string WeaponKind { get; set; }
    
    public float Damage { get; set; }

    public GameObject BulletPrefab;

    public float bulletSpeed = 1;
    public float lastFire;
    public float fireRate;

    // Start is called before the first frame update
    void Start()
    {
        Damage = 50f;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Shoot(Vector3 shootingDir)
    {
        GameObject bullet = Instantiate(BulletPrefab, transform.position, transform.rotation) as GameObject;
        bullet.AddComponent<Rigidbody2D>().gravityScale = 0;

        var betragX = shootingDir.x;
        var betragY = shootingDir.y;
        if (betragX < 0)
        {
            betragX *= -1;
        }
        if (betragY < 0)
        {
            betragY *= -1; 
        }
        var bigger = betragX;

        if (betragY > bigger)
        {
            bigger = betragY;
        }

        try
        {
            shootingDir.x = shootingDir.x / bigger * bulletSpeed;
            shootingDir.y = shootingDir.y / bigger * bulletSpeed;
        }
        catch (System.Exception e)
        {

        }
        bullet.GetComponent<Rigidbody2D>().velocity = shootingDir;
    }

    public void Shoot(float x, float y)
    {
        GameObject bullet = Instantiate(BulletPrefab, transform.position, transform.rotation) as GameObject;
        bullet.AddComponent<Rigidbody2D>().gravityScale = 0;

        if (x < 0)
        {
            x = Mathf.Floor(x) * bulletSpeed;
        }
        else
        {
            x = Mathf.Ceil(x) * bulletSpeed;
        }

        if (y < 0)
        {
            y = Mathf.Floor(y) * bulletSpeed;
        }
        else
        {
            y = Mathf.Ceil(y) * bulletSpeed;
        }

        bullet.GetComponent<Rigidbody2D>().velocity = new Vector3(x, y, 0);
    }
}
