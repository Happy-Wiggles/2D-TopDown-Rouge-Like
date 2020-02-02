using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class floatingText : MonoBehaviour
{
    public float DestroyTime = 3f;
    public Vector3 LocalOffset = new Vector3(0, 2, 0);
    void Start()
    {
        DestroyTime = Random.Range(0.5f, 1);
        Destroy(gameObject, DestroyTime);
        transform.localPosition += LocalOffset;
    }
}
