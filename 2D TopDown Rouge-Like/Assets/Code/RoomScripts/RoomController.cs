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

    public RoomInfo() { }
    public RoomInfo(string name, int X, int Y, bool DoorN, bool DoorE, bool DoorS, bool DoorW)
    {
        this.name = name;
        this.X = X;
        this.Y = Y;
        this.DoorN = DoorN;
        this.DoorE = DoorE;
        this.DoorS = DoorS;
        this.DoorW = DoorW;
    }
}

public class RoomController : MonoBehaviour
{
    List<RoomInfo> Generated=new List<RoomInfo>();
    public static RoomController instance;
    Queue<RoomInfo> loadRoomQueue = new Queue<RoomInfo>();
    RoomInfo currentLoadRoomInfo = new RoomInfo();
    List<Room> loadedRooms = new List<Room>();

    

    bool isLoadingRoom=false;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        RandomGen Roomgeneration = new RandomGen();
        Generated = Roomgeneration.getLevel(20,3,3);

        foreach(RoomInfo a in Generated)
        {
            LoadRoom(a);
        }

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

    public void LoadRoom(RoomInfo roomInfo)
    {
        if (RoomExists(roomInfo.X, roomInfo.Y))
        {
            return;
        }
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
