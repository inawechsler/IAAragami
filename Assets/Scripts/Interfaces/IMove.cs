using UnityEngine;

public interface IMove
{
    Transform SelfPosition { get; set; }
    void Move(Vector3 dir);
    Vector3 CalculateMovementDirection();

}
