using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomGen : MonoBehaviour
{

    public List<RoomInfo> getLevel(int roomCount, int maxX, int maxY)
    {
        List<RoomInfo> generated = new List<RoomInfo>();
        generated.Add(new RoomInfo("Room", 0, 0, false, false, false, false));

        for (int i = roomCount - 1; i > 0; i--)
        {
            int Flag = generated.Count;
            while (Flag == generated.Count)
            {
                int currentRoomInfo = Random.Range(0, generated.Count);
                int currentDirection = Random.Range(0, 4);

                if (currentDirection == 0 && generated[currentRoomInfo].Y + 1 <= maxY && (!existsInList(generated, generated[currentRoomInfo].X, generated[currentRoomInfo].Y + 1)))
                {
                    generated.Add(new RoomInfo("Room", generated[currentRoomInfo].X, generated[currentRoomInfo].Y + 1, false, false, false, false));
                }

                if (currentDirection == 1 && generated[currentRoomInfo].X + 1 <= maxX && (!existsInList(generated, generated[currentRoomInfo].X + 1, generated[currentRoomInfo].Y)))
                {
                    generated.Add(new RoomInfo("Room", generated[currentRoomInfo].X + 1, generated[currentRoomInfo].Y, false, false, false, false));
                }

                if (currentDirection == 2 && generated[currentRoomInfo].Y - 1 >= (-1) * maxY && (!existsInList(generated, generated[currentRoomInfo].X, generated[currentRoomInfo].Y - 1)))
                {
                    generated.Add(new RoomInfo("Room", generated[currentRoomInfo].X, generated[currentRoomInfo].Y - 1, false, false, false, false));
                }

                if (currentDirection == 3 && generated[currentRoomInfo].X - 1 >= (-1) * maxX && (!existsInList(generated, generated[currentRoomInfo].X - 1, generated[currentRoomInfo].Y)))
                {
                    generated.Add(new RoomInfo("Room", generated[currentRoomInfo].X - 1, generated[currentRoomInfo].Y, false, false, false, false));
                }
            }
        }
        foreach (RoomInfo roomInfo in generated)
        {
            if (existsInList(generated, roomInfo.X, roomInfo.Y + 1))
                roomInfo.DoorN = true;
            if (existsInList(generated, roomInfo.X + 1, roomInfo.Y))
                roomInfo.DoorE = true;
            if (existsInList(generated, roomInfo.X, roomInfo.Y - 1))
                roomInfo.DoorS = true;
            if (existsInList(generated, roomInfo.X - 1, roomInfo.Y))
                roomInfo.DoorW = true;
        }

        Debug.Log(generated.Count);
        return generated;
    }

    public List<RoomInfo> getLevelExtreme(int roomCount, int density)
    {
        List<RoomInfo> generated = new List<RoomInfo>();
        generated.Add(new RoomInfo("Room", 0, 0, false, false, false, false));

        for (int i = roomCount - 1; i > 0; i--)
        {
            int Flag = generated.Count;
            while (Flag == generated.Count)
            {
                int currentDirection = Random.Range(0, 4);
                int currentRoomInfo = 0;
                if (i % density == 0)
                {
                    List<int> extremes = findExtremes(generated);
                    currentRoomInfo = extremes[currentDirection];
                }
                else
                {
                    currentRoomInfo = Random.Range(0, generated.Count);
                }

                if (currentDirection == 0 && (!existsInList(generated, generated[currentRoomInfo].X, generated[currentRoomInfo].Y + 1)))
                {
                    generated.Add(new RoomInfo("Room", generated[currentRoomInfo].X, generated[currentRoomInfo].Y + 1, false, false, false, false));
                }

                if (currentDirection == 1 && (!existsInList(generated, generated[currentRoomInfo].X + 1, generated[currentRoomInfo].Y)))
                {
                    generated.Add(new RoomInfo("Room", generated[currentRoomInfo].X + 1, generated[currentRoomInfo].Y, false, false, false, false));
                }

                if (currentDirection == 2 && (!existsInList(generated, generated[currentRoomInfo].X, generated[currentRoomInfo].Y - 1)))
                {
                    generated.Add(new RoomInfo("Room", generated[currentRoomInfo].X, generated[currentRoomInfo].Y - 1, false, false, false, false));
                }

                if (currentDirection == 3 && (!existsInList(generated, generated[currentRoomInfo].X - 1, generated[currentRoomInfo].Y)))
                {
                    generated.Add(new RoomInfo("Room", generated[currentRoomInfo].X - 1, generated[currentRoomInfo].Y, false, false, false, false));
                }

            }
        }
        foreach (RoomInfo roomInfo in generated)
        {
            if (existsInList(generated, roomInfo.X, roomInfo.Y + 1))
                roomInfo.DoorN = true;
            if (existsInList(generated, roomInfo.X + 1, roomInfo.Y))
                roomInfo.DoorE = true;
            if (existsInList(generated, roomInfo.X, roomInfo.Y - 1))
                roomInfo.DoorS = true;
            if (existsInList(generated, roomInfo.X - 1, roomInfo.Y))
                roomInfo.DoorW = true;
        }

        Debug.Log(generated.Count);
        return generated;
    }

    bool existsInList(List<RoomInfo> list, int x, int y)
    {
        foreach (RoomInfo room in list)
        {
            if (x == room.X && y == room.Y)
            {
                return true;
            }
        }
        return false;
    }

    List<int> findExtremes(List<RoomInfo> inList)
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
