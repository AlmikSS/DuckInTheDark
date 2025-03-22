
using UnityEngine;

public interface IPushable
{
    void Push(Vector3 direction, float force);
    void ApplyExplosion(Vector3 origin, float force, float radius);
}