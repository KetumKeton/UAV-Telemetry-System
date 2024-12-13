/*
The MIT License (MIT)

Copyright (c) 2024 Ege ÖZTETİK

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in
all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN

05079329775
oztetikege@gmail.com
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class ayar : MonoBehaviour
{

    //public InputField comport;
    public Dropdown comportd;
    public GameObject baglandi;
    public GameObject baglanmadi;
    public Text comptext;
    void Update()
    {
    if(PlayerPrefs.GetString("seribaglanti")=="1")
    {
      baglandi.SetActive(true);
      baglanmadi.SetActive(false);
      comptext.text = PlayerPrefs.GetString("COMPort");
    }else{
      baglandi.SetActive(false);
      baglanmadi.SetActive(true);
      comptext.text = "Bağlantı İstenilen Port: " + PlayerPrefs.GetString("COMPort");
    }
    }
    public void combutton()
    {
      Debug.Log(comportd.options[comportd.value].text);
      PlayerPrefs.SetString("COMPort",comportd.options[comportd.value].text);
      Debug.Log("comporta koyuldu" + comportd.options[comportd.value].text);
      PlayerPrefs.Save();
    }

}
