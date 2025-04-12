using UnityEngine;
using Zenject;

public class PlayerAnimator : MonoBehaviour
{
    private Animator _animator;
    private IPlayerMovement _playerMovement;

    [Inject]
    private void Construct(Animator animator, IPlayerMovement playerMovement)
    {
        _animator = animator;
        _playerMovement = playerMovement;
    }

    private void Update()
    {
        var vel = _playerMovement.MovementDirection.magnitude;
        _animator.SetFloat("Velocity", vel);
    }
}