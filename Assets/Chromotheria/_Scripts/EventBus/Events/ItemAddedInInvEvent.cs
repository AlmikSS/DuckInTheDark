using VH.Tools;

public class ItemAddedInInvEvent : Event
{
    public readonly Item Item;
    
    public ItemAddedInInvEvent(object sender, Item item) : base(sender)
    {
        Item = item;
    }
}