using System.Collections;
using UnityEngine;

public class SlapperWeapon : WeaponBase
{
    protected override IEnumerator AttackRoutine(bool isRightMouseBtn)
    {
        Debug.Log("attack");
        yield return null;
        State = WeaponState.Idle;
    }

    protected override IEnumerator ComboRoutine(bool isRightMouseBtn)
    {
        Debug.Log("combo");
        yield return null;
        Reload();
    }
}