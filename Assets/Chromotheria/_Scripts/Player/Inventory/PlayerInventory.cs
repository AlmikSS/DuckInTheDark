using System.Collections.Generic;
using UnityEngine;
using VH.Tools;
using Zenject;

public class PlayerInventory : MonoBehaviour
{
    private Dictionary<ItemSO, int> _items = new();
    private EventBus _eventBus;

    [Inject]
    private void Construct(EventBus eventBus)
    {
        _eventBus = eventBus;
    }
    
    public void AddItem(ItemSO item, GameObject instance)
    {
        if (_items.ContainsKey(item))
        {
            if (_items[item] < item.Data.MaxStack)
                _items[item]++;
            else
                return;
        }
        else 
            _items.Add(item, 1);
        
        _eventBus.Invoke(new ItemAddedInInvEvent(this, item));
        Destroy(instance);
    }

    public void RemoveItem(ItemSO item)
    {
        
    }

    public bool HaveItem(ItemSO item, int amount)
    {
        if (_items.ContainsKey(item))
        {
            return _items[item] >= amount;
        }
        
        return false;
    }
}