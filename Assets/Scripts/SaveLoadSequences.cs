using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public static class SaveLoadSequences  {

    private static GameObject seanceParent;

    public static GameObject SeanceParent
    {
        get { return seanceParent; }
        set { seanceParent = value; }
    }
       
    private static List<List<GameObject>> seances;

    public static void Save()
    {
        List<GameObject> seance = new List<GameObject>();
        foreach (Transform item in seanceParent.transform)
        {
            seance.Add(item.gameObject);
        }
        SaveLoadSequences.seances.Add(seance);
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/seances.save");
        bf.Serialize(file, SaveLoadSequences.seances);
        file.Close();
    }

    public static void Load()
    {
        if (File.Exists(Application.persistentDataPath + "/seances.save"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/seances.save", FileMode.Open);
            SaveLoadSequences.seances = (List<List<GameObject>>)bf.Deserialize(file);
            file.Close();
        }
    }
}
