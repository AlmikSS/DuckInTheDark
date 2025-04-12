using UnityEngine;

public interface IPlayerMovement
{
    MovementState MovementState { get; }
    Vector3 MovementDirection { get; }
    void Move(Vector2 inputDirection);
    void Jump();
    void Dash();
    void Sprint(bool isSprinting);
    void Stop();
}

public enum MovementState
{
    Idle,
    Walking,
    Running,
    Landing,
    Dashing,
    Climbing,
}