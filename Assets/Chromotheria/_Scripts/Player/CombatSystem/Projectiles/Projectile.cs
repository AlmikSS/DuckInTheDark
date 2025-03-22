using UnityEngine;

public abstract class Projectile : MonoBehaviour
{
    public abstract void Launch(Vector3 direction, int damage);
}