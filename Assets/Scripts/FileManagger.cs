using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using UnityEngine.UI;
using TMPro;
public class FileManagger : MonoBehaviour
{
    public string logFilePath;
    public string missionfolderPath;
    public string mission;
    public string userName;
    public string mainfolderPath;

    public TextMeshProUGUI logtext; 
    // Start is called before the first frame update
    void Start()
    {
        userName = Environment.UserName;
        mainfolderPath = $@"C:\Users\{userName}\Documents\UAVTelemetry";
            if (Directory.Exists(mainfolderPath))
                {
                    //Debug.Log("Klasör mevcut.");
                }
            else
                {
                    //Debug.Log("Klasör bulunamadı.");
                    Directory.CreateDirectory(mainfolderPath);
                    Debug.Log("Ana Dizin Klasörü Oluşturuldu");
                }

        mission = PlayerPrefs.GetString("mission");
        Debug.Log("last mission =" + mission);
        if (mission == null)
        {
            PlayerPrefs.SetString("mission","1");
            Debug.Log("mission = 1");
        }
        else 
        {
            int missionint = int.Parse(mission);
            missionint++;
            mission = missionint.ToString();
            PlayerPrefs.SetString("mission", mission);
            Debug.Log("current mission = " + mission);

        }

        missionfolderPath = $@"C:\Users\{userName}\Documents\UAVTelemetry\mission{mission}";
        logFilePath = missionfolderPath + "/log.txt";
        
        Directory.CreateDirectory(missionfolderPath);
        Debug.Log($"Görev{mission} Klasörü Oluşturuldu");

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnApplicationQuit()
    {   
        File.AppendAllText(logFilePath, "Log başladı: " + System.DateTime.Now + "\n" + logtext.text);
    }

}
