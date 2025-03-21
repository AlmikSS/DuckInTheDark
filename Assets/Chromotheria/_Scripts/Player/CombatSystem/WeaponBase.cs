using System.Collections;
using UnityEngine;
using VH.Tools;
using Zenject;

public abstract class WeaponBase : MonoBehaviour
{
    [SerializeField] private float _reloadTime = 5f;
    [SerializeField] private float _cancelReloadWaitTime = 0.3f;
    [SerializeField] private float _comboWaitDelay = 0.2f; 
    [SerializeField] private int _comboCount = 3;
    
    private EventBus _eventBus;
    private bool _canCancelReload;
    private float _lastAttackTime;
    private int _currentCombo;
    
    public WeaponState State { get; protected set; }

    [Inject]
    private void Construct(EventBus eventBus)
    {
        _eventBus = eventBus;
        _eventBus.Register<CombatSlotChangedEvent>(OnSlotChanged);
        Debug.Log(0);
    }

    private void Start()
    {
        
    }

    private void OnSlotChanged(CombatSlotChangedEvent e)
    {
        Debug.Log("OnSlotChanged");
        
        if (_canCancelReload)
        {
            Debug.Log(938459384579438567);
            State = WeaponState.Idle;
        }
    }
    
    public void Attack(bool isRightMouseBtn)
    {
        if (State != WeaponState.Idle)
            return;

        State = WeaponState.Attacking;

        if (Time.time - _lastAttackTime > _comboWaitDelay)
            _currentCombo = 0;
        
        _currentCombo++;
        _lastAttackTime = Time.time;

        if (_currentCombo >= _comboCount)
        {
            _currentCombo = 0;
            StartCoroutine(ComboRoutine(isRightMouseBtn));
            return;
        }
        
        StartCoroutine(AttackRoutine(isRightMouseBtn));
    }

    protected abstract IEnumerator AttackRoutine(bool isRightMouseBtn);
    protected abstract IEnumerator ComboRoutine(bool isRightMouseBtn);

    protected void Reload()
    {
        _canCancelReload = true;
        Invoke(nameof(ResetCancelReload), _cancelReloadWaitTime);
        State = WeaponState.Reloading;
        Invoke(nameof(EndReloading), _reloadTime);
    }

    private void EndReloading()
    {
        _canCancelReload = false;
        State = WeaponState.Idle;
    }
    
    private void ResetCancelReload() => _canCancelReload = false;

    private void OnDestroy()
    {
        _eventBus.Unregister<CombatSlotChangedEvent>(OnSlotChanged);
    }
}

public enum WeaponState
{
    Idle,
    Attacking,
    Reloading,
}