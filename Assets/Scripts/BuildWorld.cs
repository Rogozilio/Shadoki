using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BuildWorld : MonoBehaviour
{
    private AI                      _ai;
    private UI                      _interface;
    private Draw                    _draw;
    private Tilemap                 _map;
    private SaveLoad                _saveLoad;
    private MoveManager             _moveManager;
    private AudioSource             _audio;
    private LinkedList<IUnit>       _units;
    private LinkedListNode<IUnit>   _unit;

    public byte                     _countFlower;
    public AudioClip                AudioMain;
    public AudioClip                AudioGoodEnd;
    public AudioClip                AudioBadEnd;
    private void Start()
    {
        _map         = GameObject.FindGameObjectWithTag("Tilemap").GetComponent<Tilemap>();
        _interface   = GameObject.FindGameObjectWithTag("UI").GetComponent<UI>();
        _audio       = GetComponent<AudioSource>();
        _draw        = new Draw(Instantiate, _map);
        _ai          = new AI();
        _saveLoad    = new SaveLoad();
        _units       = new LinkedList<IUnit>();
        _moveManager = new MoveManager();

        LoadGame();
        
        _draw.All(ref _units);

        _unit = _units.First;
        _moveManager.SetCommand(new MoveManagerOnCommand(_unit.Value));
    }
    private void Update()
    {
        ActiveMoveManager();
        CheckFlowers();
        DrawFinish();
        ChangeAudio();
    }
    /// <summary>
    /// Загрузка игры, если есть сохранение
    /// </summary>
    private void LoadGame()
    {
        if(!_saveLoad.LoadData(ref _interface))
        {
            Grid.SetMark('s');
            Grid.SetMark('f', _countFlower);
            Grid.SetMark('g', 5);
        }
    }
    /// <summary>
    /// Генерация цветов
    /// </summary>
    private void CheckFlowers()
    {
        if(_interface.CircuiteBar.fillAmount > 0.5)
        {
            _countFlower = 4;
        }
        if(Grid.CountMark('f') < _countFlower)
        {
            Vector2 pos = Grid.SetMarkGetPosition('f');
            _draw.Mark((byte)pos.x, (byte)pos.y);
        }
    }
    /// <summary>
    /// Пошаговая система
    /// </summary>
    private void ActiveMoveManager()
    {
        if(_unit.Value.IsMoveEnd == true)
        {
            _unit = (_unit.Next == null) ? _unit.List.First : _unit.Next;
            _moveManager.SetCommand(new MoveManagerOnCommand(_unit.Value));
        }
        else if(Grid.CountMark('s') != 0)
        {
            _moveManager.Go(_ai);
        }
    }
    /// <summary>
    /// Рисует финиш
    /// </summary>
    private void DrawFinish()
    {
        if(_interface.CircuiteBar.enabled 
            && _interface.CircuiteBar.fillAmount == 1)
        {
            _interface.CircuiteBar.enabled = false;
            Grid.Value[Grid.GameWidth - 2, Grid.GameHeight - 4] = 'e';
            _draw.Mark('e');
        }
    }
    /// <summary>
    /// Работа со звуком
    /// </summary>
    private void ChangeAudio()
    {
        if(Grid.CountMark('s') == 0 && _audio.clip == AudioMain)
        {
            _audio.clip = AudioGoodEnd;
            _audio.Play();
        }
        else if(Grid.CountMark('g') == 4 && _audio.clip == AudioMain)
        {
            _audio.clip = AudioBadEnd;
            _audio.Play();
        }
    }
}
