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

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO.Ports;
using System.Text;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;
using UnityEngine.Networking;
//using static MAVLink;
public class MAVLinkReceiver : MonoBehaviour
{
    MAVLink mavlink = new Mavlink();
    
    public Dropdown comportd;
    public Dropdown baudrated;
    public GameObject baglandi;
    public GameObject baglanmadi;
    public Text comptext;
 
    SerialPort stream = new SerialPort(); 

    bool flag1;
    string receivedData = "";
    void Start()
    {

        flag1 = false;

        PlayerPrefs.SetString("seribaglanti","0");

        stream.Close();
        stream.BaudRate = int.Parse(PlayerPrefs.GetString("BaudRate"));
        PlayerPrefs.Save();
        stream.PortName = PlayerPrefs.GetString("COMPort");
        stream.Parity = Parity.None;
        stream.DataBits = 8;
        stream.StopBits = StopBits.One;
        stream.Handshake = Handshake.None;
        stream.ReadTimeout = 1000;
        stream.WriteTimeout = 100;
        if(stream.IsOpen){ 
        PlayerPrefs.SetString("seribaglanti","1");
        }else{
         Baglantibaslat(); 
        }
    }


        public void Baglantibaslat(){
        try
        {
            // SERI BAGLANTIYI AC
            stream.Close();
            stream.PortName = PlayerPrefs.GetString("COMPort"); //!! port butona bastığında da yenilenmeli
            stream.BaudRate = int.Parse(PlayerPrefs.GetString("BaudRate"));
            Debug.Log("Bağlanılan COM Port: " + stream.PortName);
            Debug.Log("bağlantı bu portta ve Baudrate ile baslatiliyor" + PlayerPrefs.GetString("COMPort") +" / "+PlayerPrefs.GetString("BaudRate"));
            stream.Open();
            PlayerPrefs.SetString("seribaglanti","1");
        }
        catch (System.Exception e)
        {
            Debug.LogError("Seri baglanti acilamadi: " + e.Message);
           // Baglantibaslat();
           PlayerPrefs.SetString("seribaglanti","0"); 
        }
        }
        public void StreamC(){
            stream.Close();
        }
void SeriOku() //mavlink v1 okuma 
{
    try //_-------------------
    {
        
        /*
        byte startByte = 0xFE;

        byte[] buffer = new byte[1024]; 

        while (stream.BytesToRead > 0)
        {
            byte receivedByte = (byte)stream.ReadByte();

            if (receivedByte == startByte)
            {
                buffer[0]= receivedByte;
                byte payloadLength = (byte)stream.ReadByte();
                buffer[1]=payloadLength;
                Debug.Log(payloadLength);
                // Payload uzunluğu kadar veriyi oku
                byte[] payloadData = new byte[payloadLength + 6];
                int bytesRead = 0;
                while (bytesRead < payloadLength + 6) //data uzunluğu + seq + 2 chech sum + mesage id + comp id + sys id (6 sabit data)
                {
                    payloadData[bytesRead] = (byte)stream.ReadByte();
                    bytesRead++;
                }
                Array.Copy(payloadData, 0, buffer, 2, payloadData.Length); 

                receivedData = BitConverter.ToString(payloadData);
                PlayerPrefs.SetString("receivedDataGLB", receivedData);
                Debug.Log(payloadData);
                // Debug log için
                Debug.Log("Alınan Veri GLB: " + receivedData);
                break;
            }
        }*/

        if (serialPort != null && serialPort.IsOpen && serialPort.BytesToRead > 0)
        {
            byte[] buffer = new byte[serialPort.BytesToRead];
            serialPort.Read(buffer, 0, buffer.Length);

            foreach (byte b in buffer)
            {
                if (mavlink.ParseByte(b))
                {
                    MAVLinkMessage message = mavlink.LastReceivedMessage;

                    // Gelen MAVLink mesajlarını işle
                    ProcessMAVLinkMessage(message);
                }
            }
        }

    }  //_________________
    catch (System.Exception e)
    {
        // Hata durumunda
        receivedData = "EMP" + e;
    }
}

    void Update()
    {

      if(PlayerPrefs.GetString("seribaglanti")=="1")
    {
      baglandi.SetActive(true);
      baglanmadi.SetActive(false);
      comptext.text = PlayerPrefs.GetString("COMPort") +" / "+ baudrated.options[baudrated.value].text;
    }else{
      baglandi.SetActive(false);
      baglanmadi.SetActive(true);
      comptext.text = "Bağlantı İstenilen Port: " + PlayerPrefs.GetString("COMPort") +" / "+ PlayerPrefs.GetString("BaudRate");
    }

        if(!stream.IsOpen && flag1 == false ){Debug.Log("port açık değil!"); flag1 = true; Baglantibaslat();} //V2D

        // START KOMUTU
        if (stream.IsOpen) {
            SeriOku();

        if (PlayerPrefs.GetString("receivedDataGLB").Trim().Length == 8)
        {
            PlayerPrefs.SetString("GelenData",PlayerPrefs.GetString("receivedDataGLB").Trim());
            PlayerPrefs.DeleteKey("receivedDataGLB");
            Debug.Log("sifrealindi");
            PlayerPrefs.SetInt("verialindi",1);
            stream.Write("O");
        }
     }
    }

        void ProcessMAVLinkMessage(MAVLinkMessage message)
    {
        switch (message.MsgID)
        {
            case (uint)MAVLINK_MSG_ID.VFR_HUD:
                var vfrHud = (MAVLINK_MSG_VFR_HUD)message.ToStructure();
                float speed = vfrHud.groundspeed; // Hız (m/s)
                float altitude = vfrHud.alt; // Yükseklik (m)

                // UI Güncelle
                speedText.text = "Hız: " + speed.ToString("F1") + " m/s";
                altitudeText.text = "Yükseklik: " + altitude.ToString("F1") + " m";
                break;

            case (uint)MAVLINK_MSG_ID.SYS_STATUS:
                var status = (MAVLINK_MSG_SYS_STATUS)message.ToStructure();
                float battery = status.voltage_battery / 1000.0f; // Voltajı mV'den V'ye çevir

                batteryText.text = "Pil: " + battery.ToString("F1") + " V";
                break;

            case (uint)MAVLINK_MSG_ID.SCALED_IMU:
                var imu = (MAVLINK_MSG_SCALED_IMU)message.ToStructure();
                float accelX = imu.xacc / 1000.0f; // m/s²
                float accelY = imu.yacc / 1000.0f;
                float accelZ = imu.zacc / 1000.0f;

                accelText.text = $"İvme: X:{accelX:F1} Y:{accelY:F1} Z:{accelZ:F1}";
                break;
        }
    }

     public void combutton()
    {
      PlayerPrefs.SetString("COMPort",comportd.options[comportd.value].text);
      PlayerPrefs.SetString("BaudRate",baudrated.options[baudrated.value].text);
      Debug.Log("comporta koyuldu" + comportd.options[comportd.value].text +" / "+ baudrated.options[baudrated.value].text);
      PlayerPrefs.Save();
    }

    void OnApplicationQuit()
    {
        if (stream != null && stream.IsOpen)
        {
            stream.Close();
            Debug.Log("Seri baglanti kapatildi.");
        }

       // isStreaming = false;

    }

}