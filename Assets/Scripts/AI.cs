using System;
using System.Collections.Generic;
using UnityEngine;

public class AI
{
    private bool                        _isExistFinish;
    private bool[]                      _isFreeFlower;
    private float[]                     _sumDistanceDesс;
    private float[,]                    _distacneBtwGibiAndFlower;
    private List<Vector2>               _locationGibi;
    private List<Vector2>               _locationFlower;
    private Dictionary<byte, Vector2>   _posTarget;

    public AI()
    {
        _locationGibi   = new List<Vector2>();
        _locationFlower = new List<Vector2>();
        _posTarget      = new Dictionary<byte, Vector2>();
    }
    /// <summary>
    /// Получение кординат активных объектов из сетки
    /// </summary>
    private void SetLocationActiveObjects()
    {
        Vector2 locationShadok = Vector2.zero;
        for (int i = 1; i < Grid.GameWidth - 1; i++)
        {
            for (int j = 1; j < Grid.GameHeight - 1; j++)
            {
                if (Grid.Value[i, j] == 's')
                {
                    locationShadok = new Vector2(i, j);
                }
                else if (Grid.Value[i, j] == 'g')
                {
                    _locationGibi.Add(new Vector2(i, j));
                }
                else if (Grid.Value[i, j] == 'f')
                {
                    _locationFlower.Add(new Vector2(i, j));
                }
                else if(Grid.Value[i, j] == 'e')
                {
                    _isExistFinish = true;
                }
            }
        }
        if(_locationGibi.Count > _locationFlower.Count)
        {
            _locationFlower.Add(locationShadok);
        }
    }
    /// <summary>
    /// Освобождение flowers
    /// </summary>
    private void SetFreeFlower()
    {
        _isFreeFlower = new bool[_locationFlower.Count];

        for (int i = 0; i < _isFreeFlower.Length; i++)
        {
            _isFreeFlower[i] = true;
        }
    }
    /// <summary>
    /// Задает массив дистанций между gibi и flowers 
    /// </summary>
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
    /// <summary>
    /// Задает сумму дистанций gibi до всех flowers в порядке убывания
    /// </summary>
    private void SetSumDistanceDesc()
    {
        _sumDistanceDesс = new float[_locationGibi.Count];
        for (int i = 0; i < _locationGibi.Count; i++)
        {
            _sumDistanceDesс[i] = _distacneBtwGibiAndFlower[i, _locationFlower.Count];
        }
        Array.Sort(_sumDistanceDesс);
        Array.Reverse(_sumDistanceDesс);
    }
    /// <summary>
    /// Перемещаем gibi с большой суммой дистанций до меньшей
    /// </summary>
    private void WalkSumDistanceDesc()
    {
        foreach (float maxDistance in _sumDistanceDesс)
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
    /// <summary>
    /// Gibi получает не занятый другим gibi ближайший цветочек
    /// </summary>
    /// <param name="i">номер gibi</param>
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
        if (_isExistFinish)
        {
            _posTarget.Add(i, new Vector2(Grid.GameWidth - 2, Grid.GameHeight - 4)); 
        }
        else if(!_posTarget.ContainsKey(i))
        {
            _posTarget.Add(i, _locationFlower[index]);
        }        
    }
    /// <summary>
    /// Получений индекса свободного flower
    /// </summary>
    private byte GetIndexMinDistanceFreeFlower()
    {
        for (byte i = 0; i < _locationFlower.Count; i++)
        {
            if (_isFreeFlower[i] == true)
                return i;
        }
        return 0;
    }
    /// <summary>
    /// Gibi занимает flower по индексу
    /// </summary>
    private void DeleteFreeFlower(byte index)
    {
        _isFreeFlower[index] = false;
    }
    /// <summary>
    /// Округление полученного направления до целого числа 
    /// </summary>
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
    /// <summary>
    /// Проверяет свободна ли клетка для перемещения,
    /// иначе ищет соседнию свободную клетку
    /// </summary>
    private Vector2 CheckDirection(Vector2 posGibi, Vector2 dir)
    {
        Vector2 oldDir = RoundDirection(dir);
        Vector2 newPos = posGibi + oldDir;

        for (int numberDirection = 4; numberDirection >= 0; numberDirection--)
        {
            if ((Grid.GetMark(newPos) == '0'
                || Grid.GetMark(newPos) == 'f'
                || Grid.GetMark(newPos) == 'e')
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
    /// <summary>
    /// очещение данных для расчета следующих ходов
    /// </summary>
    private void ClearDataTravel()
    {
        _isExistFinish = false;
        _locationGibi.Clear();
        _locationFlower.Clear();
        _posTarget.Clear();
    }
    /// <summary>
    /// Выдает направление движения для gibi
    /// </summary>
    public Vector2 GetDir(Vector2 posUnit)
    {
        if (Grid.Value[(int)posUnit.x, (int)posUnit.y] == 'g')
        {
            ClearDataTravel();
            SetLocationActiveObjects();
            for (byte i = 0; i < _locationGibi.Count; i++)
            {
                if (_locationGibi[i] == posUnit)
                { 
                    SetArrayDistance();
                    SetFreeFlower();
                    SetSumDistanceDesc();
                    WalkSumDistanceDesc();
                    Vector2 dir = (_posTarget[i] - posUnit).normalized;
                    return CheckDirection(posUnit, dir);
                }
            }
        }
        return Vector2.zero;
    }
}
