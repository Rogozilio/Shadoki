using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shadok : MonoBehaviour, IUnit
{
    private bool _isMoveEnd;
    private Transform _transform;
    private UI _interface;
    private Vector3Int _pastDir;
    private Animator _animator;
    private Vector3Int _currentPos;
    private Vector3Int _targetPos;
    private Transform _pointer;
    public bool IsMoveEnd { get => _isMoveEnd; set => _isMoveEnd = value; }
    public Transform Transform { get => _transform; }

    private void Start()
    {
        _targetPos = Vector3Int.zero;
        _animator = GetComponent<Animator>();
        _transform = GetComponent<Transform>();
        _pointer = _transform.GetChild(0);
        _interface = GameObject.FindGameObjectWithTag("UI").GetComponent<UI>();
        _pastDir = _interface.GetDirection();
    }
    private void CheckMove(Vector3Int dir)
    {
        Vector3Int newPos = Vector3Int.FloorToInt(transform.position) + dir;

        if (Grid.Value[newPos.x, newPos.y] == '0'
            || Grid.Value[newPos.x, newPos.y] == 'f'
            || Grid.Value[newPos.x, newPos.y] == 'e')
        {
            _targetPos = Vector3Int.FloorToInt(transform.position + dir);
            _currentPos = Vector3Int.FloorToInt(transform.position);
            SetAnimation(dir, true, true);
        }
    }
    private void KeyboardControl()
    {
        Vector3Int dir = Vector3Int.zero;

        if (Input.GetAxis("Horizontal") > 0
            && Input.GetAxis("Vertical") > 0)
        {
            dir = Vector3Int.right + Vector3Int.up;
        }
        else if (Input.GetAxis("Horizontal") < 0
            && Input.GetAxis("Vertical") > 0)
        {
            dir = Vector3Int.left + Vector3Int.up;
        }
        else if (Input.GetAxis("Horizontal") > 0
            && Input.GetAxis("Vertical") < 0)
        {
            dir = Vector3Int.right + Vector3Int.down;
        }
        else if (Input.GetAxis("Horizontal") < 0
            && Input.GetAxis("Vertical") < 0)
        {
            dir = Vector3Int.left + Vector3Int.down;
        }
        else if (Input.GetAxis("Horizontal") > 0)
        {
            dir = Vector3Int.right;
        }
        else if (Input.GetAxis("Horizontal") < 0)
        {
            dir = Vector3Int.left;
        }
        else if (Input.GetAxis("Vertical") > 0)
        {
            dir = Vector3Int.up;
        }
        else if (Input.GetAxis("Vertical") < 0)
        {
            dir = Vector3Int.down;
        }
        SetAnimation(dir);
        if (Input.GetKeyDown("space"))
        {
            CheckMove(dir);
        }
    }
    private void MouseControl()
    {
        if(_interface.isUIControl)
        {
            if (_interface.GetDirection() == Vector3Int.zero
                && _pastDir != Vector3Int.zero)
            {
                CheckMove(_pastDir);
                _pastDir = _interface.GetDirection();
            }
            else
            {
                _pastDir = _interface.GetDirection();
                SetAnimation(_pastDir, false);
            }
        }
    }
    /// <summary>
    /// Включение анимации персонажа и его интерфейса
    /// </summary>
    /// <param name="dir">Направление перемещения</param>
    /// <param name="isControlKeyboard">Управление клавиатурой?</param>
    /// <param name="isInMove">Анимация движения?</param>
    private void SetAnimation(Vector3Int dir,bool isControlKeyboard = false, bool isInMove = false)
    {
        if (isControlKeyboard)
            _interface.isUIControl = false;
        if (dir == Vector3Int.up)
        {
            _animator.SetInteger("Shadok", (isInMove) ? 11 : 1);
            _interface.SetButtonSelected((isInMove) ? "Step" : "Up");
        }
        else if (dir == Vector3Int.right + Vector3Int.up)
        {
            _animator.SetInteger("Shadok", (isInMove) ? 22 : 2);
            _interface.SetButtonSelected((isInMove) ? "Step" : "Right_up");
        }
        else if (dir == Vector3Int.right)
        {
            _animator.SetInteger("Shadok", (isInMove) ? 33 : 3);
            _interface.SetButtonSelected((isInMove) ? "Step" : "Right");
        }
        else if (dir == Vector3Int.right + Vector3Int.down)
        {
            _animator.SetInteger("Shadok", (isInMove) ? 44 : 4);
            _interface.SetButtonSelected((isInMove) ? "Step" : "Right_down");
        }
        else if (dir == Vector3Int.down)
        {
            _animator.SetInteger("Shadok", (isInMove) ? 55 : 5);
            _interface.SetButtonSelected((isInMove) ? "Step" : "Down");
        }
        else if (dir == Vector3Int.left + Vector3Int.down)
        {
            _animator.SetInteger("Shadok", (isInMove) ? 66 : 6);
            _interface.SetButtonSelected((isInMove) ? "Step" : "Left_down");
        }
        else if (dir == Vector3Int.left)
        {
            _animator.SetInteger("Shadok", (isInMove) ? 77 : 7);
            _interface.SetButtonSelected((isInMove) ? "Step" : "Left");
        }
        else if (dir == Vector3Int.left + Vector3Int.up)
        {
            _animator.SetInteger("Shadok", (isInMove) ? 88 : 8);
            _interface.SetButtonSelected((isInMove) ? "Step" : "Left_up");
        }
        else if(dir == Vector3Int.zero)
        {
            _animator.SetInteger("Shadok", (isInMove) ? 55 : 5);
            _interface.SetButtonSelected((isInMove) ? "Step" : "Empty");
        }
    }
    private void GoTowardDirection()
    {
        transform.position = Vector3.MoveTowards(transform.position, _targetPos, 10f * Time.deltaTime);
        if (Vector3.Distance(transform.position, _targetPos) < 0.001f)
        {
            transform.position = _targetPos;
            Grid.SwapMark(_currentPos, _targetPos);
            CheckFinish();
            _isMoveEnd = true;
            SetAnimation(_targetPos - _currentPos);
            _targetPos = Vector3Int.zero;
            _pointer.gameObject.SetActive(false);
        }
    }
    private void CheckFinish()
    {
        if (Grid.GetMark(transform.position) == 'e')
        {
            Debug.Log("End");
        }
    }
    public void Move(Vector3 dir)
    {
        if (_targetPos != Vector3.zero)
        {
            GoTowardDirection();
        }
        else
        {
            _pointer.gameObject.SetActive(true);
            KeyboardControl();
            MouseControl();
        }
    }
}