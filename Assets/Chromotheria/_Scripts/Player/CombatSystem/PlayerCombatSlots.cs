using System.Collections.Generic;
using UnityEngine;
using VH.Tools;
using Zenject;

public class PlayerCombatSlots : MonoBehaviour
{
    [SerializeField] private Transform _rigthHandeTransform;
    [SerializeField] private Transform _leftHandeTransform;
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
        var item = e.ItemSO;

        if (item.Data.ItemType == ItemType.Weapon)
            AddWeaponToSlots(item.ItemPrefab);
    }
    
    private void AddWeaponToSlots(GameObject weaponPrefab)
    {
        if (!TryAddToCurrentSlot(weaponPrefab))
            TryAddWeaponToAnySlot(weaponPrefab);
    }

    private bool TryAddToCurrentSlot(GameObject prefab)
    {
        if (_weapons[_currentSlot] != null)
            return false;

        return AddWeaponToSlot(prefab, _currentSlot);
    }

    private bool TryAddWeaponToAnySlot(GameObject prefab)
    {
        for (var i = 0; i < _weapons.Count; i++)
        {
            if (_weapons[i] == null)
                return AddWeaponToSlot(prefab, i);
        }
        
        return false;
    }
    
    private bool AddWeaponToSlot(GameObject prefab, int slot)
    {
        if (!prefab.TryGetComponent(out WeaponBase _))
            return false;
        
        var obj = Instantiate(prefab, _rigthHandeTransform).GetComponent<WeaponBase>();
        _weapons[slot] = obj;
        _eventBus.Invoke(new WeaponAddedToSlotEvent(this, obj, slot));
        return true;
    }

    public bool ChangeSlot(int slot)
    {
        if (slot < 0 || slot > _weapons.Count - 1)
            return false;
        
        if (_weapons[_currentSlot] != null)
        {
            if (_weapons[_currentSlot].State == WeaponState.Attacking)
                return false;
            
            _weapons[_currentSlot].gameObject.SetActive(false);
        }
        
        _currentSlot = slot;
        if (_weapons[_currentSlot] != null)
            _weapons[_currentSlot].gameObject.SetActive(true);
        
        _eventBus.Invoke(new CombatSlotChangedEvent(this));
        return true;
    }
    
    public WeaponBase GetCurrentWeapon()
    {
        return _weapons[_currentSlot];
    }
}