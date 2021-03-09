using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI
{
    private List<Vector2> _locationGibi;
    private List<Vector2> _locationFlower;
    private byte[,] _distacneBtwGibiAndFlower;
    private byte[] sumDistanceDes�;
    private Dictionary<byte, Vector2> _dir;
    private bool[] _isFreeFlower;

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
        _distacneBtwGibiAndFlower = new byte[_locationGibi.Count, _locationFlower.Count + 1];

        for (int i = 0; i < _locationGibi.Count; i++)
        {
            for (int j = 0; j < _locationFlower.Count; j++)
            {
                _distacneBtwGibiAndFlower[i, j] = (byte)Vector2.Distance(_locationGibi[i], _locationFlower[j]);
                _distacneBtwGibiAndFlower[i, _locationFlower.Count] += _distacneBtwGibiAndFlower[i, j];
            }
        }
    }
    private void SetSumDistanceDesc()
    {
        //��������� ����������� �������������� ������� �� ��������
        sumDistanceDes� = new byte[_locationGibi.Count];
        for (int i = 0; i < _locationGibi.Count; i++)
        {
            sumDistanceDes�[i] = _distacneBtwGibiAndFlower[i, _locationFlower.Count];
        }
        Array.Sort(sumDistanceDes�);
        Array.Reverse(sumDistanceDes�);
    }
    private void WalkSumDistanceDesc()
    {
        foreach (byte maxDistance in sumDistanceDes�)
        {
            for (byte i = 0; i < _locationGibi.Count; i++)
            {
                if (_distacneBtwGibiAndFlower[i, _locationFlower.Count] == maxDistance)
                {
                    FindFreeFlowerAtGibi(i);
                }
            }
        }
    }
    private void FindFreeFlowerAtGibi(byte i)
    {
        byte min = _distacneBtwGibiAndFlower[i, 0];
        byte index = 0;

        for (byte j = 0; j < _locationFlower.Count - 1; j++)
        {
            if (_distacneBtwGibiAndFlower[i, j] < min && _isFreeFlower[j] == true)
            {
                min = _distacneBtwGibiAndFlower[i, j];
                index = j;
            }
        }
        DeleteFreeFlower(index);
        _dir.Add(i, _locationFlower[index]);
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

        for (int numberDirection = 4; numberDirection > 0; numberDirection--)
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
        if (Grid.Value[(int)posUnit.x, (int)posUnit.y] == 's'
            && _dir.Count == 0)
        {
            SetLocationGibiAndFlower();
            SetFreeFlower();
            SetArrayDistance();
            SetSumDistanceDesc();
            WalkSumDistanceDesc();
            return Vector2.zero;
        }
        for (byte i = 0; i < _locationGibi.Count; i++)
        {
            if (_locationGibi[i] == posUnit)
            {
                Vector2 dir = (_dir[i] - posUnit).normalized;
                if (i == _locationGibi.Count - 1)
                {
                    ClearDataTravel();
                }
                return CheckDirection(posUnit, dir);
            }
        }
        return Vector2.zero;
    }
}
