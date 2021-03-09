using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gibi : MonoBehaviour, IUnit
{
    private bool _isMoveEnd;
    private Vector3Int _currentPos;
    private Vector3Int _targetPos;
    private Transform _transform;
    private Animator _animator;
    private AI _ai;
    public bool IsMoveEnd { get => _isMoveEnd; set => _isMoveEnd = value; }
    public Transform Transform { get => _transform; set => _transform = value; }
    public void Start()
    {
        _targetPos = Vector3Int.zero;
        _transform = GetComponent<Transform>();
        _animator = GetComponent<Animator>();
    }
    /// <summary>
    /// ��������� �������� ��������� � ��� ����������
    /// </summary>
    /// <param name="dir">����������� �����������</param>
    /// <param name="isInMove">�������� ��������?</param>
    private void SetAnimation(Vector3 dir, bool isInMove = false)
    {
        if (dir == Vector3Int.up)
        {
            _animator.SetInteger("Gibi", (isInMove) ? 11 : 1);
        }
        else if (dir == Vector3Int.right + Vector3Int.up)
        {
            _animator.SetInteger("Gibi", (isInMove) ? 22 : 2);
        }
        else if (dir == Vector3Int.right)
        {
            _animator.SetInteger("Gibi", (isInMove) ? 33 : 3);
        }
        else if (dir == Vector3Int.right + Vector3Int.down)
        {
            _animator.SetInteger("Gibi", (isInMove) ? 44 : 4);
        }
        else if (dir == Vector3Int.down)
        {
            _animator.SetInteger("Gibi", (isInMove) ? 55 : 5);
        }
        else if (dir == Vector3Int.left + Vector3Int.down)
        {
            _animator.SetInteger("Gibi", (isInMove) ? 66 : 6);
        }
        else if (dir == Vector3Int.left)
        {
            _animator.SetInteger("Gibi", (isInMove) ? 77 : 7);
        }
        else if (dir == Vector3Int.left + Vector3Int.up)
        {
            _animator.SetInteger("Gibi", (isInMove) ? 88 : 8);
        }
    }
    private void GoTowardDirection(Vector3Int dir)
    {
        transform.position = Vector3.MoveTowards(transform.position, _targetPos, 10f * Time.deltaTime);
        if (Vector3.Distance(transform.position, _targetPos) < 0.001f)
        {
            SetAnimation(_targetPos - _currentPos);
            Grid.SwapMark(_currentPos, _targetPos);
            transform.position = _targetPos;
            _targetPos = Vector3Int.zero;
            _isMoveEnd = true;
        }
    }
    private void CheckMove(Vector3 dir)
    {
        Vector3Int newPos = Vector3Int.FloorToInt(transform.position + dir);
        if (Grid.Value[newPos.x, newPos.y] == '0'
            || Grid.Value[newPos.x, newPos.y] == 'f'
            || Grid.Value[newPos.x, newPos.y] == 'e')
        {
            _targetPos = newPos;
            _currentPos = Vector3Int.FloorToInt(transform.position);
        }
    }
    public void Move(Vector3 dir)
    {
        if (_targetPos == Vector3Int.zero)
        {
            CheckMove(dir);
        }
        else
        {
            SetAnimation(_targetPos - _currentPos, true);
            GoTowardDirection(Vector3Int.FloorToInt(dir));
        }
    }
}
