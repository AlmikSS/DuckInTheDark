using System.Collections;
using UnityEngine;
using Zenject;

[RequireComponent(typeof(Rigidbody))]
public class RigidbodyPlayerMovement : MonoBehaviour, IPlayerMovement
{
    [Header("Movement")] [SerializeField] private Transform _orientation;
    [SerializeField] private Transform _headCheckOrigin;
    [SerializeField] private LayerMask _obstacleLayerMask;
    [SerializeField] private float _checkObstacleDistance = 0.5f;
    [SerializeField] private float _walkSpeed = 7f;
    [SerializeField] private float _sprintSpeed = 10f;
    [SerializeField] private float _slopeAngle = 45f;
    [SerializeField] private float _slopeCheckDistance = 0.5f;

    [Header("Jump")] [SerializeField] private float _jumpForce = 5f;
    [SerializeField] private float _jumpCulDown = 0.2f;
    [SerializeField] private Transform _ledgeCheckOrigin;

    [Header("Dash")] [SerializeField] private float _dashForce = 50f;
    [SerializeField] private float _dashTme = 0.1f;
    [SerializeField] private float _dashCulDown = 0.5f;

    [Header("Physics")] [SerializeField] private float _gravityForce = 9.81f;
    [SerializeField] private Vector3 _gravityDirection = Vector3.down;

    [Header("Ground Check")] [SerializeField]
    private Transform _groundCheckOrigin;

    [SerializeField] private LayerMask _groundLayerMask;
    [SerializeField] private float _groundCheckRadius;

    [Header("Gizmos")] [SerializeField] private bool _drawGroundCheckGizmos;
    [SerializeField] private Color _groundCheckGizmoColor = Color.blue;
    [SerializeField] private bool _drawMoveDirectionGizmos;
    [SerializeField] private Color _moveDirectionGizmoColor = Color.green;
    [SerializeField] private bool _drawSlopeAngleGizmos;
    [SerializeField] private Color _slopeCheckGizmoColor = Color.yellow;
    [SerializeField] private bool _drawCheckObstacleGizmos;
    [SerializeField] private Color _checkObstacleGizmoColor = Color.red;

    private Rigidbody _rb;
    private IPlayerParkour _playerParkour;
    private Vector3 _movementDirection;
    private bool _grounded;
    private bool _readyToJump;
    private bool _readyToDash;
    private bool _isDashing;
    private bool _readyToMove;
    private bool _useGravity;
    private float _moveSpeed;

    public float Velocity => _movementDirection.magnitude * _moveSpeed;
    public bool Grounded => _grounded;

    [Inject]
    private void Construct(IPlayerParkour playerParkour)
    {
        _playerParkour = playerParkour;
    }

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _rb.freezeRotation = true;
        _rb.useGravity = false;
        _moveSpeed = _walkSpeed;
        _readyToJump = true;
        _readyToDash = true;
        _readyToMove = true;
        _useGravity = true;
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
        if (!_readyToMove) return;

        if (!_isDashing)
            _rb.linearVelocity = new Vector3(0f, _rb.linearVelocity.y, 0f);

        var rawDirection = new Vector3(inputDirection.x, 0f, inputDirection.y).normalized;
        var direction = _orientation.TransformDirection(rawDirection);

        if (_grounded)
            _movementDirection = ProjectDirection(direction);

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

        StartCoroutine(JumpRoutine());
    }

    public void Dash()
    {
        if (!_readyToDash) return;

        StartCoroutine(DashRoutine());
    }

    public void Sprint(bool isSprinting)
    {
        StartCoroutine(SprintRoutine(isSprinting));
    }

    public void Stop()
    {
        _movementDirection = Vector3.zero;
        _rb.MovePosition(_rb.position);
        _rb.linearVelocity = new Vector3(0f, _rb.linearVelocity.y, 0f);
    }

    private IEnumerator JumpRoutine()
    {
        var direction = GetDirection();

        if (_playerParkour.CheckObstacle(direction, transform.position, out var point))
        {
            StartCoroutine(Climb(point));
        }
        else
        {
            _readyToJump = false;
            _rb.linearVelocity = new Vector3(_rb.linearVelocity.x, 0f, _rb.linearVelocity.z);
            _rb.AddForce(-_gravityDirection * _jumpForce, ForceMode.Impulse);
            yield return new WaitForSeconds(_jumpCulDown);
            _readyToJump = true;
        }
    }

    private IEnumerator Climb(Vector3 point)
    {
        _useGravity = false;
        _readyToMove = false;
        yield return _playerParkour.ApplyJump(point);
        _useGravity = true;
        _readyToMove = true;
        Stop();
    }

    private IEnumerator SprintRoutine(bool isSprinting)
    {
        if (!_grounded)
            yield return new WaitUntil(() => _grounded);

        if (isSprinting)
            _moveSpeed = _sprintSpeed;
        else
            _moveSpeed = _walkSpeed;
    }

    private IEnumerator DashRoutine()
    {
        _readyToDash = false;
        _isDashing = true;

        var direction = new Vector3(_movementDirection.x, 0, _movementDirection.z).normalized;
        if (direction == Vector3.zero)
            direction = new Vector3(_orientation.forward.x, 0, _orientation.forward.z).normalized;

        _rb.AddForce(direction * _dashForce, ForceMode.Impulse);
        yield return new WaitForSeconds(_dashTme);
        _rb.linearVelocity = new Vector3(0f, _rb.linearVelocity.y, 0f);
        _isDashing = false;

        yield return new WaitForSeconds(_dashCulDown);
        _readyToDash = true;
    }

    private void ApplyGravity()
    {
        if (_grounded || !_useGravity) return;

        _rb.AddForce(_gravityDirection * _gravityForce, ForceMode.Force);
    }

    private Vector3 ProjectDirection(Vector3 forward)
    {
        Physics.Raycast(_groundCheckOrigin.position, _gravityDirection, out var hit, _groundCheckRadius, _groundLayerMask);
        var normal = hit.normal;
        var projectedDirection = forward - Vector3.Dot(forward, normal) * normal;
        return projectedDirection;
    }

    private Vector3 GetDirection()
    {
        var direction = _movementDirection.normalized;
        if (direction == Vector3.zero)
            direction = _orientation.forward;
        return direction;
    }

    private bool CanMove()
    {
        var checkSlope = CheckSlope();
        var obstacleForward = CheckObstacleForward();

        return checkSlope && !obstacleForward;
    }

    private bool CheckObstacleForward()
    {
        return Physics.Raycast(_headCheckOrigin.position, new Vector3(_movementDirection.x, 0, _movementDirection.z), _checkObstacleDistance, _obstacleLayerMask);
    }

    private bool CheckSlope()
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

        if (_drawCheckObstacleGizmos)
        {
            Gizmos.color = _checkObstacleGizmoColor;
            Gizmos.DrawLine(_headCheckOrigin.position, _headCheckOrigin.position + new Vector3(_movementDirection.x, 0, _movementDirection.z) * _checkObstacleDistance);
        }
    }

    private void OnValidate()
    {
        _gravityDirection.Normalize();
        _gravityForce = Mathf.Abs(_gravityForce);
    }
}