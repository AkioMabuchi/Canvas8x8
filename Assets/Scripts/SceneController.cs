using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : SingletonMonoBehaviour<SceneController>
{
    public void ChangeScene(string sceneName)
    {
        StartCoroutine(CoroutineChangeScene(sceneName));
    }
    IEnumerator CoroutineChangeScene(string sceneName)
    {
        Scene prevScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(sceneName, LoadSceneMode.Additive);
        Scene nextScene = SceneManager.GetSceneByName(sceneName);
        yield return UniTask.WaitUntil(() => nextScene.isLoaded);
        SceneManager.SetActiveScene(nextScene);
        yield return SceneManager.UnloadSceneAsync(prevScene);
    }
}
