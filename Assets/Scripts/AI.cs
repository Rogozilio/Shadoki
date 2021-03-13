using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI
{
    public static List<Vector2> _locationGibi;
    private List<Vector2> _locationFlower;
    private bool[] _isFreeFlower;
    private float[,] _distacneBtwGibiAndFlower;
    private float[] sumDistanceDesñ;
    public static Dictionary<byte, Vector2> _dir;

    public AI()
    {
        _locationGibi = new List<Vector2>();
        _locationFlower = new List<Vector2>();
        _dir = new Dictionary<byte, Vector2>();
    }
    private void SetLocationGibiAndFlower()
    {
        for (int i = 1; i < GlobalData.gameWidth - 1; i++)
        {
            for (int j = 1; j < GlobalData.gameHeight - 1; j++)
            {
                if (Grid.Value[i, j] == 'g')
                {
                    _locationGibi.Add(new Vector2(i, j));
                }
                else if (Grid.Value[i, j] == 'f')
                {
                    _locationFlower.Add(new Vector2(i, j));
                }
            }
        }
    }
    private void SetFreeFlower()
    {
        _isFreeFlower = new bool[_locationFlower.Count];

        for (int i = 0; i < _isFreeFlower.Length; i++)
        {
            _isFreeFlower[i] = true;
        }
    }
    private void SetArrayDistance()
    {
        _distacneBtwGibiAndFlower = new float[_locationGibi.Count, _locationFlower.Count + 1];

        for (int i = 0; i < _locationGibi.Count; i++)
        {
            for (int j = 0; j < _locationFlower.Count; j++)
            {
                _distacneBtwGibiAndFlower[i, j] = Vector2.Distance(_locationGibi[i], _locationFlower[j]);
                _distacneBtwGibiAndFlower[i, _locationFlower.Count] += _distacneBtwGibiAndFlower[i, j];
            }
        }
    }
    private void SetSumDistanceDesc()
    {
        //Ïîëó÷åíèÿ îäíîìåðíîãî ñîðòèðîâàííîãî ìàññèâà ïî óáûâàíèþ
        sumDistanceDesñ = new float[_locationGibi.Count];
        for (int i = 0; i < _locationGibi.Count; i++)
        {
            sumDistanceDesñ[i] = _distacneBtwGibiAndFlower[i, _locationFlower.Count];
        }
        Array.Sort(sumDistanceDesñ);
        Array.Reverse(sumDistanceDesñ);
    }
    private void WalkSumDistanceDesc()
    {
        foreach (float maxDistance in sumDistanceDesñ)
        {
            for (byte i = 0; i < _locationGibi.Count; i++)
            {
                if (_distacneBtwGibiAndFlower[i, _locationFlower.Count] == maxDistance)
                {
                    FindFreeFlowerForGibi(i);
                    break;
                }
            }
        }
    }
    private void FindFreeFlowerForGibi(byte i)
    {
        byte index = GetIndexMinDistanceFreeFlower();
        float min = _distacneBtwGibiAndFlower[i, index];

        for (byte j = 0; j < _locationFlower.Count; j++)
        {
            if (_distacneBtwGibiAndFlower[i, j] < min
                && _isFreeFlower[j] == true)
            {
                min = _distacneBtwGibiAndFlower[i, j];
                index = j;
            }
        }
        DeleteFreeFlower(index);
        if (!_dir.ContainsKey(i))
            _dir.Add(i, _locationFlower[index]);
    }
    private byte GetIndexMinDistanceFreeFlower()
    {
        for (byte i = 0; i < _locationFlower.Count; i++)
        {
            if (_isFreeFlower[i] == true)
                return i;
        }
        return 0;
    }
    private void DeleteFreeFlower(byte index)
    {
        _isFreeFlower[index] = false;
    }
    private Vector2 RoundDirection(Vector2 dir)
    {
        Vector2 newDir = Vector2.zero;
        if (dir.x > 0)
            newDir += Vector2.right;
        else if (dir.x < 0)
            newDir += Vector2.left;
        if (dir.y > 0)
            newDir += Vector2.up;
        else if (dir.y < 0)
            newDir += Vector2.down;
        return newDir;
    }
    private Vector2 CheckDirection(Vector2 posGibi, Vector2 dir)
    {
        Vector2 oldDir = RoundDirection(dir);
        Vector2 newPos = posGibi + oldDir;

        for (int numberDirection = 4; numberDirection >= 0; numberDirection--)
        {
            if ((Grid.GetMark(newPos) == '0'
                || Grid.GetMark(newPos) == 'f')
                && oldDir != Vector2.zero
                && oldDir.x >= -1 && oldDir.x <= 1
                && oldDir.y >= -1 && oldDir.y <= 1)
            {
                return oldDir;
            }
            oldDir = RoundDirection(dir);
            switch (numberDirection)
            {
                case 4: oldDir += Vector2.up; break;
                case 3: oldDir += Vector2.right; break;
                case 2: oldDir += Vector2.left; break;
                case 1: oldDir += Vector2.down; break;
            }
            newPos = posGibi + oldDir;
        }
        return Vector2.zero;
    }
    private void ClearDataTravel()
    {
        _locationGibi.Clear();
        _locationFlower.Clear();
        _dir.Clear();
    }
    public Vector2 GetDir(Vector2 posUnit)
    {
        if (Grid.Value[(int)posUnit.x, (int)posUnit.y] == 'g')
        {
            ClearDataTravel();
            SetLocationGibiAndFlower();
            for (byte i = 0; i < _locationGibi.Count; i++)
            {
                if (_locationGibi[i] == posUnit)
                { 
                    SetArrayDistance();
                    SetFreeFlower();
                    SetSumDistanceDesc();
                    WalkSumDistanceDesc();
                    Vector2 dir = (_dir[i] - posUnit).normalized;
                    return CheckDirection(posUnit, dir);
                }
            }
        }
        return Vector2.zero;
    }
}
