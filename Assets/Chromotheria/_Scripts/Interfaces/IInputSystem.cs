using UnityEngine;

public interface IInputSystem
{
    Vector2 GetVector2Input(InputKey key);
    float GetFloatInput(InputKey key);
    bool GetInputDown(InputKey key);
    bool GetInput(InputKey key);
    bool GetInputUp(InputKey key);
}

public enum InputKey
{
    Move,
    Look,
    Attack,
    Interact,
    Jump,
    Crouch,
    Sprint,
    Dash,
    Block,
    WeaponScroll,
    RightClick,
}