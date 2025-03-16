using System.Collections.Generic;
using UnityEngine;
using VH.Tools;
using Zenject;

public class PlayerCombatSlots : MonoBehaviour
{
    [SerializeField] private int _maxSlots;
    
    private List<WeaponBase> _weapons = new();
    private EventBus _eventBus;
    private int _currentSlot;
    
    public int CurrentSlot => _currentSlot;

    [Inject]
    private void Construct(EventBus eventBus)
    {
        _eventBus = eventBus;
    }

    private void Start()
    {
        for (var i = 0; i < _maxSlots; i++)
        {
            _weapons.Add(null);
        }
        
        _eventBus.Register<ItemAddedInInvEvent>(OnItemAddedInInv);
    }

    private void OnItemAddedInInv(ItemAddedInInvEvent e)
    {
        
    }
    
    public bool ChangeSlot(int slot)
    {
        if (slot < 0 || slot > _weapons.Count - 1) return false;

        _currentSlot = slot;
        return true;
    }

    public WeaponBase GetCurrentWeapon()
    {
        return _weapons[_currentSlot];
    }
}