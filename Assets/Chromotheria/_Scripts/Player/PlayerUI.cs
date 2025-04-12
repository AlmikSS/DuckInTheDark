using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class PlayerUI : MonoBehaviour
{
    [SerializeField] private Image _healthBarImage;
    [SerializeField] private TMP_Text _losesWinsText;
    
    private PlayerStats _playerStats;

    [Inject]
    private void Construct(PlayerStats playerStats)
    {
        _playerStats = playerStats;
    }

    private void Update()
    {
        _healthBarImage.fillAmount = _playerStats.CurrentHealth / (float)_playerStats.MaxHealth;
        _losesWinsText.SetText($"Loses: {_playerStats.Loses} \n Wins: {_playerStats.Wins}");
    }
}