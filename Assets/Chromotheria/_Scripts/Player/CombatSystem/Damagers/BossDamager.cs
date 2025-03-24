using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossDamager : DamagerBase
{
    [SerializeField] private Transform _damageSphereOrigin;
    [SerializeField] private LayerMask _damagableLayerMask;
    [SerializeField] private float _damageSphereRadius = 1f;
    [SerializeField] private float _applyDamageDelay = 0.1f;
    [SerializeField] private float _pushForce = 10f;
    
    private bool _applyDamage;
    
    public override void StartApplyDamage(int damage, bool isCombo)
    {
        _applyDamage = true;
        StartCoroutine(ApplyDamageRoutine(damage, isCombo));
    }

    public override void StopApplyDamage()
    {
        _applyDamage = false;
    }

    private IEnumerator ApplyDamageRoutine(int damage, bool isCombo)
    {
        var attacked = new List<GameObject>();
        
        while (_applyDamage)
        {
            var cols = Physics.OverlapSphere(_damageSphereOrigin.position, _damageSphereRadius, _damagableLayerMask);
            
            foreach (var col in cols)
            {
                if (!attacked.Contains(col.gameObject))
                {
                    if (col.gameObject.TryGetComponent(out IDamageable damageable))
                    {
                        damageable.TakeDamage(damage, gameObject);
                        attacked.Add(col.gameObject);
                    }
                }
            }
            
            yield return new WaitForSeconds(_applyDamageDelay);
        }
        
        attacked.Clear();
    }
}