using UnityEngine;
using UnityEngine.InputSystem;

public class NewInputSystemSystem : IInputSystem
{
    private InputActionAsset _actionAsset;

    public NewInputSystemSystem(InputActionAsset actionAsset)
    {
        _actionAsset = actionAsset;
    }

    public Vector2 GetVector2Input(InputKey key)
    {
        return _actionAsset[key.ToString()].ReadValue<Vector2>();
    }

    public float GetFloatInput(InputKey key)
    {
        return _actionAsset[key.ToString()].ReadValue<float>();
    }

    public bool GetInputDown(InputKey key)
    {
        return _actionAsset[key.ToString()].triggered;
    }

    public bool GetInput(InputKey key)
    {
        return _actionAsset[key.ToString()].IsPressed();
    }

    public bool GetInputUp(InputKey key)
    {
        return _actionAsset[key.ToString()].WasReleasedThisFrame();
    }
}