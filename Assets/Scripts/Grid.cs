using UnityEngine;

public class Grid
{
    public static byte GameWidth { get => 10 + 2; }
    public static byte GameHeight { get => 10 + 2; }
    public static byte Width { get => (byte)(GameWidth + 4); }

    public static char[,] Value = new char[Width, GameHeight];
    static Grid()
    {
        //Заполнение сетки игрового поля
        for (int i = 0; i < GameWidth; i++)
        {
            for (int j = 0; j < GameHeight; j++)
            {
                if (i == 0 && j == 0)
                    Value[i, j] = '└';
                else if (i == 0 && j == GameHeight - 1)
                    Value[i, j] = '┌';
                else if (i == GameWidth - 1 && j == 0)
                    Value[i, j] = '┴';
                else if (i == GameWidth - 1 && j == GameHeight - 1)
                    Value[i, j] = '┐';
                else if (i == GameWidth - 1 && j == 4)
                    Value[i, j] = '├';
                else if (i == 0 || i == GameHeight - 1)
                    Value[i, j] = '|';
                else if (j == 0 || j == GameHeight - 1)
                    Value[i, j] = '-';
                else
                    Value[i, j] = '0';
            }
        }
        //Заполнение оставшейся сетки
        for (int i = GameWidth; i < Width; i++)
        {
            for (int j = 0; j < GameHeight; j++)
            {
                Value[i, j] = '0';
                if ((j == 4 || j == 0) && i < Width - 1)
                    Value[i, j] = '-';
                else if (i == Width - 1 && j > 0 && j < 4)
                    Value[i, j] = '|';
                else if (i == Width - 1 && j == 4)
                    Value[i, j] = '┐';
                else if (i == Width - 1 && j == 0)
                    Value[i, j] = '┘';
            }
        }
    }
    /// <summary>
    /// Заполнить сетку значениями в случайных местах
    /// </summary>
    public static void SetMark(char mark, byte count = 1)
    {
        while (count > 0)
        {
            byte x = (byte)Random.Range(1, GameWidth - 1);
            byte y = (byte)Random.Range(1, GameHeight - 1);
            if (Value[x, y] == '0')
            {
                string number = "123456789";
                Value[x, y] = (mark == 'f') ? number[Random.Range(0, 9)] : mark;
                count--;
            }
        }
    }
    /// <summary>
    /// Заполнить сетку значением в случайном месте и получить координату этого места
    /// </summary>
    public static Vector2 SetMarkGetPosition(char mark)
    {
        while (true)
        {
            byte x = (byte)Random.Range(1, GameWidth - 1);
            byte y = (byte)Random.Range(1, GameHeight - 1);
            if (Value[x, y] == '0')
            {
                string number = "123456789";
                Value[x, y] = (mark == 'f') ? number[Random.Range(0, 9)] : mark;
                return new Vector2(x, y);
            }
        }
    }
    /// <summary>
    /// Получить координат сетки по символу
    /// </summary>
    public static Vector3Int GetMarkPosition(char mark)
    {
        for (int i = 0; i < GameWidth; i++)
        {
            for (int j = 0; j < GameHeight; j++)
            {
                if (Value[i, j] == mark)
                {
                    return new Vector3Int(i, j, -1);
                }
            }
        }
        return Vector3Int.zero;
    }
    /// <summary>
    /// Получить значения сетки по координатам
    /// </summary>
    public static char GetMark(Vector3 pos)
    {
        return Value[(int)pos.x, (int)pos.y];
    }
    /// <summary>
    /// Перемещение объектов через сетку
    /// </summary>
    public static void SwapMark(Vector3Int oldPos, Vector3Int newPos)
    {
        Value[newPos.x, newPos.y] = Value[oldPos.x, oldPos.y];
        Value[oldPos.x, oldPos.y] = '0';
    }
    /// <summary>
    /// получения количества знаков в сетке
    /// </summary>
    public static byte CountMark(string marks)
    {
        byte count = 0;
        for (int i = 0; i < GameWidth; i++)
        {
            for (int j = 0; j < GameHeight; j++)
            {
                foreach(char mark in marks)
                {
                    if (Value[i, j] == mark)
                    {
                        count++;
                        break;
                    }
                }
            }
        }
        return count;
    }
    /// <summary>
    /// 
    /// </summary>
    public static bool IsPathFree(Vector3Int pos, string marks)
    {
        foreach(char mark in marks)
        {
            if (Value[pos.x, pos.y] == mark)
            {
                return true;
            }
        }
        return false;
    }
    /// <summary>
    /// 
    /// </summary>
    public static bool IsPathFree(int x, int y, string marks)
    {
        foreach (char mark in marks)
        {
            if (Value[x, y] == mark)
            {
                return true;
            }
        }
        return false;
    }
    /// <summary>
    /// Вывести сетку в консоль
    /// </summary>
    public static void Show()
    {
        string row = "";
        for (int x = GameHeight - 2; x > 0; x--) 
        {
            for (int y = 1; y < GameWidth - 1; y++)
            {
                row += Value[y,x] + " ";
            }
            row += "\n";
        }
        Debug.Log(row);
    }
}
