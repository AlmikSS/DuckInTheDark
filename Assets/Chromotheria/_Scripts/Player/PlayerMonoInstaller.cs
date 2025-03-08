using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

public class PlayerMonoInstaller : MonoInstaller
{
    [SerializeField] private RigidbodyPlayerMovement _playerMovement;
    [SerializeField] private InputActionAsset _mainActionAsset;
    [SerializeField] private PlayerCamera _playerCamera;
    
    public override void InstallBindings()
    {
        Container.BindInterfacesTo<RigidbodyPlayerMovement>().FromInstance(_playerMovement).AsSingle();
        Container.BindInterfacesTo<NewInputSystemSystem>().AsSingle().WithArguments(_mainActionAsset);
        Container.BindInterfacesTo<PlayerCamera>().FromInstance(_playerCamera).AsSingle();
    }
}