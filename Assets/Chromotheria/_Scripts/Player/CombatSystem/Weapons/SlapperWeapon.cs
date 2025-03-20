using System.Collections;
using UnityEngine;

public class SlapperWeapon : WeaponBase
{
    protected override IEnumerator AttackRoutine(Animator animator, bool isSpecial = false)
    {
        _state = WeaponState.Attacking;
        
        animator.SetBool(AnimatorConstants.IsSpecBoolAnimName, isSpecial);
        animator.SetTrigger(AnimatorConstants.AttackTriggerAnimName);
        
        yield return new WaitUntil(() => animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0);
        yield return new WaitUntil(() => animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.85f);
        
        _comboTimer = Time.time;
        _state = WeaponState.Idle;
    }

    protected override IEnumerator AttackComboRoutine(Animator animator, bool isSpecial = false)
    {
        Debug.Log("Combo started!");
        _state = WeaponState.Attacking;
        
        animator.SetBool(AnimatorConstants.IsSpecBoolAnimName, isSpecial);
        animator.SetTrigger(AnimatorConstants.AttackTriggerAnimName);
        
        yield return new WaitUntil(() => animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0);
        yield return new WaitUntil(() => animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.85f);
        
        _reloadCoroutine = StartCoroutine(ReloadRoutine());
    }
}