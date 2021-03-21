using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid
{
    public static byte GameWidth { get => 10 + 2; }
    public static byte GameHeight { get => 10 + 2; }
    public static byte Width { get => (byte)(GameWidth + 4); }
    public static char[,] Value
        = new char[Width, GameHeight];
    static Grid()
    {
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
    public static void SetMark(char gameObjectMark, byte count = 1)
    {
        while (count > 0)
        {
            byte x = (byte)Random.Range(1, GameWidth - 1);
            byte y = (byte)Random.Range(1, GameHeight - 1);
            if (Value[x, y] == '0')
            {
                Value[x, y] = gameObjectMark;
                count--;
            }
        }
    }
    public static char GetMark(Vector3 pos)
    {
        return Value[(int)pos.x, (int)pos.y];
    }
    public static Vector2 SetMarkGetPosition(char gameObjectMark)
    {
        while (true)
        {
            byte x = (byte)Random.Range(1, GameWidth - 1);
            byte y = (byte)Random.Range(1, GameHeight - 1);
            if (Value[x, y] == '0')
            {
                Value[x, y] = gameObjectMark;
                return new Vector2(x, y);
            }
        }
    }
    public static void SwapMark(Vector3Int oldPos, Vector3Int newPos)
    {
        Value[newPos.x, newPos.y] = Value[oldPos.x, oldPos.y];
        Value[oldPos.x, oldPos.y] = '0';
    }
    public static byte CountMark(char mark)
    {
        byte count = 0;
        for (int i = 0; i < GameWidth; i++)
        {
            for (int j = 0; j < GameHeight; j++)
            {
                if (Value[i, j] == mark)
                {
                    count++;
                }
            }
        }
        return count;
    }
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
