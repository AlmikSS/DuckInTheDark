using UnityEngine;
using UnityEngine.SceneManagement;
using VH.Tools;
using Zenject;

public class CutSceneManager : MonoBehaviour
{
    [SerializeField] private GameObject _cutScene;
    [SerializeField] private GameObject _ui;
    [SerializeField] private PlayerController _player;
    [SerializeField] private ItemSO _cristall;

    private EventBus _eventBus;

    [Inject]
    private void Construct(EventBus eventBus)
    {
        _eventBus = eventBus;
    }
    
    public void StopCutScene()
    {
        _ui.SetActive(true);
        _eventBus.Invoke(new WinEvent(this));
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (!_player.gameObject.GetComponent<PlayerInventory>().HaveItem(_cristall, 3))
                return;
            
            _ui.SetActive(false);
            _player.enabled = false;
            _cutScene.SetActive(true);
        }
    }
}