using UnityEngine;
using Zenject;

[SelectionBase]
public class PlayerController : MonoBehaviour
{
    private IPlayerMovement _playerMovement;
    private IInputSystem _inputSystem;
    private IPlayerCamera _playerCamera;

    private Vector2 _moveInputDirection;
    private Vector2 _lookInputDirection;
    
    [Inject]
    private void Construct(IPlayerMovement playerMovement, IInputSystem inputSystem, IPlayerCamera playerCamera)
    {
        _playerMovement = playerMovement;
        _inputSystem = inputSystem;
        _playerCamera = playerCamera;
    }

    private void Update()
    {
        HandleInput();
    }

    private void FixedUpdate()
    {
        _playerMovement.Move(_moveInputDirection);
    }

    private void LateUpdate()
    {
        _playerCamera.Look(_lookInputDirection);
    }
    
    private void HandleInput()
    {
        HandleVector2Inputs();
        HandleMovementInput();
    }

    private void HandleMovementInput()
    {
        HandleSprintInput();
        
        if (_inputSystem.GetInputDown(InputKey.Jump))
            _playerMovement.Jump();
        if (_inputSystem.GetInputDown(InputKey.Dash))
            _playerMovement.Dash();
    }

    private void HandleSprintInput()
    {
        if (_inputSystem.GetInputDown(InputKey.Sprint))
            _playerMovement.Sprint(true);
        else if (_inputSystem.GetInputUp(InputKey.Sprint))
            _playerMovement.Sprint(false);
    }
    
    private void HandleVector2Inputs()
    {
        _moveInputDirection = _inputSystem.GetVector2Input(InputKey.Move);
        _lookInputDirection = _inputSystem.GetVector2Input(InputKey.Look);
    }
}