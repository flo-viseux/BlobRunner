using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIDebugText : MonoBehaviour
{
        public TextMeshProUGUI text;

        void OnEnable(){
            Application.logMessageReceived += HandleLog;
        }

        void OnDisable(){
            Application.logMessageReceived -= HandleLog;
        }

        void HandleLog(string logString, string stackTrace, LogType type){
            text.text += $"{logString}\n";
        }
    
}
