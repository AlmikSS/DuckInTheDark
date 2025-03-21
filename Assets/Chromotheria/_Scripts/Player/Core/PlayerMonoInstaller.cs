using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

public class PlayerMonoInstaller : MonoInstaller
{
    [SerializeField] private RigidbodyPlayerMovement _playerMovement;
    [SerializeField] private PlayerParkour _playerParkour;
    [SerializeField] private InputActionAsset _mainActionAsset;
    [SerializeField] private PlayerCamera _playerCamera;
    [SerializeField] private PlayerInventory _playerInventory;
    [SerializeField] private PlayerInteractions _playerInteractions;
    [SerializeField] private PlayerCombat _playerCombat;
    [SerializeField] private Camera _camera;
    [SerializeField] private Animator _animator;
    
    public override void InstallBindings()
    {
        Container.BindInterfacesTo<RigidbodyPlayerMovement>().FromInstance(_playerMovement).AsSingle();
        Container.BindInterfacesTo<NewInputSystem>().AsSingle().WithArguments(_mainActionAsset);
        Container.BindInterfacesTo<PlayerCamera>().FromInstance(_playerCamera).AsSingle();
        Container.BindInterfacesTo<PlayerParkour>().FromInstance(_playerParkour).AsSingle();
        Container.BindInterfacesTo<PlayerInteractions>().FromInstance(_playerInteractions).AsSingle();
        Container.BindInterfacesTo<PlayerCombat>().FromInstance(_playerCombat).AsSingle();
        Container.Bind<PlayerInventory>().FromInstance(_playerInventory).AsSingle();
        Container.Bind<Camera>().FromInstance(_camera).AsSingle();
        Container.Bind<Animator>().FromInstance(_animator).AsSingle();
    }
}