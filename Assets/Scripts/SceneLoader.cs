using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.LowLevel;
using UnityEngine.SceneManagement;

public static class SceneLoader
{
    private const string GameScene = "GameScene";
    private const string PhotonScene = "PhotonScene";
    private const string SceneControllerScene = "SceneControllerScene";
    private const string WallsScene = "WallsScene";
    
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    public static void LoadScenes()
    {
        if (!SceneManager.GetSceneByName(GameScene).IsValid())
        {
            SceneManager.LoadScene(GameScene, LoadSceneMode.Additive);
        }

        if (!SceneManager.GetSceneByName(PhotonScene).IsValid())
        {
            SceneManager.LoadScene(PhotonScene, LoadSceneMode.Additive);
        }

        if (!SceneManager.GetSceneByName(SceneControllerScene).IsValid())
        {
            SceneManager.LoadScene(SceneControllerScene, LoadSceneMode.Additive);
        }
        
        if (!SceneManager.GetSceneByName(WallsScene).IsValid())
        {
            SceneManager.LoadScene(WallsScene, LoadSceneMode.Additive);
        }
    }
}
