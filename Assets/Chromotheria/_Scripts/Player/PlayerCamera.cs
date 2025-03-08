using UnityEngine;

public class PlayerCamera : MonoBehaviour, IPlayerCamera
{
    [SerializeField] private float _sensitivity;
    [SerializeField, Range(0, 90)] private float _cameraXClamp;
    [SerializeField] private Transform _orientation;
    
    private float _xRotation;
    private float _yRotation;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void Look(Vector2 delta)
    {
        var x = delta.x;
        var y = delta.y;

        _xRotation -= y;
        _yRotation += x;
        
        _xRotation = Mathf.Clamp(_xRotation, -_cameraXClamp, _cameraXClamp);
        _orientation.rotation = Quaternion.Euler(0, _yRotation, 0f);
        transform.localRotation = Quaternion.Euler(_xRotation, _yRotation, 0f);
    }
}