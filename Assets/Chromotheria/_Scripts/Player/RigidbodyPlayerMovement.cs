using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class RigidbodyPlayerMovement : MonoBehaviour, IPlayerMovement
{
    [Header("Movement")]
    [SerializeField] private Transform _orientation;
    [SerializeField] private float _walkSpeed = 7f;
    [SerializeField] private float _sprintSpeed = 10f;
    [SerializeField] private float _slopeAngle = 45f;
    [SerializeField] private float _slopeCheckDistance = 0.4f;

    [Header("Jump")]
    [SerializeField] private float _jumpForce = 5f;
    [SerializeField] private float _jumpCulDown = 0.2f;
    
    [Header("Physics")]
    [SerializeField] private float _gravityForce = 9.81f;
    [SerializeField] private Vector3 _gravityDirection = Vector3.down;
    
    [Header("Ground Check")]
    [SerializeField] private Transform _groundCheckOrigin;
    [SerializeField] private LayerMask _groundLayerMask;
    [SerializeField] private float _groundCheckRadius;
    
    [Header("Gizmos")]
    [SerializeField] private bool _drawGroundCheckGizmos;
    [SerializeField] private Color _groundCheckGizmoColor = Color.blue;
    [SerializeField] private bool _drawMoveDirectionGizmos;
    [SerializeField] private Color _moveDirectionGizmoColor = Color.green;
    [SerializeField] private bool _drawSlopeAngleGizmos;
    [SerializeField] private Color _slopeCheckGizmoColor = Color.yellow;
    
    private Rigidbody _rb;
    private Vector3 _movementDirection;
    private bool _grounded;
    private bool _readyToJump;
    private float _moveSpeed;

    public float Velocity => _movementDirection.magnitude * _moveSpeed;
    public bool Grounded => _grounded;
    
    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _rb.freezeRotation = true;
        _rb.useGravity = false;
        _moveSpeed = _walkSpeed;
        _readyToJump = true;
    }

    private void Update()
    {
        _grounded = Physics.CheckSphere(_groundCheckOrigin.position, _groundCheckRadius, _groundLayerMask);
    }

    private void FixedUpdate()
    {
        ApplyGravity();
    }
    
    public void Move(Vector2 inputDirection)
    {
        var rawDirection = new Vector3(inputDirection.x, 0f, inputDirection.y).normalized;
        var direction = _orientation.TransformDirection(rawDirection);
        _movementDirection = ProjectDirection(direction);
        
        if (!_grounded) return;
        
        if (!CanMove())
        {
            Stop();
            return;
        }
        
        _rb.MovePosition(_rb.position + _movementDirection * (_moveSpeed * Time.fixedDeltaTime));
    }

    public void Jump()
    {
        if (!_grounded || !_readyToJump) return;
        
        _readyToJump = false;
        _rb.linearVelocity = new Vector3(_rb.linearVelocity.x, 0f, _rb.linearVelocity.z);
        _rb.AddForce(-_gravityDirection * _jumpForce, ForceMode.Impulse);
        Invoke(nameof(ApplyJumpCulDown), _jumpCulDown);
        StartCoroutine(JumpRoutine());
    }

    public void Dash()
    {
        
    }

    public void Stop()
    {
        _rb.MovePosition(_rb.position);
        _rb.linearVelocity = new Vector3(0f, _rb.linearVelocity.y, 0f);
    }

    private IEnumerator JumpRoutine()
    {
        yield return new WaitWhile(() => _grounded);
        
        while (!_grounded)
        {
            _rb.AddForce(_movementDirection * _moveSpeed);
            yield return new WaitForFixedUpdate();
        }

        _rb.linearVelocity = new Vector3(0, _rb.linearVelocity.y, 0);
    }
    
    private void ApplyGravity()
    {
        if (_grounded) return;
        
        _rb.AddForce(_gravityDirection * _gravityForce, ForceMode.Force);
    }
    
    private void ApplyJumpCulDown()
    {
        _readyToJump = true;
    }
    
    private Vector3 ProjectDirection(Vector3 forward)
    {
        Physics.Raycast(_groundCheckOrigin.position, _gravityDirection, out var hit, _groundCheckRadius, _groundLayerMask);
        var normal = hit.normal;
        var projectedDirection = forward - Vector3.Dot(forward, normal) * normal;
        return projectedDirection;
    }
    
    private bool CanMove()
    {
        if (Physics.Raycast(_groundCheckOrigin.position, new Vector3(_movementDirection.x, 0, _movementDirection.z), out var hit, _slopeCheckDistance, _groundLayerMask))
        {
            var angle = Mathf.Abs(Vector3.Angle(_orientation.up, hit.normal));
            return angle <= _slopeAngle;
        }

        return true;
    }

    private void OnDrawGizmos()
    {
        if (_groundCheckOrigin != null)
        {
            if (_drawGroundCheckGizmos)
            {
                Gizmos.color = _groundCheckGizmoColor;
                Gizmos.DrawWireSphere(_groundCheckOrigin.position, _groundCheckRadius);
            }
            
            if (_drawSlopeAngleGizmos)
            {
                Gizmos.color = _slopeCheckGizmoColor;
                Gizmos.DrawLine(_groundCheckOrigin.position, _groundCheckOrigin.position + new Vector3(_movementDirection.x, 0, _movementDirection.z) * _slopeCheckDistance);
            }
        }

        if (_drawMoveDirectionGizmos)
        {
            Gizmos.color = _moveDirectionGizmoColor;
            Gizmos.DrawLine(transform.position, transform.position + _movementDirection);
        }
    }

    private void OnValidate()
    {
        _gravityDirection.Normalize();
    }
}