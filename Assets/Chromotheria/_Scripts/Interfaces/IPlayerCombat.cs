using UnityEngine;

public interface IPlayerCombat
{
    void AttackBase();
    void AttackSpec();
    void UseAdditionalWeapon();
    void ChangeWeapon(Vector2 scrollInput);
    void ChangeWeapon(int index);
}