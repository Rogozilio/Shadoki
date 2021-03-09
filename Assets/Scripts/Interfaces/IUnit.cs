using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IUnit
{
    public Transform Transform { get;}
    public bool IsMoveEnd { get; set; }
    public void Move(Vector3 newPos);
}
