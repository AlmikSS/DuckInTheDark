using DG.Tweening;
using UnityEngine;

public class Zoomer : MonoBehaviour
{
    [SerializeField] private float _duration;
    
    public void OnHover()
    {
        transform.DOScale(transform.localScale * 1.2f, _duration).SetEase(Ease.OutBounce);
    }

    public void OnExit()
    {
        transform.DOScale(transform.localScale / 1.2f, _duration).SetEase(Ease.OutBounce);
    }
}