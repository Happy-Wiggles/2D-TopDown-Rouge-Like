using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RoomInfo{
    public string name;
    public int X;
    public int Y;
    public bool DoorN;
    public bool DoorE;
    public bool DoorS;
    public bool DoorW;
}

public class RoomController : MonoBehaviour
{
    public static RoomController instance;

    List<Room> Rooms = new List<Room>();

    Queue<RoomInfo> loadRoomQueue = new Queue<RoomInfo>();
    
    List<Room> loadedRooms = new List<Room>();

    RoomInfo currentLoadRoomInfo = new RoomInfo();

    bool isLoadingRoom=false;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        LoadRoom("Hub", 0, 0, false, true, false, true);
        LoadRoom("Room", 1, 0,false,true,false,true);
        LoadRoom("Room", 0, 1, false, true, false, true);
    }

    bool RoomExists(int x, int y)
    {
        foreach (Room room in loadedRooms)
        {
            if (room.X == x && room.Y == y)
            {
                return true;
            }
        }

        return false;
    }

    public void LoadRoom(string name, int x, int y, bool N, bool E, bool S, bool W)
    {
        if (RoomExists(x, y))
        {
            return;
        }

        RoomInfo roomInfo = new RoomInfo();
        roomInfo.name = name;
        roomInfo.X = x;
        roomInfo.Y = y;
        roomInfo.DoorN = N;
        roomInfo.DoorE = E;
        roomInfo.DoorS = S;
        roomInfo.DoorW = W;
        Debug.Log(E);
        loadRoomQueue.Enqueue(roomInfo);
        
    }

    void Update()
    {
        UpdateRoomQueue();
    }

    void UpdateRoomQueue()
    {
        if (isLoadingRoom)
        {
            return;
        }

        if (loadRoomQueue.Count == 0)
        {
            return;
        }

        currentLoadRoomInfo = loadRoomQueue.Dequeue();
        isLoadingRoom = true;

        StartCoroutine(LoadRoomRoutine(currentLoadRoomInfo));

    }
    IEnumerator LoadRoomRoutine(RoomInfo roomInfo)
    {
        string currentRoomName = roomInfo.name;

        AsyncOperation loadRoom = SceneManager.LoadSceneAsync(currentRoomName, LoadSceneMode.Additive);

        while (!loadRoom.isDone)
        {
            yield return null;
        }
    }

    public void RegisterRoom(Room room)
    {
        room.transform.position = new Vector3(
            currentLoadRoomInfo.X * room.Width, 
            currentLoadRoomInfo.Y * room.Height,
            0);

        room.X = currentLoadRoomInfo.X;
        room.Y = currentLoadRoomInfo.Y;
        room.name = currentLoadRoomInfo.name;

        room.transform.parent = transform;

        if (currentLoadRoomInfo.DoorN == false)
        {
            Destroy(room.transform.Find("DoorN").gameObject);
        }
        if (currentLoadRoomInfo.DoorE == false)
        {
            Destroy(room.transform.Find("DoorE").gameObject);
        }
        if (currentLoadRoomInfo.DoorS == false)
        {
            Destroy(room.transform.Find("DoorS").gameObject);
        }
        if (currentLoadRoomInfo.DoorW == false)
        {
            Destroy(room.transform.Find("DoorW").gameObject);
        }

        isLoadingRoom = false;

        loadedRooms.Add(room);
    }





}
