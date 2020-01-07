using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
public static class SaveSystem
{
    public static void SaveGame()
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/player.sav";
        FileStream stream = new FileStream(path, FileMode.Create);
        SaveData data = new SaveData();
        formatter.Serialize(stream, data);
        stream.Close();
    }


    public static void LoadGame()
    {
        string path = Application.persistentDataPath + "/player.sav";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);
            SaveData data = formatter.Deserialize(stream) as SaveData;
            stream.Close();

            GameController.UnspentPoints = data.unspentPoints;
            GameController.PointsInDamage = data.pointsInDamage;
            GameController.PointsInMaxHealth = data.pointsInMaxHealth;
        }
        else
        {
            Debug.LogError("FileNotFound");
        }
    }


}
