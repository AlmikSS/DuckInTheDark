using UnityEngine;

public class Rotator : MonoBehaviour
{
    [Header("What")]
    [SerializeField] private Transform _origin;
    [SerializeField] private Transform _target;

    [Header("How")]
    [SerializeField] private bool _x = true;
    [SerializeField] private bool _y = true;
    [SerializeField] private bool _z = true;
    
    [Header("When")]
    [SerializeField] private bool _lateUpdate;
    [SerializeField] private bool _update = true;
    [SerializeField] private bool _fixedUpdate;

    private bool _canWork;

    private void Start()
    {
        _canWork = _origin != null;
    }
    
    private void LateUpdate()
    {
        if (_lateUpdate)
            Rotate();
    }

    private void Update()
    {
        if (_update)
            Rotate();
    }

    private void FixedUpdate()
    {
        if (_fixedUpdate)
            Rotate();
    }

    private void Rotate()
    {
        if (!_canWork)
            return;
        
        var target = _target != null ? _target : transform;
        
        if (_x)
            target.rotation = Quaternion.Euler(_origin.eulerAngles.x, target.eulerAngles.y, target.eulerAngles.z);
        
        if (_y)
            target.rotation = Quaternion.Euler(target.eulerAngles.x, _origin.eulerAngles.y, target.eulerAngles.z);

        if (_z)
            target.rotation = Quaternion.Euler(target.eulerAngles.x, target.eulerAngles.y, _origin.eulerAngles.z);
    }
}