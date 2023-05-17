using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class SaveSystem
{
    /*
    Jamalian, N. (2022). Nima-Jamalian/Final-Project-In-Games-Development. [online] GitHub.Available at: https://github.com/Nima-Jamalian/Final-Project-In-Games-Development 
    */

    public static void SavePlayerData(Player player) //need to create player data thing
    {
        BinaryFormatter binaryFormatter = new BinaryFormatter();
        string path = Path.Combine(Application.persistentDataPath, "PlayerLocalData");
        FileStream stream = new FileStream(path, FileMode.Create);

        PlayerData data = new PlayerData(player);
        binaryFormatter.Serialize(stream, data);
        stream.Close();
    }

    public static PlayerData LoadPlayerData()
    {
        string path = Path.Combine(Application.persistentDataPath, "PlayerLocalData");
        if(File.Exists(path))
        {
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            PlayerData data = binaryFormatter.Deserialize(stream) as PlayerData;
            stream.Close();

            return data;
        }
        else
        {
            Debug.Log("No Saved File Found");
            return null;
        }
    }
}
