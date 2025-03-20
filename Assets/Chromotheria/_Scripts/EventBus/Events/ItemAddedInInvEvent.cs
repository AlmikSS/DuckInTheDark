using VH.Tools;

public class ItemAddedInInvEvent : Event
{
    public readonly ItemSO ItemSO;
    
    public ItemAddedInInvEvent(object sender, ItemSO itemSo) : base(sender)
    {
        ItemSO = itemSo;
    }
}