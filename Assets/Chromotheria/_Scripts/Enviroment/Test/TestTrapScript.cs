using UnityEngine;

public class TestTrapScript : MonoBehaviour
{
    [SerializeField] private int _damage;

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.TryGetComponent(out IDamageable damageable))
            damageable.TakeDamage(_damage, gameObject);
    }
}