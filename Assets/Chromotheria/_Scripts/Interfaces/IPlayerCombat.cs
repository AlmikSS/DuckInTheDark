using UnityEngine;

public interface IPlayerCombat
{
    void Attack(bool isRightMouseBtn);
    void ChangeWeaponSlot(Vector2 mouseWealDelta);
    void ChangeWeaponSlot(int index);
}