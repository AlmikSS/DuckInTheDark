using UnityEngine;
using VH.Tools;
using Zenject;

[RequireComponent(typeof(PlayerCombatSlots))]
public class PlayerCombat : MonoBehaviour, IPlayerCombat
{
    private WeaponBase _currentWeapon;
    private PlayerCombatSlots _playerCombatSlots;
    private EventBus _eventBus;
    private Animator _animator;

    [Inject]
    private void Construct(EventBus eventBus, Animator animator)
    {
        _eventBus = eventBus;
        _animator = animator;
    }
    
    private void Start()
    {
        _playerCombatSlots = GetComponent<PlayerCombatSlots>();
        _eventBus.Register<WeaponAddedToSlotEvent>(OnWeaponAddedToSlot);
    }

    private void OnWeaponAddedToSlot(WeaponAddedToSlotEvent e)
    {
        if (e.Slot == _playerCombatSlots.CurrentSlot)
            _currentWeapon = e.Weapon;
    }
    
    public void Attack(bool isRightMouseBtn)
    {
        if (_currentWeapon != null)
            _currentWeapon.Attack(isRightMouseBtn, _animator);
    }

    public void ChangeWeaponSlot(Vector2 mouseWealDelta)
    {
        var currentSlot = _playerCombatSlots.CurrentSlot;
        
        if (mouseWealDelta.y > 0)
            ChangeWeaponSlot(currentSlot + 1);
        else if (mouseWealDelta.y < 0)
            ChangeWeaponSlot(currentSlot - 1);
    }

    public void ChangeWeaponSlot(int index)
    {
        if (_playerCombatSlots.ChangeSlot(index))
            _currentWeapon = _playerCombatSlots.GetCurrentWeapon();
    }

    private void OnDestroy()
    {
        _eventBus.Unregister<WeaponAddedToSlotEvent>(OnWeaponAddedToSlot);
    }
}