using UnityEngine;
using Zenject;

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
        HandleVector2Inputs();
        
        if (_inputSystem.GetInputDown(InputKey.Jump))
            _playerMovement.Jump();
    }

    private void HandleVector2Inputs()
    {
        _moveInputDirection = _inputSystem.GetVector2Input(InputKey.Move);
        _lookInputDirection = _inputSystem.GetVector2Input(InputKey.Look);
    }

    private void FixedUpdate()
    {
        _playerMovement.Move(_moveInputDirection);
    }

    private void LateUpdate()
    {
        _playerCamera.Look(_lookInputDirection);
    }
}