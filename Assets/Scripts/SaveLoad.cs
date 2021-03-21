using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System;

[Serializable]
public class SaveData
{
    public char[,] Grid;
    public float CircuitBar;
}
public class SaveLoad
{
    /// <summary>
    /// Сохранение данных
    /// </summary>
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
        }
    }
    /// <summary>
    /// Загрузка данных
    /// </summary>
    public bool LoadData(ref UI interfaсe)
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
            interfaсe.CircuiteBar.fillAmount = data.CircuitBar;
            return true;
        }
        else
            return false;
    }
    /// <summary>
    /// Очищение данных
    /// </summary>
    public void ResetData()
    {
        if (File.Exists(Application.persistentDataPath
          + "/MySaveData.dat"))
        {
            File.Delete(Application.persistentDataPath
              + "/MySaveData.dat");
        }
    }
}
