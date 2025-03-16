using UnityEngine;

[RequireComponent(typeof(PlayerCombatSlots))]
public class PlayerCombat : MonoBehaviour, IPlayerCombat
{
    private PlayerCombatSlots _slots;
    private WeaponBase _currentWeapon;

    private void Start()
    {
        _slots = GetComponent<PlayerCombatSlots>();
    }
    
    public void AttackBase()
    {
        StartCoroutine(_currentWeapon.AttackBaseRoutine());
    }

    public void AttackSpec()
    {
        StartCoroutine(_currentWeapon.AttackSpecRoutine());
    }

    public void Defend(bool isDefending)
    {
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
}