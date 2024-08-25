using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadState : IGameBaseState
{
    public GameStatus Status => GameStatus.LOAD;
    private string sceneToLoad;
    private float loadingTime;

    public LoadState(string sceneName, float loadTime)
    {
        sceneToLoad = sceneName;
        loadingTime = loadTime;
    }

    public void OnEnterState()
    {
        //Debug.Log("Enter Load State");
        UIManager.Instance.ShowUIPanel(Status, true, 0.3f, 0.1f);
    }

    public void OnExitState()
    {
        UIManager.Instance.HideUIPanel(Status, true, 0.3f, 0.1f);
        SectionGenerator.Instance.Scrolling = true;
    }
    

    public IEnumerator UnloadScene()
    {
        Scene gameScene = SceneManager.GetSceneByName(sceneToLoad);
        
        if (gameScene.isLoaded)
        {
            AsyncOperation unload = SceneManager.UnloadSceneAsync(sceneToLoad);
            while (!unload.isDone) yield return null;
        }
    }

    public IEnumerator LoadScene()
    {
        float startLoadingTime = Time.time;
        
        AsyncOperation async = SceneManager.LoadSceneAsync(sceneToLoad, LoadSceneMode.Additive);
        while (!async.isDone) yield return null;
        
        SectionGenerator.Instance.Scrolling = false;
        
        // Make sure the load screen doesn't flash if the loading is fast, wait 0.5s
        float loadedTime = Time.time - startLoadingTime;
        if (loadedTime < loadingTime)
        {
            yield return new WaitForSeconds(loadingTime - loadedTime);
        }
    }
}
