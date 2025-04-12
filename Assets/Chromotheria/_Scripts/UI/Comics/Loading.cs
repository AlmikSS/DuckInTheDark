using System.Collections;
using UnityEngine;

public class Loading : MonoBehaviour
{
    [SerializeField] private float _rotationSpeed;
    
    private void OnEnable()
    {
        StartCoroutine(RotationRoutine());
    }

    private IEnumerator RotationRoutine()
    {
        while (true)
        {
            transform.Rotate(0, 0, _rotationSpeed * Time.deltaTime);
            yield return null;
        }
    }
}