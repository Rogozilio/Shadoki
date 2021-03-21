using UnityEngine;

public interface IUnit
{
    public Transform Transform { get;}
    public bool IsMoveEnd { get; set; }
    public void Move(Vector3 newPos);
}
