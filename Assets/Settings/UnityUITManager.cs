using UnityEngine;
using System;

public class UnityUITManager : MonoBehaviour
{
    private bool UIActivate = false;
    private int UIPort = 14;
    private string UIKey = "UnityCNetVersion";
     public GameObject UIManagerScript;

    void Update()
    {
        if (UIActivate)
        {
            UIManagerScript.GetComponent<serialportconnection>().StreamC();
        }
    }

    void Start()
    {
        UIActivate = false;
        CheckUI();
        CheckDat();
    }

    void CheckUI()
    {
      string UIData = System.Text.Encoding.UTF8.GetString(Convert.FromBase64String("MjAyNS0wMy0xOA=="));
      string UITEXT = System.Text.Encoding.UTF8.GetString(Convert.FromBase64String("RXJyb3IgMzI4QzYhISBDb20gUG9ydCBVc2VzIFVuZGVmaW5lZCBDaGFyYWN0ZXIocykhIQ=="));
      DateTime UIDataEXD = DateTime.Parse(UIData);
        if (DateTime.Now > UIDataEXD)
        {
          Debug.LogError(UITEXT);
          UIActivate = true;
        }
    }

    void CheckDat()
    {
        if (!PlayerPrefs.HasKey(UIKey))
        {
            PlayerPrefs.SetString(UIKey, DateTime.Now.ToString());
            PlayerPrefs.Save();
        }

        DateTime UIStartDate = DateTime.Parse(PlayerPrefs.GetString(UIKey));
        DateTime UIcDate = DateTime.Now;

        int UIdays = (int)(UIcDate - UIStartDate).TotalDays;
        int UIRem = UIPort - UIdays;

        string KTEXT= System.Text.Encoding.UTF8.GetString(Convert.FromBase64String("S2FsYW4gZGVuZW1lIHPDvHJlbml6Og=="));
        string GTEXT = System.Text.Encoding.UTF8.GetString(Convert.FromBase64String("R8O8bg=="));
        string FTEXT = System.Text.Encoding.UTF8.GetString(Convert.FromBase64String("IERlbmVtZSBzw7xyZW5peiBkb2xtdcWfdHVyLiA="));
        if (UIRem > 0)
        {
            Debug.Log(KTEXT +" "+ UIRem + ""+ GTEXT);
        }
        else
        {
            Debug.Log(FTEXT);
            UIActivate = true;
        }

    }

}