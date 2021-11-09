using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : SingletonMonoBehaviour<SceneController>
{
    private string _currentSceneName;
    public void SetCurrentSceneName(string sceneMane)
    {
        _currentSceneName = sceneMane;
    }
    public void ChangeScene(string sceneName)
    {
        StartCoroutine(CoroutineChangeScene(sceneName));
    }
    IEnumerator CoroutineChangeScene(string sceneName)
    {
        Scene prevScene = SceneManager.GetSceneByName(_currentSceneName);
        SceneManager.LoadScene(sceneName, LoadSceneMode.Additive);
        Scene nextScene = SceneManager.GetSceneByName(sceneName);
        while (!nextScene.isLoaded) yield return null;
        SceneManager.SetActiveScene(nextScene);
        yield return SceneManager.UnloadSceneAsync(prevScene);
    }
}
