using UnityEngine;

public class Item : MonoBehaviour, IInteractable
{
    [SerializeField] private ItemSO _itemSO;

    public ItemSO ItemSo => _itemSO;
    
    public void Interact(GameObject sender)
    {
        if (sender.TryGetComponent(out PlayerInventory inventory))
            inventory.AddItem(_itemSO, gameObject);
    }
}

public enum ItemType
{
    Item,
    Weapon,
}