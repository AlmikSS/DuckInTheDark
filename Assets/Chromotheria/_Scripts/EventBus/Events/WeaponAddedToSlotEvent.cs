using VH.Tools;

public class WeaponAddedToSlotEvent : Event
{
    public readonly WeaponBase Weapon;
    public readonly int Slot;
    
    public WeaponAddedToSlotEvent(object sender, WeaponBase weapon, int slot) : base(sender)
    {
        Weapon = weapon;
        Slot = slot;
    }
}