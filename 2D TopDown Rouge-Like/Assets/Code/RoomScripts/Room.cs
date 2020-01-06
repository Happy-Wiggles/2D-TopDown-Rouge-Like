using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    public string roomName;
    public int X;
    public int Y;

    public int Width = 19;
    public int Height = 19;
    public int amountOfEnemies = 0;


    // Start is called before the first frame update
    void Start()
    {
        if (RoomController.instance == null)
        {
            return;
        }

        RoomController.instance.RegisterRoom(this);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, new Vector3(Width, Height, 0));
    }

    public Vector3 GetRoomCenter()
    {
        return new Vector3(X * Width, Y * Height);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.CompareTag("Player"))
        {
            this.transform.Find("FogOfWar").gameObject.SetActive(false);
            this.transform.Find("MinimapRoom").gameObject.SetActive(true);
            if (this.transform.Find("MinimapPortal") != null)
            {
                this.transform.Find("MinimapPortal").gameObject.SetActive(true);
            }
            GameController.CurrentRoom = this;
            GameController.CurrentRoomEnemies = this.amountOfEnemies;
            GameController.CurrentX_PlayerPosition = this.X;
            GameController.CurrentY_PlayerPosition = this.Y;
            RoomController.instance.OnPlayerEnterRoom(this);
        }
    }
}
