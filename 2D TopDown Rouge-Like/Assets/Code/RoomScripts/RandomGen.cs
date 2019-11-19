using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomGen : MonoBehaviour
{

    public List<RoomInfo> getLevel(int roomCount, int maxX, int maxY)
    {
        List<RoomInfo> generated = new List<RoomInfo>();
        generated.Add(new RoomInfo("Room",0, 0,false,false,false,false));

        for( int i = roomCount; i > 0; i--)
        {
            int Flag = generated.Count;
            while (Flag == generated.Count)
            {
                int currentRoomInfo = Random.Range(0, generated.Count );
                int currentDirection = Random.Range(1, 5);
                if (currentDirection == 1 && generated[currentRoomInfo].DoorN == false && generated[currentRoomInfo].Y + 1 <= maxY)
                {
                    generated[currentRoomInfo].DoorN = true;
                    if (!existsInList(generated, generated[currentRoomInfo].X, generated[currentRoomInfo].Y + 1))
                    {
                        generated.Add(new RoomInfo("Room", generated[currentRoomInfo].X, generated[currentRoomInfo].Y + 1, false, false, true, false));
                    }
                }

                if (currentDirection == 2 && generated[currentRoomInfo].DoorE == false && generated[currentRoomInfo].X + 1 <= maxX)
                {
                    generated[currentRoomInfo].DoorE = true;
                    if (!existsInList(generated, generated[currentRoomInfo].X + 1, generated[currentRoomInfo].Y))
                    {
                        generated.Add(new RoomInfo("Room", generated[currentRoomInfo].X + 1, generated[currentRoomInfo].Y, false, false, false, true));
                    }
                }

                if (currentDirection == 3 && generated[currentRoomInfo].DoorS == false && generated[currentRoomInfo].Y - 1 >= (-1) * maxY)
                {
                    generated[currentRoomInfo].DoorS = true;
                    if (!existsInList(generated, generated[currentRoomInfo].X, generated[currentRoomInfo].Y - 1))
                    {
                        generated.Add(new RoomInfo("Room", generated[currentRoomInfo].X, generated[currentRoomInfo].Y - 1, true, false, false, false));
                    }
                }

                if (currentDirection == 4 && generated[currentRoomInfo].DoorW == false && generated[currentRoomInfo].X - 1 >= (-1) * maxX)
                {
                    generated[currentRoomInfo].DoorW = true;
                    if (!existsInList(generated, generated[currentRoomInfo].X - 1, generated[currentRoomInfo].Y))
                    {
                        generated.Add(new RoomInfo("Room", generated[currentRoomInfo].X - 1, generated[currentRoomInfo].Y, false, true, false, false));
                    }
                }
            }
        }
        Debug.Log(generated.Count);
        return generated;
    }


    bool existsInList(List<RoomInfo>list,int x,int y)
    {
        foreach(RoomInfo room in list)
        {
            if (x == room.X && y == room.Y) 
            { 
                return true; 
            }
        }
        return false;
    }

}
