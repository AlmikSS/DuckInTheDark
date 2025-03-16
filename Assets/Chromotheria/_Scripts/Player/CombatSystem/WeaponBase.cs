using System.Collections;
using UnityEngine;

public abstract class WeaponBase : MonoBehaviour
{
    [SerializeField] private float _reloadTime = 3f;
    [SerializeField] private int _damage = 7;
    [SerializeField] private int _attackBaseCombo;
    [SerializeField] private int _attackSpecCombo;
    
    private WeaponState _weaponState;
    private float _reloadTimer = 0f;
    
    public WeaponState WeaponState => _weaponState;
    
    public abstract IEnumerator AttackBaseRoutine();
    public abstract IEnumerator AttackSpecRoutine();
    public abstract IEnumerator ReloadRoutine();
    
    protected abstract IEnumerator AttackBaseComboRoutine();
    protected abstract IEnumerator AttackSpecComboRoutine();
}

public enum WeaponState
{
    Active,
    Inactive,
    Reloading,
}