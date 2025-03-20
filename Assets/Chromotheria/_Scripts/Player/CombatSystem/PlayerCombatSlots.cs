using System.Collections.Generic;
using UnityEngine;
using VH.Tools;
using Zenject;

public class PlayerCombatSlots : MonoBehaviour
{
    [SerializeField] private int _maxSlots;
    [SerializeField] private Transform _rightHandle;
    [SerializeField] private Transform _leftHandle;
    
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
        var item = e.ItemSO;
        
        if (item.Data.ItemType == ItemType.Weapon)
        {
            if (item.ItemPrefab.TryGetComponent(out WeaponBase _))
                AddWeaponToFreeSlot(item.ItemPrefab);
        }
    }

    private void AddWeaponToFreeSlot(GameObject weapon)
    {
        if (!TryAddToCurrentSlot(weapon))
        {
            for (var i = 0; i < _weapons.Count; i++)
            {
                if (_weapons[i] == null)
                {
                    AddWeaponInSlot(weapon, i);
                    return;
                }
            }
        }
    }

    private bool TryAddToCurrentSlot(GameObject weapon)
    {
        if (_weapons[_currentSlot] != null)
            return false;
        
        AddWeaponInSlot(weapon, _currentSlot);
        
        return true;
    }
    
    private void AddWeaponInSlot(GameObject weapon, int slotId)
    {
        var obj = Instantiate(weapon, _rightHandle.position, _rightHandle.rotation, _rightHandle).GetComponent<WeaponBase>();
        _weapons[slotId] = obj;
        _eventBus.Invoke(new WeaponAddedToSlotEvent(this, obj, slotId));
    }
    
    public bool ChangeSlot(int slot)
    {
        if (slot < 0 || slot > _weapons.Count - 1) return false;
        
        if (_weapons[_currentSlot] != null)
        {
            if (_weapons[_currentSlot].State == WeaponState.Attacking)
                return false;
            
            if (_weapons[_currentSlot].State == WeaponState.Reloading)
                _weapons[_currentSlot].TryStopReload();
            
            _weapons[_currentSlot].gameObject.SetActive(false);
        }
        
        _currentSlot = slot;
        
        if (_weapons[_currentSlot] != null)
            _weapons[_currentSlot].gameObject.SetActive(true);
        
        return true;
    }

    public WeaponBase GetCurrentWeapon()
    {
        return _weapons[_currentSlot];
    }

    public void OnDestroy()
    {
        _eventBus.Unregister<ItemAddedInInvEvent>(OnItemAddedInInv);
    }
}