using UnityEngine;
using Zenject;

public class PlayerInteractions : MonoBehaviour, IPlayerInteractions
{
    [SerializeField] private float _checkDistance = 10f;
    [SerializeField] private LayerMask _interactableLayerMask;
    
    private Camera _camera;

    [Inject]
    private void Construct(Camera camera)
    {
        _camera = camera;
    }
    
    public void TryInteract()
    {
        var ray = _camera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out var hit, _checkDistance, _interactableLayerMask))
        {
            if (hit.collider.gameObject.TryGetComponent(out IInteractable interactable))
                interactable.Interact(gameObject);
        }
    }
}