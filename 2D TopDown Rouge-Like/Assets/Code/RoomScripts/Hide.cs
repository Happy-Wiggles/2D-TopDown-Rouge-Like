using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hide : MonoBehaviour
{
    public Room ownRoom;
    private Room currRoom;

    // Start is called before the first frame update
    void Start()
    {
        currRoom = GameController.CurrentRoom;
        ownRoom = gameObject.GetComponentInParent<Room>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        currRoom = GameController.CurrentRoom;
        ownRoom = gameObject.GetComponentInParent<Room>();
        GameObject[] hideRoomGOs = GameObject.FindGameObjectsWithTag("HideRoom");

        if (currRoom != null)
        {
            if (this.currRoom.transform.position == this.ownRoom.transform.position)
            {
                foreach (var roomToHide in hideRoomGOs)
                {
                    if (roomToHide.gameObject.transform.position == currRoom.transform.position)
                    {
                        roomToHide.GetComponent<SpriteRenderer>().enabled = false;
                    }
                }
            }
            else
            {
                foreach (var roomToHide in hideRoomGOs)
                {
                    if (roomToHide.gameObject.transform.position != currRoom.transform.position)
                    {
                        roomToHide.GetComponent<SpriteRenderer>().enabled = true;
                    }
                }
            }
        }
    }
}
