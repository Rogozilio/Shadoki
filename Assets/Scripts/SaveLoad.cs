using System;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

[System.Serializable]
public class SaveData
{
    public char[,] Grid;
    public float CircuitBar;
}
public class SaveLoad
{
    public void SaveData(float circuitBar)
    {
        if(circuitBar != 1)
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Create(Application.persistentDataPath
              + "/MySaveData.dat");
            SaveData data = new SaveData();
            data.Grid = Grid.Value;
            data.CircuitBar = circuitBar;
            bf.Serialize(file, data);
            file.Close();
            Debug.Log("Game data saved!");
        }
    }
    public bool LoadData(ref UI interfañe)
    {
        if (File.Exists(Application.persistentDataPath
    + "/MySaveData.dat"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file =
              File.Open(Application.persistentDataPath
              + "/MySaveData.dat", FileMode.Open);
            SaveData data = (SaveData)bf.Deserialize(file);
            file.Close();
            Grid.Value = data.Grid;
            interfañe.CircuiteBar.fillAmount = data.CircuitBar;
            Debug.Log("Game data loaded!");
            return true;
        }
        else
            return false;
    }
    public void ResetData()
    {
        if (File.Exists(Application.persistentDataPath
          + "/MySaveData.dat"))
        {
            File.Delete(Application.persistentDataPath
              + "/MySaveData.dat");
            Debug.Log("Data reset complete!");
        }
    }
}
