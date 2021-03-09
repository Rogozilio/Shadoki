using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveManagerOnCommand : ICommand
{
    private IUnit _unit;
    public MoveManagerOnCommand(IUnit unit)
    {
        _unit = unit;
        _unit.IsMoveEnd = false;
    }
    public void Execute(AI ai)
    {
        _unit.Move(ai.GetDir(_unit.Transform.position));
    }
}
