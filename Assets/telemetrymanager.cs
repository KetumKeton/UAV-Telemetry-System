using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO.Ports;
using System.Text;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class telemetrymanager : MonoBehaviour
{
    public GameObject LogMenu;
    public GameObject SettingsMenu;
    public TextMeshProUGUI textMeshPro;
   //public Text logText; // UI Text objesi 
    private string logContent = ""; // Logları saklamak için değişken
    public ScrollRect scrollRect; // Scroll View için referans


     void OnEnable()
    {
        Application.logMessageReceived += HandleLog;
    }

        void OnDisable()
    {
        Application.logMessageReceived -= HandleLog;
    }

 private void HandleLog(string logString, string stackTrace, LogType type)
    {
        logContent += logString + "\n";
        if (textMeshPro.text != null) //if (logText != null)
        {
            
            textMeshPro.text = logContent;
       
            ScrollToBottom(); // En alta kaydır
        }
    }

    private void ScrollToBottom()
    {
        // Scroll View'ı en alt pozisyona getir
        Canvas.ForceUpdateCanvases(); // UI güncellemesini zorla
        scrollRect.verticalNormalizedPosition = 0f;
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void LogButton()
    {
        LogMenu.SetActive(true);
    }

    public void LogCloseButton()
    {
        LogMenu.SetActive(false);    
    }

    public void SettingsButton()
    {
        SettingsMenu.SetActive(true);    
    }

    public void SettingsCloseButton()
    {
        SettingsMenu.SetActive(false); 
    }

}
