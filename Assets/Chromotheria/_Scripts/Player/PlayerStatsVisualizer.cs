using TMPro;
using UnityEngine;
using Zenject;

public class PlayerStatsVisualizer : MonoBehaviour
{
    [SerializeField] private TMP_Text _speedText;
    [SerializeField] private TMP_Text _isGroundedText;
    
    private IPlayerMovement _playerMovement;

    [Inject]
    private void Construct(IPlayerMovement playerMovement)
    {
        _playerMovement = playerMovement;
    }

    private void Update()
    {
        _speedText.SetText("Speed: " + _playerMovement.Velocity);
        _isGroundedText.SetText("Grounded: " + _playerMovement.Grounded);
    }
}