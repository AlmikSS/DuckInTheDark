using UnityEngine;
using UnityEngine.SceneManagement;
using VH.Tools;
using Zenject;

public class LoseWinUI : MonoBehaviour
{
    [SerializeField] private GameObject _winPanel;
    [SerializeField] private GameObject _losePanel;
    
    private EventBus _eventBus;
    
    [Inject]
    private void Construct(EventBus eventBus)
    {
        _eventBus = eventBus;
    }

    private void Start()
    {
        _eventBus.Register<WinEvent>(OnWin);
        _eventBus.Register<LoseEvent>(OnLose);
    }

    private void OnWin(WinEvent e)
    {
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
        Time.timeScale = 0f;
        _winPanel.SetActive(true);
    }

    private void OnLose(LoseEvent e)
    {
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
        Time.timeScale = 0f;
        _losePanel.SetActive(true);
    }

    public void Restart()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void OnDestroy()
    {
        _eventBus.Unregister<WinEvent>(OnWin);
        _eventBus.Unregister<LoseEvent>(OnLose);
    }
}