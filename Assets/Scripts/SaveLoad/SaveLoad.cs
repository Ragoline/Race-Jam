using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using System.IO;

public class SaveLoad : MonoBehaviour
{
    public static GameContainer SavedGame;

    public static void Save()
    {
        Debug.Log("save");
        SavedGame = GameContainer.Current;
        Debug.Log("g" + SavedGame.Gears);
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/savedGame.rj");
        bf.Serialize(file, SavedGame);
        file.Close();
    }

    public static void Load()
    {
        Debug.Log("load");
        if (File.Exists(Application.persistentDataPath + "/savedGame.rj"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/savedGame.rj", FileMode.Open);
            SavedGame = (GameContainer)bf.Deserialize(file);
            file.Close();
            GameContainer.Current.Load(SavedGame);
        }
    }

    public static void Delete()
    {
        if (File.Exists(Application.persistentDataPath + "/savedGame.rj"))
            File.Delete(Application.persistentDataPath + "/savedGame.rj");
    }
}
