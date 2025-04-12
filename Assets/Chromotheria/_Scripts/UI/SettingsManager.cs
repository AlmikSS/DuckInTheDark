using UnityEngine;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    [SerializeField] private Slider _sensitivitySlider;
    
    private float _sensitivity;
    
    public float Sensitivity => _sensitivity;

    private void Awake()
    {
        DontDestroyOnLoad(this);
        _sensitivity = _sensitivitySlider.value;
    }
    
    public void ChangeSensitivity()
    {
        _sensitivity = _sensitivitySlider.value;
    }
}