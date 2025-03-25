using System.Collections;
using UnityEngine;

public class RaccoonCannon : WeaponBase
{
    [SerializeField] private GameObject _projectilePrefab;
    [SerializeField] private Transform _projectileSpawnPoint;
    
    protected override IEnumerator AttackRoutine(bool isRightMouseBtn, Animator animator)
    {
        var projectile = Instantiate(_projectilePrefab, _projectileSpawnPoint.position, Quaternion.identity).GetComponent<Projectile>();
        var direction = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        projectile.Launch(direction, _damage);
        yield return null;
        Reload();
    }

    protected override IEnumerator ComboRoutine(bool isRightMouseBtn, Animator animator)
    {
        yield return StartCoroutine(AttackRoutine(isRightMouseBtn, animator));
    }
}