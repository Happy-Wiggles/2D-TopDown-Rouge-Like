using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RoomInfo
{
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

    List<RoomInfo> Generated = new List<RoomInfo>();
    List<Room> loadedRooms = new List<Room>();
    Queue<RoomInfo> loadRoomQueue = new Queue<RoomInfo>();
    RoomInfo currentLoadRoomInfo = new RoomInfo();
    Room currRoom;
    public GameObject portalObject;
    public int amountRooms = 10;
    public int density = 2;
    bool portalBool = false;
    private bool firstTimeLoad = true;

    bool isLoadingRoom = false;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        RandomGen Roomgeneration = new RandomGen();
        Generated = Roomgeneration.getLevelExtreme(amountRooms, density);

        foreach (RoomInfo newRoom in Generated)
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
                Room portalRoom = loadedRooms[findExtremes(loadedRooms)[Random.Range(0, 4)]];
                portalRoom.transform.Find("Portal").gameObject.SetActive(true);
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
                    }
                }
            }
        }
    }
    List<int> findExtremes(List<Room> inList)
    {
        int N = 0;
        int E = 0;
        int S = 0;
        int W = 0;

        for (int i = 0; i < inList.Count; i++)
        {
            if (inList[i].Y > inList[N].Y)
                N = i;

            if (inList[i].X > inList[E].X)
                E = i;

            if (inList[i].Y < inList[S].Y)
                S = i;

            if (inList[i].X < inList[W].X)
                W = i;

        }
        List<int> outList = new List<int>();
        outList.Add(N);
        outList.Add(E);
        outList.Add(S);
        outList.Add(W);
        return outList;
    }
}
