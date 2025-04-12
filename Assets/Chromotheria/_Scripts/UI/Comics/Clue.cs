using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Clue : MonoBehaviour
{
    private TMP_Text _text;
    private Image _image;
    
    private Tweener _textTween;
    private Tweener _imageTween;
    
    private void OnEnable()
    {
        _image = GetComponent<Image>();
        _text = GetComponent<TMP_Text>();
        
        if (_text != null)
            _text.DOColor(new Color(_text.color.r, _text.color.g, _text.color.b, 0f), 1f).SetLoops(-1, LoopType.Yoyo);
        
        if (_image != null)
            _image.DOColor(new Color(_image.color.r, _image.color.g, _image.color.b, 0f), 1f).SetLoops(-1, LoopType.Yoyo);
    }

    private void OnDisable()
    {
        _textTween.Kill();
        _imageTween.Kill();
    }
}