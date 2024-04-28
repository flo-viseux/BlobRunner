using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadPersitantScene : MonoBehaviour
{
    [SerializeField] private string sceneName;
    
    void Start()
    {
        if (String.IsNullOrEmpty(sceneName)) sceneName = "Persistant";
        SceneManager.LoadScene(sceneName, LoadSceneMode.Additive);
    }

}
