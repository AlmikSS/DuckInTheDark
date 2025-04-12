using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class RaccoonCannonProjectile : Projectile
{
    [SerializeField] private GameObject _effectPrefab;
    [SerializeField] private float _force;
    [SerializeField] private float _explosionRadius;
    [SerializeField] private float _explosionForce;
    [SerializeField] private LayerMask _damagableLayerMask;
    [SerializeField] private int _ricochetCountMax;
    
    private Rigidbody _rb;
    private Vector3 _direction;
    private int _damage;
    private int _ricochetCount;
    
    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }
    
    public override void Launch(Vector3 direction, int damage)
    {
        _rb.linearVelocity = direction * _force;
        _damage = damage;
        _direction = direction;
        Destroy(gameObject, 20f);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
            return;
        
        _ricochetCount++;

        if (((1 << collision.gameObject.layer) & _damagableLayerMask) == 0)
        {
            if (_ricochetCount < _ricochetCountMax)
            {
                var normal = collision.contacts[0].normal;
                _direction = Vector3.Reflect(_direction, normal.normalized);
                _rb.linearVelocity = _direction * _force;
                return;
            }
        }
        
        var fx = Instantiate(_effectPrefab, transform.position, Quaternion.identity);
        Destroy(fx, 3f);
        
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