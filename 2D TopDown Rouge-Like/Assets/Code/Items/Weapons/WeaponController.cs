using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{

    public string WeaponKind { get; set; }
    
    public float Damage { get; set; }

    public GameObject BulletPrefab;

    public float bulletSpeed;
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
