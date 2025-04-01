using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using UnityEngine.UI;
using System;
public class Limits : MonoBehaviour
{
    public Text timeText;
    public Text dateText;
    private bool bateryalert;
    private bool Voltagealert;
    private bool Currentalert;
    private bool Velocityalert;
    private bool Altitudealert;
    public Text Baterytext;
    public Text VoltageText;
    public Text CurrentText;
    public Text GroundSpeedText;
    public Text AirSpeedText;
    public Text AltitudeText;

    public Text TempatureText;
    public Text AccelerationText;


    public InputField Baterylimit;

    public InputField Voltagehlimit;
    public InputField Voltagellimit;

    public InputField Currenthlimit;
    public InputField Currentllimit;

    public InputField Velocityhlimit;
    public InputField Velocityllimit;

    public InputField Altitudehlimit;
    public InputField Altitudellimit;
    
    // Start is called before the first frame update
    void Start()
    {
        bateryalert = false;
        Velocityalert = false;
        Currentalert = false;
        Velocityalert = false;
        Voltagealert = false;
    }

    // Update is called once per frame
    void Update()
    {

        timeText.text = DateTime.Now.ToString("HH:mm:ss");
        dateText.text = DateTime.Now.ToString("dd/MM/yyyy");

        if (float.TryParse(VoltageText.text, out float textValueVoltage) &&
            float.TryParse(Voltagellimit.text, out float minValueVoltage) &&
            float.TryParse(Voltagehlimit.text, out float maxValueVoltage))
        {
            if (textValueVoltage >= minValueVoltage && textValueVoltage <= maxValueVoltage)
            {
                VoltageText.color = Color.green;
                Voltagealert = false;
                
            }
            else
            {
                if(Voltagealert == false){
                VoltageText.color = Color.red;
                Debug.Log("Voltaj Sınırı Aşıldı Değer:" + textValueVoltage);
                Voltagealert = true;
                }
            }
        }
        else
        {
            VoltageText.color = Color.red;
        }

        if (float.TryParse(CurrentText.text, out float textValueCurrent) &&
            float.TryParse(Currentllimit.text, out float minValueCurrent) &&
            float.TryParse(Currenthlimit.text, out float maxValueCurrent))
        {
            if (textValueCurrent >= minValueCurrent && textValueCurrent <= maxValueCurrent)
            {
                CurrentText.color = Color.green;
                Currentalert = false;
            }
            else
            {
                if (Currentalert == false){
                CurrentText.color = Color.red;
                Debug.Log("Akım Sınırı Aşıldı Değer:" + textValueCurrent);
                Currentalert = true;
                }
            }
        }
        else
        {
            CurrentText.color = Color.red;
        }
    //3
        if (float.TryParse(GroundSpeedText.text, out float textValueVelocity) && 
            float.TryParse(Velocityllimit.text, out float minValueVelocity) &&
            float.TryParse(Velocityhlimit.text, out float maxValueVelocity)) 
        {
            if (textValueVelocity >= minValueVelocity && textValueVelocity <= maxValueVelocity)
            {
                GroundSpeedText.color = Color.green;
                Velocityalert = false;
            }
            else
            {
                if(Velocityalert == false){
                GroundSpeedText.color = Color.red;
                Debug.Log("Hız Sınırı Aşıldı Değer:" + textValueVelocity);
                Velocityalert = true;
                }
            }
        }
        else
        {
            GroundSpeedText.color = Color.red;
        }
//4
        if (float.TryParse(Baterytext.text, out float textValueBattery) &&
            float.TryParse(Baterylimit.text, out float minValueBattery))
        {
            if (textValueBattery >= minValueBattery)
            {
                Baterytext.color = Color.green;
                bateryalert = false;
            }
            else
            {
                if (!bateryalert){
                Baterytext.color = Color.red;
                Debug.Log("Batarya Azaldı:" + textValueBattery);
                bateryalert = true;
                }
            }
        }
        else
        {
            VoltageText.color = Color.red;
        }

        if (float.TryParse(AltitudeText.text, out float textValueAltitude) &&
            float.TryParse(Altitudellimit.text, out float minValueAltitude) &&
            float.TryParse(Altitudehlimit.text, out float maxValueAltitude))
        {
            if (textValueAltitude >= minValueAltitude && textValueAltitude <= maxValueAltitude)
            {
                AltitudeText.color = Color.green;
                Altitudealert = false;
            }
            else
            {
                if(!Altitudealert){
                AltitudeText.color = Color.red;
                Debug.Log("İrtifa sınırların dışında :" + AltitudeText.text);
                Altitudealert = true;
                }
            }
        }
        else
        {
            AltitudeText.color = Color.red;
        }


    
    GroundSpeedText.text = PlayerPrefs.GetString("groundspeed");
    AirSpeedText.text = PlayerPrefs.GetString("airspeed");
    AltitudeText.text = PlayerPrefs.GetString("altitude");
    Baterytext.text = PlayerPrefs.GetString("battery");
    VoltageText.text = PlayerPrefs.GetString("voltage");
    CurrentText.text = PlayerPrefs.GetString("current");
    TempatureText.text = PlayerPrefs.GetString("temperature");
    AccelerationText.text = PlayerPrefs.GetString("acceleration");

    }
}
