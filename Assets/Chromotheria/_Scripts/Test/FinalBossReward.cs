using UnityEngine;

public class FinalBossReward : MonoBehaviour
{
    [SerializeField] private GameObject _reward;

    private void OnDestroy()
    {
        _reward.SetActive(true);
    }
}