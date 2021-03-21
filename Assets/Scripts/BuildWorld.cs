using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BuildWorld : MonoBehaviour
{
    private Tilemap _map;
    private Draw _draw;
    private LinkedList<IUnit> _units;
    private LinkedListNode<IUnit> _unit;
    private MoveManager _moveManager;
    private SaveLoad _saveLoad;
    private UI _interface;
    private AI _ai;
    private AudioSource _audio;
    public byte _countFlower;
    public AudioClip AudioMain;
    public AudioClip AudioGoodEnd;
    public AudioClip AudioBadEnd;
    private void Start()
    {
        _map = GameObject.FindGameObjectWithTag("Tilemap").GetComponent<Tilemap>();
        _interface = GameObject.FindGameObjectWithTag("UI").GetComponent<UI>();
        _audio = GetComponent<AudioSource>();
        _draw = new Draw(Instantiate, _map);
        _ai = new AI();
        _saveLoad = new SaveLoad();
        _units = new LinkedList<IUnit>();
        _moveManager = new MoveManager();

        if(!_saveLoad.LoadData(ref _interface))
        {
            Grid.SetMark('s');
            Grid.SetMark('f', _countFlower);
            Grid.SetMark('g', 5);
        }
        
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
    private void CheckFlowers()
    {
        if(_interface.CircuiteBar.fillAmount > 0.5)
        {
            _countFlower = 4;
        }
        if(Grid.CountMark('f') < _countFlower)
        {
            Vector2 pos = Grid.SetMarkGetPosition('f');
            _draw.Mark((byte)pos.x, (byte)pos.y,'f');
        }
    }
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
