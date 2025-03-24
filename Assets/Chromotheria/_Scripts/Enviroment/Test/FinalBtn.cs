using UnityEngine;
using VH.Tools;
using Zenject;

public class FinalBtn : MonoBehaviour, IInteractable
{
    [SerializeField] private ItemSO _item;
    
    private EventBus _eventBus;

    [Inject]
    private void Construct(EventBus eventBus)
    {
        _eventBus = eventBus;
    }
    
    public void Interact(GameObject sender)
    {
        if (sender.TryGetComponent(out PlayerInventory inventory))
        {
            if (inventory.HaveItem(_item, 3))
                _eventBus.Invoke(new WinEvent(this));
        }
    }
}