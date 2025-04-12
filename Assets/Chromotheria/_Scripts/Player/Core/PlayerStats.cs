using System.Collections;
using UnityEngine;
using VH.Tools;
using Zenject;

public class PlayerStats : MonoBehaviour, IDamageable
{
    [SerializeField] private int _maxHealth;
    [SerializeField] private float _regenerationRate;
    
    private EventBus _eventBus;
    private Animator _animator;
    private int _currentHealth;
    private static int _loses;
    private static int _wins;

    public int MaxHealth => _maxHealth;
    public int CurrentHealth => _currentHealth;
    public int Loses => _loses;
    public int Wins => _wins;
    
    [Inject]
    private void Construct(EventBus eventBus, Animator animator)
    {
        _eventBus = eventBus;
        _animator = animator;
        
        _eventBus.Register<LoseEvent>(OnLose);
        _eventBus.Register<WinEvent>(OnWin);
    }
    
    private void Start()
    {
        _currentHealth = _maxHealth;
        StartCoroutine(RegenerationRoutine());
    }

    private void OnLose(LoseEvent loseEvent)
    {
        _loses++;
    }

    private void OnWin(WinEvent winEvent)
    {
        _wins++;
    }
    
    private IEnumerator RegenerationRoutine()
    {
        while (true)
        {
            if (_currentHealth < _maxHealth)
                _currentHealth++;
            yield return new WaitForSeconds(_regenerationRate);
        }
    }
    
    public void TakeDamage(int damage, GameObject attacker)
    {
        if (damage > 0)
        {
            _currentHealth -= damage;
            _animator.SetTrigger("Damaged");
        }

        if (_currentHealth <= 0)
            Die();
    }

    private void Die()
    {
        _eventBus.Invoke(new LoseEvent(this));
        //Destroy(gameObject);
    }

    private void OnDestroy()
    {
        _eventBus.Unregister<LoseEvent>(OnLose);
        _eventBus.Unregister<WinEvent>(OnWin);
    }
}