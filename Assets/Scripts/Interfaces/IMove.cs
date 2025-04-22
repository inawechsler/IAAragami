using UnityEngine;

public interface IMove
{
    Transform Position { get; set; }
    void Move(Vector3 dir);
    Vector3 CalculateMovementDirection();

}
