using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RoomInfo{
    public string name;
    public int X;
    public int Y;
}

public class RoomController : MonoBehaviour
{
    RoomController instance;

    List<Room> Rooms = new List<Room>();

    Queue<RoomInfo> loadRoomQueue = new Queue<RoomInfo>();
    
    List<Room> loadedRooms = new List<Room>();

    RoomInfo currentLoadRoomInfo = new RoomInfo();

    bool isLoadingRoom;

    private void Awake()
    {
        instance = this;
    }

    void loadRoom(string name, int x, int y)
    {
        if (RoomExists(x, y))
        {
            return;
        }

        RoomInfo roomInfo = new RoomInfo();
        roomInfo.name = name;
        roomInfo.X = x;
        roomInfo.Y = y;

        loadRoomQueue.Enqueue(roomInfo);
        
    }

    IEnumerator loadRoomRoutine(RoomInfo roomInfo)
    {
        string currentRoomName = roomInfo.name;

        AsyncOperation loadRoom = SceneManager.LoadSceneAsync(currentRoomName.ToString(), LoadSceneMode.Additive);

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

        isLoadingRoom = false;

        loadedRooms.Add(room);
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

    // Start is called before the first frame update
    void Start()
    {
        loadRoom("Start", 0, 0);
        loadRoom("Empty", 1, 0);
    }

    // Update is called once per frame
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

        //StartCoroutine(loadRoomRoutine(currentLoadRoomInfo));

    }
}
