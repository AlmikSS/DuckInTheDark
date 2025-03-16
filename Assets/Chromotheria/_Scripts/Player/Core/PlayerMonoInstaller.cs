using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

public class PlayerMonoInstaller : MonoInstaller
{
    [SerializeField] private RigidbodyPlayerMovement _playerMovement;
    [SerializeField] private PlayerParkour _playerParkour;
    [SerializeField] private InputActionAsset _mainActionAsset;
    [SerializeField] private PlayerCamera _playerCamera;
    [SerializeField] private PlayerCombat _playerCombat;
    
    public override void InstallBindings()
    {
        Container.BindInterfacesTo<RigidbodyPlayerMovement>().FromInstance(_playerMovement).AsSingle();
        Container.BindInterfacesTo<NewInputSystem>().AsSingle().WithArguments(_mainActionAsset);
        Container.BindInterfacesTo<PlayerCamera>().FromInstance(_playerCamera).AsSingle();
        Container.BindInterfacesTo<PlayerParkour>().FromInstance(_playerParkour).AsSingle();
        Container.BindInterfacesTo<PlayerCombat>().FromInstance(_playerCombat).AsSingle();
    }
}