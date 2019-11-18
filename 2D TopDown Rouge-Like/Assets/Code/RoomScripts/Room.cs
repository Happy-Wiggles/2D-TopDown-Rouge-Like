using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    public string name;
    public int X;
    public int Y;
    
    public int Width = 20;
    public int Height = 20;

    // Start is called before the first frame update
    void Start()
    {
        if(RoomController.instance == null)
        {
            return;
        }

        RoomController.instance.RegisterRoom(this);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, new Vector3(Width,Height,0));
    }
}
