using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System;

[Serializable]
public class SaveData
{
    public char[,]  Grid;
    public float    ShadokBar;
    public float    BadokBar;
}
public class SaveLoad
{
    /// <summary>
    /// Сохранение данных
    /// </summary>
    public void SaveData(float shadokBar, float badokBar = 0, string name = "Shadok")
    {
        if(shadokBar != 1 && badokBar != 1)
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Create(Application.persistentDataPath
              + "/" + name + ".dat");
            SaveData data = new SaveData();
            data.Grid = Grid.Value;
            data.ShadokBar = shadokBar;
            data.BadokBar = badokBar;
            bf.Serialize(file, data);
            file.Close();
        }
    }
    /// <summary>
    /// Загрузка данных
    /// </summary>
    public bool LoadData(ref UI interfaсe, string name = "Shadok")
    {
        if (File.Exists(Application.persistentDataPath
    + "/" + name + ".dat"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file =
              File.Open(Application.persistentDataPath
              + "/" + name + ".dat", FileMode.Open);
            SaveData data = (SaveData)bf.Deserialize(file);
            file.Close();
            Grid.Value = data.Grid;
            interfaсe.ShadokBar.fillAmount = data.ShadokBar;
            interfaсe.BadokBar.fillAmount = data.BadokBar;
            return true;
        }
        else
            return false;
    }
    /// <summary>
    /// Очищение данных
    /// </summary>
    public void ResetData(string name = "Shadok")
    {
        if (File.Exists(Application.persistentDataPath
          + "/" + name + ".dat"))
        {
            File.Delete(Application.persistentDataPath
              + "/" + name + ".dat");
        }
    }
}
