using System.Collections;
using UnityEngine;

public abstract class WeaponBase : MonoBehaviour
{
    [SerializeField] private float _reloadTime = 3f;
    [SerializeField] private float _comboWaitTime = 0.3f;
    [SerializeField] protected int _damage = 7;
    [SerializeField] private int _attackBaseCombo;
    [SerializeField] private int _attackSpecCombo;

    protected Coroutine _reloadCoroutine;
    protected WeaponState _state;
    protected float _comboTimer = 0f;
    private float _lastReloadTime = 0f;
    private int _currentCombo = 0;

    public WeaponState State => _state;

    public void Attack( Animator animator, bool isSpecial = false)
    {
        if (_state == WeaponState.Idle)
        {
            if (Time.time - _comboTimer > _comboWaitTime)
                _currentCombo = 0;

            _currentCombo++;

            var maxCombo = isSpecial ? _attackBaseCombo : _attackSpecCombo;

            if (_currentCombo >= maxCombo)
            {
                StartCoroutine(AttackComboRoutine(animator, isSpecial));
                _currentCombo = 0;
                return;
            }

            StartCoroutine(AttackRoutine(animator, isSpecial));
        }
    }

    public bool TryStopReload()
    {
        if (_reloadCoroutine != null && Time.time - _lastReloadTime <= _comboWaitTime)
        {
            StopCoroutine(_reloadCoroutine);
            _reloadCoroutine = null;
            _state = WeaponState.Idle;
            return true;
        }
        
        return false;
    }

    protected IEnumerator ReloadRoutine()
    {
        _state = WeaponState.Reloading;
        _lastReloadTime = Time.time;
        yield return new WaitForSeconds(_reloadTime);
        _state = WeaponState.Idle;
        _reloadCoroutine = null;
    }

    protected abstract IEnumerator AttackRoutine(Animator animator, bool isSpecial = false);
    
    protected abstract IEnumerator AttackComboRoutine(Animator animator, bool isSpecial = false);
}

public enum WeaponState
{
    Idle,
    Attacking,
    Reloading,
}