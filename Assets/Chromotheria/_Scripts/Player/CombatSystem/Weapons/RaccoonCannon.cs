using System.Collections;
using UnityEngine;

public class RaccoonCannon : WeaponBase
{
    [SerializeField] private GameObject _projectilePrefab;
    [SerializeField] private GameObject _projectileRicochetPrefab;
    [SerializeField] private Transform _projectileSpawnPoint;
    
    private Camera _camera;

    private void Start()
    {
        _camera = Camera.main;
    }
    
    protected override IEnumerator AttackRoutine(bool isRightMouseBtn, Animator animator)
    {
        var prefab = !isRightMouseBtn ? _projectilePrefab : _projectileRicochetPrefab;
        
        var projectile = Instantiate(prefab, _projectileSpawnPoint.position, _projectileSpawnPoint.rotation).GetComponent<Projectile>();
        
        var ray = _camera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        Vector3 targetPoint;
        
        if (Physics.Raycast(ray, out var hit))
            targetPoint = hit.point;
        else
            targetPoint = ray.origin + ray.direction * 100f;
        
        var direction = (targetPoint - projectile.transform.position).normalized;
        
        projectile.Launch(direction, _damage);
        yield return null;
        Reload();
    }

    protected override IEnumerator ComboRoutine(bool isRightMouseBtn, Animator animator)
    {
        yield return StartCoroutine(AttackRoutine(isRightMouseBtn, animator));
    }
}