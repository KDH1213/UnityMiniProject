using UnityEngine;

public interface IRouteMove
{
    public void Move();
    public Vector2 MovePoint { get; }
    public Vector2 CurrentPosition { get; }
    public Vector2 MoveDirection { get; }
    public int MoveIndex { get; }
}