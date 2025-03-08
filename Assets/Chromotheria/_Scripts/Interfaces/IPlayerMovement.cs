using UnityEngine;

public interface IPlayerMovement
{
    void Move(Vector2 inputDirection);
    void Jump();
    void Dash();
    void Stop();
    float Velocity { get; }
    bool Grounded { get; }
}