using System.Collections;
using UnityEngine;

public class SlapperWeapon : WeaponBase
{
    [SerializeField] private DamagerBase _damager;
    
    protected override IEnumerator AttackRoutine(bool isRightMouseBtn, Animator animator)
    {
        animator.SetTrigger(AnimatorConstants.AttackBoolAnimName);
        animator.SetBool(AnimatorConstants.IsSpecBoolAnimName, isRightMouseBtn);
        yield return null;
        _damager.StartApplyDamage(_damage, false);
        yield return new WaitUntil(() => animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.9f);
        //animator.SetBool(AnimatorConstants.AttackBoolAnimName, false);
        State = WeaponState.Idle;
        _currentCombo++;
        _lastAttackTime = Time.time;
        _damager.StopApplyDamage();
    }

    protected override IEnumerator ComboRoutine(bool isRightMouseBtn, Animator animator)
    {
        animator.SetTrigger(AnimatorConstants.AttackBoolAnimName);
        animator.SetBool(AnimatorConstants.IsSpecBoolAnimName, isRightMouseBtn);
        yield return null;
        _damager.StartApplyDamage(_damage, true);
        yield return new WaitUntil(() => animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.9f);
        //animator.SetBool(AnimatorConstants.ComboBoolAnimName, false);
        _damager.StopApplyDamage();
        Reload(); 
    }
}