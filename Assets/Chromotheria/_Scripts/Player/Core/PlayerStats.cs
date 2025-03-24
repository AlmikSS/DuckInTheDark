using System.Collections;
using UnityEngine;
using VH.Tools;
using Zenject;

public class PlayerStats : MonoBehaviour, IDamageable
{
    [SerializeField] private int _maxHealth;
    [SerializeField] private int _regenerationRate;
    
    private EventBus _eventBus;
    private int _currentHealth;

    public int MaxHealth => _maxHealth;

    public int CurrentHealth => _currentHealth;

    [Inject]
    private void Construct(EventBus eventBus)
    {
        _eventBus = eventBus;
    }
    
    private void Start()
    {
        _currentHealth = _maxHealth;
        StartCoroutine(RegenerationRoutine());
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
            _currentHealth -= damage;

        if (_currentHealth <= 0)
            Die();
    }

    private void Die()
    {
        _eventBus.Invoke(new LoseEvent(this));
        //Destroy(gameObject);
    }
}