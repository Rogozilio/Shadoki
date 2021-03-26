using UnityEngine;

public class Gibi : MonoBehaviour, IUnit
{
    private string      _name;
    private bool        _isMoveEnd;
    private Vector3Int  _currentPos;
    private Vector3Int  _targetPos;
    private Transform   _transform;
    private Transform   _pointer;
    private Animator    _animator;
    private AudioSource _audio;

    public AudioClip    AudioPickUp;
    public bool IsMoveEnd { get => _isMoveEnd; set => _isMoveEnd = value; }
    public Transform Transform { get => _transform; set => _transform = value; }
    public void Start()
    {
        _name = gameObject.name.Replace("(Clone)", "");
        _targetPos  = Vector3Int.zero;
        _audio      = GetComponent<AudioSource>();
        _transform  = GetComponent<Transform>();
        _animator   = GetComponent<Animator>();
        _pointer    = _transform.GetChild(0);
        
        _pointer.gameObject.SetActive(false);
    }
    /// <summary>
    /// Включение анимации персонажа
    /// </summary>
    private void SetAnimation(Vector3 dir, bool isInMove = false)
    {
        if (dir == Vector3Int.up)
        {
            _animator.SetInteger(_name, (isInMove)
                ? (int)AnimMove.Up : (int)AnimStay.Up);
        }
        else if (dir == Vector3Int.right + Vector3Int.up)
        {
            _animator.SetInteger(_name, (isInMove)
                ? (int)AnimMove.RightUp : (int)AnimStay.RightUp);
        }
        else if (dir == Vector3Int.right)
        {
            _animator.SetInteger(_name, (isInMove)
                ? (int)AnimMove.Right : (int)AnimStay.Right);
        }
        else if (dir == Vector3Int.right + Vector3Int.down)
        {
            _animator.SetInteger(_name, (isInMove)
                ? (int)AnimMove.RightDown : (int)AnimStay.RightDown);
        }
        else if (dir == Vector3Int.down)
        {
            _animator.SetInteger(_name, (isInMove)
                ? (int)AnimMove.Down : (int)AnimStay.Down);
        }
        else if (dir == Vector3Int.left + Vector3Int.down)
        {
            _animator.SetInteger(_name, (isInMove)
                ? (int)AnimMove.LeftDown : (int)AnimStay.LeftDown);
        }
        else if (dir == Vector3Int.left)
        {
            _animator.SetInteger(_name, (isInMove)
                ? (int)AnimMove.Left : (int)AnimStay.Left);
        }
        else if (dir == Vector3Int.left + Vector3Int.up)
        {
            _animator.SetInteger(_name, (isInMove)
                ? (int)AnimMove.LeftUp : (int)AnimStay.LeftUp);
        }
    }
    /// <summary>
    /// При финише происходит выключение gibi
    /// </summary>
    private void CheckFinish()
    {
        if (Grid.GetMark(transform.position) == 'e')
        {
            gameObject.SetActive(false);
        }
    }
    /// <summary>
    /// Перемещение к целевой позиции
    /// </summary>
    private void GoTowardDirection()
    {
        transform.position = Vector3.MoveTowards(transform.position, _targetPos, 10f * Time.deltaTime);
        if (Vector3.Distance(transform.position, _targetPos) < 0.001f)
        {
            SetAnimation(_targetPos - _currentPos); 
            transform.position = _targetPos;        
            CheckFinish();               
            Grid.SwapMark(_currentPos, _targetPos);
            _targetPos = Vector3Int.zero;
            _isMoveEnd = true;
            _pointer.gameObject.SetActive(false);
        }
    }
    /// <summary>
    /// Проверка на свободную клетку
    /// </summary>
    private void CheckMove(Vector3 dir)
    {
        Vector3Int newPos = Vector3Int.FloorToInt(transform.position + dir);
        if (Grid.IsPathFree(newPos, "e0123456789"))
        {
            _targetPos = newPos;
            _currentPos = Vector3Int.FloorToInt(transform.position);
            if(Grid.IsPathFree(newPos, "123456789"))
            {
                _audio.clip = AudioPickUp;
                _audio.Play();
            }
        }
    }
    /// <summary>
    /// Двигатель объекта
    /// </summary>
    public void Move(Vector3 dir)
    {
        _pointer.gameObject.SetActive(true);
        if (_targetPos == Vector3Int.zero)
        {
            CheckMove(dir);
        }
        SetAnimation(_targetPos - _currentPos, true);
        GoTowardDirection();
    }
}
