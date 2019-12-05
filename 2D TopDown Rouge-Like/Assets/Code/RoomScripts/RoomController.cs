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
    public static RoomController instance;

    List<RoomInfo> Generated=new List<RoomInfo>();
    List<Room> loadedRooms = new List<Room>();
    Queue<RoomInfo> loadRoomQueue = new Queue<RoomInfo>();
    RoomInfo currentLoadRoomInfo = new RoomInfo();
    Room currRoom;
    public GameObject portalObject;
    bool portalBool=false;
    private bool firstTimeLoad = true;

    bool isLoadingRoom=false;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        RandomGen Roomgeneration = new RandomGen();
        Generated = Roomgeneration.getLevelExtreme(12,8);

        foreach(RoomInfo newRoom in Generated)
        {
            LoadRoom(newRoom);
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
            if (!portalBool)
            {
                Room positionRoom = loadedRooms[Random.Range(0, loadedRooms.Count)];
                Vector3 positionVector = new Vector3(positionRoom.X * positionRoom.Width, positionRoom.Y * positionRoom.Height, 0);
                Instantiate(portalObject, positionVector, Quaternion.identity);
                portalBool = true;
            }
            if (firstTimeLoad)
            {
                UpdateEnemiesInRoom();
                firstTimeLoad = !firstTimeLoad;
            }
            
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
        room.roomName = currentLoadRoomInfo.name;

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

        if (loadedRooms.Count == 0)
        {
            CameraController.instance.currRoom = room;
        }

        loadedRooms.Add(room);
    }

    public void OnPlayerEnterRoom(Room room)
    {
        CameraController.instance.currRoom = room;
        currRoom = room;
        UpdateEnemiesInRoom();
    }

    private void UpdateEnemiesInRoom()
    {
        foreach (var room in loadedRooms)
        {
            if (currRoom != room)
            {
                EnemyController[] enemies = room.GetComponentsInChildren<EnemyController>();
                if (enemies != null)
                {
                    foreach (var enemy in enemies)
                    {
                        enemy.playerNotInRoom = true;
                        Debug.Log("Enemie is Idle");
                    }
                }
            }
            else
            {
                EnemyController[] enemies = room.GetComponentsInChildren<EnemyController>();
                if (enemies != null)
                {
                    foreach (var enemy in enemies)
                    {
                        enemy.playerNotInRoom = false;
                        Debug.Log("Enemie is doing something intresting");
                    }
                }
            }
        }
    }
}
