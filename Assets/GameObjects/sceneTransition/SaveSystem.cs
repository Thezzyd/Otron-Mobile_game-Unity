using Mirror;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem 
{
    public static void SaveLevel(LevelManager level)
    {
        BinaryFormatter formatter = new BinaryFormatter();

        string path = Path.Combine(Application.persistentDataPath, "playerData.fun");
        FileStream stream = new FileStream(path, FileMode.Create);
        LevelData data = new LevelData(level);

        formatter.Serialize(stream, data);
        stream.Close();

    }

    public static LevelData LoadLevel()
    {
        string path = Path.Combine(Application.persistentDataPath, "playerData.fun");
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            LevelData data = formatter.Deserialize(stream) as LevelData;
            stream.Close();

            return data;

        }
        else
        {
            Debug.Log("Save file not found in " + path);
            return null;
        }
    }
    
}
