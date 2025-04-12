using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class TestDamagableHer : MonoBehaviour, IDamageable, IPushable
{
    private Rigidbody _rb;

    public int CurrentHealth { get; }
    public int MaxHealth { get; }
    
    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _rb.freezeRotation = true;
    }

    public void TakeDamage(int damage, GameObject attacker)
    {
        Debug.Log($"Taking Damage: {damage}, from: {attacker}");
    }

    public void Push(Vector3 direction, float force)
    {
        _rb.AddForce(direction * force, ForceMode.Impulse);
        Invoke(nameof(ResetPush), 5f);
    }

    private void ResetPush()
    {
        _rb.linearVelocity = Vector3.zero;
    }

    public void ApplyExplosion(Vector3 origin, float force, float radius)
    {
        _rb.AddExplosionForce(force, origin, radius, force, ForceMode.Impulse);
        Invoke(nameof(ResetExplosion), 1f);
    }

    private void ResetExplosion()
    {
        _rb.linearVelocity = new Vector3(0, _rb.linearVelocity.y, 0);
    }
}