using System;
using System.Collections;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
#endif

using UnityEngine;
using UnityEngine.SceneManagement;

public class EditorSceneLoader : MonoBehaviour
{
    #if UNITY_EDITOR

    private Scene current;
    private string nameCurrentScene;
    private EditorSceneLoader instance;
    private void Awake()
    {
        if (instance != null && instance != this)
            Destroy(this);
        else
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
    }

    private void OnEnable()
    {
        EditorApplication.playModeStateChanged += HandlePersistantScene;
    }

    private void OnDisable()
    {
        EditorApplication.playModeStateChanged -= HandlePersistantScene;
    }

    private void HandlePersistantScene(PlayModeStateChange mode)
    {
        current = SceneManager.GetActiveScene();
        nameCurrentScene = current.name;
        if (!SceneManager.GetSceneByName("Persistant").IsValid())
        {
            StartCoroutine(LoadPersistant());
        }        
    }

    private IEnumerator LoadPersistant()
    {
        AsyncOperation op = SceneManager.LoadSceneAsync("Persistant");
        while (!op.isDone) yield return null;
        GameManager manager = FindObjectOfType<GameManager>();
        manager.gameSceneName = nameCurrentScene;
    }
    
    #endif
}
