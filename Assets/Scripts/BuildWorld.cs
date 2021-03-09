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
    private UI _interface;
    private AI _ai;
    public byte _countFlower;
    private void Start()
    {
        _map = GameObject.FindGameObjectWithTag("Tilemap").GetComponent<Tilemap>();
        _interface = GameObject.FindGameObjectWithTag("UI").GetComponent<UI>();
        _draw = new Draw(Instantiate, _map);
        _ai = new AI();
        _units = new LinkedList<IUnit>();
        _moveManager = new MoveManager();

        Grid.SetMark('s');
        Grid.SetMark('f', _countFlower);
        Grid.SetMark('g');

        _draw.All(ref _units);

        _unit = _units.First;
        _moveManager.SetCommand(new MoveManagerOnCommand(_unit.Value));
    }
    private void Update()
    {
        ActiveMoveManager();
        CheckFlowers();
        DrawFinish();
    }
    private void CheckFlowers()
    {
        if(Grid.CountMark('f') != _countFlower)
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
        else
        {
            _moveManager.Go(_ai);
        }
    }
    private void DrawFinish()
    {
        if(_interface.CircuiteBar.enabled && _interface.CircuiteBar.fillAmount == 1)
        {
            _interface.CircuiteBar.enabled = false;
            Grid.Value[GlobalData.gameWidth - 2, GlobalData.gameHeight - 4] = 'e';
            _draw.Mark('e');
        }
    }
}
