using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using VH.AI;

[RequireComponent(typeof(NavMeshAgent), typeof(AIVision))]
public class TestMob : MonoBehaviour, IDamageable, IPushable
{
    [SerializeField] private Transform _patrolCenter;
    [SerializeField] private float _patrolRadius;
    [SerializeField] private float _agrDistance;
    [SerializeField] private float _attackDistance;
    [SerializeField] private float _attackDelay;
    [SerializeField] private int _damage;
    [SerializeField] private int _maxHealth;

    private NavMeshAgent _agent;
    private AIVision _vision;
    private Transform _target;
    private Coroutine _currentCoroutine;
    private bool _canAttack;
    private State _currentState;
    private int _health;

    private enum State { Patrolling, Chasing }
    
    public int CurrentHealth => _health;
    public int MaxHealth => _maxHealth;

    private void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        _vision = GetComponent<AIVision>();
        _canAttack = true;
        _currentState = State.Patrolling;
        _currentCoroutine = StartCoroutine(PatrolRoutine());
        _health = _maxHealth;
    }

    private void Update()
    {
        CheckVision();
    }

    private IEnumerator PatrolRoutine()
    {
        while (true)
        {
            var randomPoint = GetRandomPointOnNavMesh();
            _agent.SetDestination(randomPoint);
            yield return null;
            yield return new WaitUntil(() => _agent.pathStatus == NavMeshPathStatus.PathComplete);
            yield return new WaitUntil(() => _agent.remainingDistance <= _agent.stoppingDistance);
            yield return new WaitForSeconds(Random.Range(0, 5f));
        }
    }

    private IEnumerator ChaseRoutine()
    {
        while (_target != null && IsTargetVisible(_target))
        {
            float distance = Vector3.Distance(_target.position, transform.position);

            if (distance > _agrDistance)
            {
                break;
            }

            _agent.SetDestination(_target.position);

            if (distance <= _attackDistance && _canAttack)
            {
                Attack();
            }

            yield return null;
        }

        _target = null;
        _currentState = State.Patrolling;
        _currentCoroutine = StartCoroutine(PatrolRoutine());
    }

    private void Attack()
    {
        if (_target == null || !_canAttack)
            return;

        Debug.Log("Attack");
        _canAttack = false;
        if (_target.TryGetComponent(out IDamageable damageable))
            damageable.TakeDamage(_damage, gameObject);
        Invoke(nameof(ResetAttack), _attackDelay);
    }

    private void ResetAttack() => _canAttack = true;

    private void CheckVision()
    {
        if (_currentState == State.Patrolling)
        {
            var visibleObjects = _vision.VisibleObjects;
            foreach (var visibleObject in visibleObjects)
            {
                if (visibleObject.gameObject.CompareTag("Player"))
                {
                    float distance = Vector3.Distance(transform.position, visibleObject.transform.position);
                    if (distance <= _agrDistance)
                    {
                        _target = visibleObject.transform;
                        if (_currentCoroutine != null)
                            StopCoroutine(_currentCoroutine);
                        _currentCoroutine = StartCoroutine(ChaseRoutine());
                        _currentState = State.Chasing;
                        return;
                    }
                }
            }
        }
    }

    private bool IsTargetVisible(Transform target)
    {
        return _vision.VisibleObjects.Contains(target.gameObject);
    }

    private Vector3 GetRandomPointOnNavMesh()
    {
        if (_patrolCenter == null)
        {
            Debug.LogError("Patrol center is not set.");
            return transform.position;
        }

        var randomDirection = Random.insideUnitCircle.normalized;
        var direction = new Vector3(randomDirection.x, 0, randomDirection.y);
        var randomDistance = Random.Range(0, _patrolRadius);
        var randomPoint = _patrolCenter.position + direction * randomDistance;

        if (NavMesh.SamplePosition(randomPoint, out var hit, _patrolRadius, NavMesh.AllAreas))
            return hit.position;
        else
        {
            Debug.LogWarning("Failed to find a point on NavMesh.");
            return transform.position;
        }
    }

    public void TakeDamage(int damage, GameObject attacker)
    {
        if (damage > 0)
            _health -= damage;

        if (_health <= 0)
            Die();
    }

    private void Die()
    {
        Destroy(gameObject);
    }

    public void Push(Vector3 direction, float force)
    {
        _agent.destination = default;
        _agent.velocity = direction * force;
    }

    public void ApplyExplosion(Vector3 origin, float force, float radius)
    {
        
    }
}