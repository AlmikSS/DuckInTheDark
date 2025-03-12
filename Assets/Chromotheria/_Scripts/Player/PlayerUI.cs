using TMPro;
using UnityEngine;
using Zenject;

public class PlayerUI : MonoBehaviour
{
    [SerializeField] private TMP_Text _playerMovementStateText;
    
    private IPlayerMovement _playerMovement;

    [Inject]
    private void Construct(IPlayerMovement playerMovement)
    {
        _playerMovement = playerMovement;
    }

    private void Update()
    {
        var movementState = _playerMovement.MovementState.ToString();
        _playerMovementStateText.SetText(movementState);
    }
}