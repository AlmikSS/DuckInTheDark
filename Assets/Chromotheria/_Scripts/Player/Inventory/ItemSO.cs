using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "VH/Inventory/Item")]
public class ItemSO : ScriptableObject
{
    [SerializeField] private ItemData _data;
    [SerializeField] private GameObject _itemPrefab;

    public ItemData Data => _data;

    public GameObject ItemPrefab => _itemPrefab;
}

[Serializable]
public class ItemData
{
    public int ItemId;
    public int MaxStack;
    public ItemType ItemType;
}