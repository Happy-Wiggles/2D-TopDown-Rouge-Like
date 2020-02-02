using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public static CameraController instance;
    public Room currRoom;
    public float moveSpeedOnRoomChange;

    private void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(instance);
    }

    // Update is called once per frame
    void Update()
    {
        UpdatePosition();
    }

    void UpdatePosition()
    {

        if (currRoom == null)
        {
            return;
        }

        Vector3 targetPos = GetCameraTargetPosition();
        float diffX = 0;
        float diffY = 0;
        float platzA;
        float platzB;

        platzA = targetPos.x;
        platzB = transform.position.x;
        if (platzA < 0)
            platzA *= -1;
        if (platzB < 0)
            platzB *= -1;
        if (platzB > platzA)
            diffX = platzB - platzA;
        if (platzA >= platzB)
            diffX = platzA - platzB;

        platzA = targetPos.y;
        platzB = transform.position.y;
        if (platzA < 0)
            platzA *= -1;
        if (platzB < 0)
            platzB *= -1;
        if (platzB > platzA)
            diffY = platzB - platzA;
        if (platzA >= platzB)
            diffY = platzA - platzB;

        if (diffX > 20 || diffY > 20) //Ebenenwechsel
        {
            transform.position = targetPos;
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPos, Time.deltaTime * moveSpeedOnRoomChange);
        }


    }

    Vector3 GetCameraTargetPosition()
    {
        if (currRoom == null)
        {
            return Vector3.zero;
        }

        Vector3 targetPos = currRoom.GetRoomCenter();
        //Debug.Log($"Position at: X: {targetPos.x} Y: {targetPos.y}");
        targetPos.z = transform.position.z;

        return targetPos;
    }

    public bool IsSwitchingRoom()
    {
        return transform.position.Equals(GetCameraTargetPosition()) == false;
    }
}
