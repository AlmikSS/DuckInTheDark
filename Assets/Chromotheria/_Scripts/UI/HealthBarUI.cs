using UnityEngine;
using UnityEngine.UI;

public class HealthBarUI : MonoBehaviour
{
    [SerializeField] private Image _healthBarImage;

    private IDamageable _damageable;
    private bool _canWork;
    
    private void Start()
    {
        if (TryGetComponent(out _damageable))
            _canWork = true;
    }

    private void Update()
    {
        if (!_canWork) return;
        
        var maxHealth = _damageable.MaxHealth;
        var health = _damageable.CurrentHealth;
        
        _healthBarImage.fillAmount = health / (float)maxHealth;
    }
}