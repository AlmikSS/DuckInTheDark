using UnityEngine;
using VH.Tools;
using Zenject;

[RequireComponent(typeof(PlayerCombatSlots))]
public class PlayerCombat : MonoBehaviour, IPlayerCombat
{
    private PlayerCombatSlots _slots;
    private WeaponBase _currentWeapon;
    private Animator _animator;
    private EventBus _eventBus;

    [Inject]
    private void Construct(Animator animator, EventBus eventBus)
    {
        _animator = animator;
        _eventBus = eventBus;
    }

    private void Start()
    {
        _slots = GetComponent<PlayerCombatSlots>();
        _eventBus.Register<WeaponAddedToSlotEvent>(OnWeaponAddedInSlot);
    }

    private void OnWeaponAddedInSlot(WeaponAddedToSlotEvent e)
    {
        if (e.Slot == _slots.CurrentSlot)
            _currentWeapon = e.Weapon;
    }
    
    public void AttackBase()
    {
        if (_currentWeapon != null)
            _currentWeapon.Attack(_animator);
    }

    public void AttackSpec()
    {
        if (_currentWeapon != null)
            _currentWeapon.Attack(_animator, true);
    }

    public void UseAdditionalWeapon()
    {
    }

    public void ChangeWeapon(Vector2 scrollInput)
    {
        var currentSlot = _slots.CurrentSlot;
        
        if (scrollInput.y > 0)
            ChangeWeapon(currentSlot + 1);
        else if (scrollInput.y < 0)
            ChangeWeapon(currentSlot - 1);
    }
    
    public void ChangeWeapon(int index)
    {
        if (_slots.ChangeSlot(index))
            _currentWeapon = _slots.GetCurrentWeapon();
    }

    private void OnDestroy()
    {
        _eventBus.Unregister<WeaponAddedToSlotEvent>(OnWeaponAddedInSlot);
    }
}