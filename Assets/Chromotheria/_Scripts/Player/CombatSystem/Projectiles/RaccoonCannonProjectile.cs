using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class RaccoonCannonProjectile : Projectile
{
    [SerializeField] private float _force;
    [SerializeField] private float _explosionRadius;
    [SerializeField] private float _explosionForce;
    [SerializeField] private LayerMask _damagableLayerMask;
    
    private Rigidbody _rb;
    private int _damage;
    
    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }
    
    public override void Launch(Vector3 direction, int damage)
    {
        _rb.linearVelocity = direction * _force;
        _damage = damage;
        Destroy(gameObject, 20f);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
            return;
        
        var cols = Physics.OverlapSphere(transform.position, _explosionRadius, _damagableLayerMask);

        foreach (var col in cols)
        {
            if (col.gameObject.TryGetComponent(out IDamageable damageable))
                damageable.TakeDamage(_damage, gameObject);
            
            if (col.gameObject.TryGetComponent(out IPushable pushable))
                pushable.ApplyExplosion(transform.position, _explosionForce, _explosionRadius);
        }
        
        Destroy(gameObject);
    }
}