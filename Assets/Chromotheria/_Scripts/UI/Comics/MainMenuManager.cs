using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private string _gamePlaySceneName;
    [SerializeField] private GameObject _toActivate;
    [SerializeField] private GameObject _toInactivate;
    
    public void PlayBtn()
    {
        StartCoroutine(LoadSceneRoutine(_gamePlaySceneName));
    }

    private IEnumerator LoadSceneRoutine(string sceneName)
    {
        var asyncOperation = SceneManager.LoadSceneAsync(sceneName);
        asyncOperation.allowSceneActivation = false;
        _toActivate.SetActive(true);
        _toInactivate.SetActive(false);
        yield return asyncOperation.isDone;
        yield return new WaitForSecondsRealtime(2);
        asyncOperation.allowSceneActivation = true;
    }
}