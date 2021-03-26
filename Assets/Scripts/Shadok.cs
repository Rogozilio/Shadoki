using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shadok : MonoBehaviour, IUnit
{
    private string      _name;
    private bool        _isMoveEnd;
    private bool        _isGameSaved;
    private UI          _interface;
    private SaveLoad    _saveLoad;
    private Transform   _transform;
    private Transform   _pointer;
    private Vector3Int  _pastDir;
    private Vector3Int  _currentPos;
    private Vector3Int  _targetPos;
    private Animator    _animator;
    private AudioSource _audio;

    public AudioClip    AudioPickUp;
    public AudioClip    AudioDeadlock;
    public bool IsMoveEnd { get => _isMoveEnd; set => _isMoveEnd = value; }
    public Transform Transform { get => _transform; }

    private void Start()
    {
        _name           = gameObject.name.Replace("(Clone)", "");
        _isGameSaved    = false;
        _targetPos      = Vector3Int.zero;
        _saveLoad       = new SaveLoad();
        _animator       = GetComponent<Animator>();
        _transform      = GetComponent<Transform>();
        _pointer        = _transform.GetChild(0);
        _audio          = GetComponent<AudioSource>();
        _interface      = GameObject.FindGameObjectWithTag("UI").GetComponent<UI>();
        _pastDir        = _interface.GetDirection();

        _pointer.gameObject.SetActive(false);
    }
    /// <summary>
    /// Проверка на свободную клетку
    /// </summary>
    private void CheckMove(Vector3Int dir)
    {
        Vector3Int newPos 
            = Vector3Int.FloorToInt(transform.position) + dir;

        if (Grid.IsPathFree(newPos, "e0123456789"))
        {
            _targetPos = Vector3Int.FloorToInt(transform.position + dir);
            _currentPos = Vector3Int.FloorToInt(transform.position);
            SetAnimation(dir, true, true);
            if(Grid.IsPathFree(newPos, "123456789"))
            {
                _audio.clip = AudioPickUp;
                _audio.Play();
            }
        }
        else
        {
            _audio.clip = AudioDeadlock;
            _audio.Play();
        }
    }
    /// <summary>
    /// Управление клавиатурой
    /// </summary>
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
    /// <summary>
    /// Управление мышью
    /// </summary>
    private void MouseControl()
    {
        if (_interface.isUIControl)
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
    /// 
    /// </summary>
    public void RegulateProgressBar(string points)
    {
        if(points != "e")
        {
            if (_name == "Shadok")
            {
                _interface.ShadokBar.fillAmount += float.Parse(points) / 100f;
            }
            else
            {
                _interface.BadokBar.fillAmount += float.Parse(points) / 100f;
            }
            if (_interface.ShadokBar.fillAmount + _interface.BadokBar.fillAmount > 1
                && _interface.ShadokBar.fillAmount != 1
                && _interface.BadokBar.fillAmount != 1)
            {
                if (_name == "Shadok")
                {
                    _interface.BadokBar.fillAmount = 1.00f - _interface.ShadokBar.fillAmount;
                }
                else
                {
                    _interface.ShadokBar.fillAmount = 1.00f - _interface.BadokBar.fillAmount;
                }
            }
        }
    }
    /// <summary>
    /// Включение анимации персонажа и его интерфейса
    /// </summary>
    /// <param name="dir">Направление перемещения</param>
    /// <param name="isControlKeyboard">Управление клавиатурой?</param>
    /// <param name="isInMove">Анимация движения?</param>
    private void SetAnimation(Vector3Int dir, bool isControlKeyboard = false, bool isInMove = false)
    {
        if (isControlKeyboard)
            _interface.isUIControl = false;
        if (dir == Vector3Int.up)
        {
            _animator.SetInteger(_name, (isInMove) 
                ? (int)AnimMove.Up : (int)AnimStay.Up);
            _interface.SetButtonSelected((isInMove) ? "Step" : "Up");
        }
        else if (dir == Vector3Int.right + Vector3Int.up)
        {
            _animator.SetInteger(_name, (isInMove) 
                ? (int)AnimMove.RightUp : (int)AnimStay.RightUp);
            _interface.SetButtonSelected((isInMove) ? "Step" : "Right_up");
        }
        else if (dir == Vector3Int.right)
        {
            _animator.SetInteger(_name, (isInMove) 
                ? (int)AnimMove.Right : (int)AnimStay.Right);
            _interface.SetButtonSelected((isInMove) ? "Step" : "Right");
        }
        else if (dir == Vector3Int.right + Vector3Int.down)
        {
            _animator.SetInteger(_name, (isInMove) 
                ? (int)AnimMove.RightDown : (int)AnimStay.RightDown);
            _interface.SetButtonSelected((isInMove) ? "Step" : "Right_down");
        }
        else if (dir == Vector3Int.down)
        {
            _animator.SetInteger(_name, (isInMove) 
                ? (int)AnimMove.Down : (int)AnimStay.Down);
            _interface.SetButtonSelected((isInMove) ? "Step" : "Down");
        }
        else if (dir == Vector3Int.left + Vector3Int.down)
        {
            _animator.SetInteger(_name, (isInMove) 
                ? (int)AnimMove.LeftDown : (int)AnimStay.LeftDown);
            _interface.SetButtonSelected((isInMove) ? "Step" : "Left_down");
        }
        else if (dir == Vector3Int.left)
        {
            _animator.SetInteger(_name, (isInMove) 
                ? (int)AnimMove.Left : (int)AnimStay.Left);
            _interface.SetButtonSelected((isInMove) ? "Step" : "Left");
        }
        else if (dir == Vector3Int.left + Vector3Int.up)
        {
            _animator.SetInteger(_name, (isInMove) 
                ? (int)AnimMove.LeftUp : (int)AnimStay.LeftUp);
            _interface.SetButtonSelected((isInMove) ? "Step" : "Left_up");
        }
        else if (dir == Vector3Int.zero)
        {
            _animator.SetInteger(_name, (isInMove) 
                ? (int)AnimMove.Down : (int)AnimStay.Down);
            _interface.SetButtonSelected((isInMove) ? "Step" : "Empty");
        }
    }
    /// <summary>
    /// Передвижение к цели
    /// </summary>
    private void GoTowardDirection()
    {
        transform.position = Vector3.MoveTowards(transform.position, _targetPos, 10f * Time.deltaTime);
        if (Vector3.Distance(transform.position, _targetPos) < 0.001f)
        {
            transform.position = _targetPos;
            RegulateProgressBar(Grid.GetMark(_targetPos).ToString());
            Grid.SwapMark(_currentPos, _targetPos);
            _isMoveEnd = true;
            _isGameSaved = false;
            SetAnimation(_targetPos - _currentPos);
            _targetPos = Vector3Int.zero;
            _pointer.gameObject.SetActive(false);
            CheckFinish();
        }
    }
    /// <summary>
    /// При финише происходить выключение гг
    /// </summary>
    private void CheckFinish()
    {
        if ((_name == "Shadok" && Grid.CountMark("e") == 0
            && _interface.ShadokBar.fillAmount == 1)
            || (_name == "Shadok"  && Grid.CountMark("b") == 1 
            && _interface.ShadokBar.fillAmount == 1)
            || (_name == "Badok"  && Grid.CountMark("b") == 1) 
            && _interface.BadokBar.fillAmount == 1)
        {
            gameObject.SetActive(false);
        }
    }
    /// <summary>
    /// Двигатель объекта
    /// </summary>
    public void Move(Vector3 dir)
    {
        if(!_isGameSaved 
            && _interface.ShadokBar.fillAmount < 1
            && _interface.BadokBar.fillAmount < 1)
        {
            _saveLoad.SaveData(_interface.ShadokBar.fillAmount
                , _interface.BadokBar.fillAmount
                , (Grid.CountMark("b") == 1)? "Badok":"Shadok");
            _isGameSaved = true;
        }
        if (_targetPos != Vector3.zero)
        {
            GoTowardDirection();
        }
        else
        {
            _pointer.gameObject.SetActive(true);
            if(Grid.CountMark("b") != 1)
            {
                KeyboardControl();
                MouseControl();
            }
            else
            {
                if(_name == "Shadok")
                    KeyboardControl();
                else
                    MouseControl();
            }
        }
    }
}