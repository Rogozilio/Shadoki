using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gibi : MonoBehaviour, IUnit
{
    private bool _isMoveEnd;
    private Transform _transform;
    private AI _ai;
    public bool IsMoveEnd { get => _isMoveEnd; set => _isMoveEnd = value; }
    public Transform Transform { get => _transform; set => _transform = value; }
    public void Start()
    {
        _transform = GetComponent<Transform>();
    }
    private void CheckMove(Vector3 dir)
    {
        Vector3Int oldPos = Vector3Int.FloorToInt(transform.position);
        Vector3Int newPos = oldPos + Vector3Int.FloorToInt(dir);
        if (Grid.Value[newPos.x, newPos.y] == '0'
            || Grid.Value[newPos.x, newPos.y] == 'f'
            || Grid.Value[newPos.x, newPos.y] == 'e')
        {
            transform.position += dir;
            Grid.SwapMark(oldPos, newPos);
            _isMoveEnd = true;
        }
    }
    public void Move(Vector3 dir)
    {
        CheckMove(dir);
    }
}
