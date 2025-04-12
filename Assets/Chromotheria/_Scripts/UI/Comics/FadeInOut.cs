using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class FadeInOut : MonoBehaviour
{
    [SerializeField] private float _duration;
    [SerializeField] private GameObject _toActivate;
    [SerializeField] private GameObject _toInactivate;
    
    private Image _image;
    
    private void OnEnable()
    {
        StartCoroutine(FadeRoutine());
    }

    private IEnumerator FadeRoutine()
    {
        _image = GetComponent<Image>();
        var tween = _image.DOFade(1f, _duration).SetLoops(2, LoopType.Yoyo);
        yield return new WaitForSeconds(_duration);
        _toActivate.SetActive(true);
        _toInactivate.SetActive(false);
    }
}