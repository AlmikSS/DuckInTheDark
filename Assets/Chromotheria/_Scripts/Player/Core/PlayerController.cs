using UnityEngine;
using Zenject;

[SelectionBase]
public class PlayerController : MonoBehaviour
{
    private IPlayerMovement _playerMovement;
    private IPlayerCamera _playerCamera;
    private IInputSystem _inputSystem;
    private IPlayerCombat _playerCombat;
    private IPlayerInteractions _playerInteractions;

    private Vector2 _moveInputDirection;
    private Vector2 _lookInputDirection;
    private Vector2 _weaponScroll;
    
    [Inject]
    private void Construct(IPlayerMovement playerMovement, IInputSystem inputSystem, IPlayerCamera playerCamera, IPlayerCombat playerCombat, IPlayerInteractions playerInteractions)
    {
        _playerMovement = playerMovement;
        _playerCamera = playerCamera;
        _inputSystem = inputSystem;
        _playerCombat = playerCombat;
        _playerInteractions = playerInteractions;
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
        HandleCombatInput();
        
        if (_inputSystem.GetInputDown(InputKey.Interact))
            _playerInteractions.TryInteract();
    }

    private void HandleCombatInput()
    {
        _playerCombat.ChangeWeapon(_weaponScroll);
        
        if (_inputSystem.GetInputDown(InputKey.Attack))
            _playerCombat.AttackBase();
        if (_inputSystem.GetInputDown(InputKey.RightClick))
            _playerCombat.AttackSpec();
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
        _weaponScroll = _inputSystem.GetVector2Input(InputKey.WeaponScroll);
    }
}