using UnityEngine;

public abstract class DamagerBase : MonoBehaviour
{
    public abstract void StartApplyDamage(int damage, bool isCombo);
    public abstract void StopApplyDamage();
}