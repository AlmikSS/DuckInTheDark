using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using VH.AI;

[RequireComponent(typeof(NavMeshAgent), typeof(AIVision))]
public class TestBoss : MonoBehaviour, IDamageable
{
    [SerializeField] private DamagerBase _rightDamager;
    [SerializeField] private DamagerBase _leftDamager;
    [SerializeField] private Animator _animator;
    [SerializeField] private float _attackDistance;
    [SerializeField, Range(0, 1f)] private float _rangeChanse;
    [SerializeField] private float _rangeTimeMin;
    [SerializeField] private float _rangeTimeMax;
    [SerializeField] private float _attackRange;
    [SerializeField] private int _damageMin;
    [SerializeField] private int _damageMax;
    [SerializeField] private int _maxHealth;
     
    private NavMeshAgent _agent;
    private AIVision _vision;
    private Transform _target;
    private Coroutine _rangeCheckCoroutine;
    private bool _isAgr;
    private bool _isAttacking;
    private int _currentHealth;

    public int CurrentHealth => _currentHealth;
    public int MaxHealth => _maxHealth;
    
    private void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        _vision = GetComponent<AIVision>();
        _currentHealth = _maxHealth;
    }

    private void Update()
    {
        CheckVision();
        
        if (_isAgr)
        {
            AgrState();
            if (_rangeCheckCoroutine == null)
                _rangeCheckCoroutine = StartCoroutine(RangeCheckRoutine());
        }
        
        _animator.SetFloat("Velocity", Mathf.Abs(_agent.velocity.magnitude));
    }

    private void AgrState()
    {
        if (!_isAttacking)
            _agent.SetDestination(_target.position);
        
        var distance = Vector3.Distance(_target.position, transform.position);
        if (distance < _attackDistance)
            StartCoroutine(AttackRoutine());
    }

    private IEnumerator AttackRoutine()
    {
        if (_isAttacking)
            yield break;
        
        _agent.isStopped = true;
        _isAttacking = true;
        var i = Random.Range(0, 5);
        _animator.Play($"Attack{i}");
        yield return null;
        _rightDamager.StartApplyDamage(Random.Range(_damageMin, _damageMax), false);
        _leftDamager.StartApplyDamage(Random.Range(_damageMin, _damageMax), false);
        yield return new WaitUntil(() => _animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.9f);
        _rightDamager.StopApplyDamage();
        _leftDamager.StopApplyDamage();
        _agent.isStopped = false;
        yield return new WaitForSeconds(_attackRange);
        _isAttacking = false;
    }

    private IEnumerator RangeRoutine()
    {
        _agent.isStopped = true;
        _animator.Play("Rage");
        yield return null;
        yield return new WaitUntil(() => _animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.9f);
        _agent.isStopped = false;
        if (_isAgr)
            _agent.SetDestination(_target.position);
    }
    
    private IEnumerator RangeCheckRoutine()
    {
        while (_isAgr)
        {
            var waitTime = Random.Range(_rangeTimeMin, _rangeTimeMax);
            yield return new WaitForSeconds(waitTime);
        
            if (Random.value < _rangeChanse)
                StartCoroutine(RangeRoutine());
        }
        _rangeCheckCoroutine = null;
    }
    
    private void CheckVision()
    {
        var visibleObjects = _vision.VisibleObjects;

        foreach (var visibleObject in visibleObjects)
        {
            if (visibleObject.CompareTag("Player"))
            {
                _target = visibleObject.transform;
                _isAgr = true;
                return;
            }
        }
    }

    public void TakeDamage(int damage, GameObject attacker)
    {
        if (damage > 0)
            _currentHealth -= damage;

        if (_currentHealth <= 0)
            Die();
    }

    private void Die()
    {
        _agent.isStopped = true;
        _isAgr = false;
        _isAttacking = false;
        _animator.SetTrigger("Die");
        Destroy(gameObject, 3f);
    }
}