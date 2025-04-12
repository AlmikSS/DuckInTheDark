using DG.Tweening;
using UnityEngine;

public class Slide : MonoBehaviour
{
    [SerializeField] private float _offset;
    [SerializeField] private float _duration = 30f;
    [SerializeField] private GameObject _next;
    [SerializeField] private bool _activate;
    
    private Tween _tween;
    
    private void OnEnable()
    {
        _tween = transform.DOMoveY(_offset, _duration).SetEase(Ease.Linear);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (_tween == null)
            {
                if (_tween.IsActive())
                    _tween.Complete();   
            }
            else
            {
                if (!_activate)
                    gameObject.SetActive(false);
                
                if (_next != null)
                    _next.SetActive(true);
            }
        }
    }
}