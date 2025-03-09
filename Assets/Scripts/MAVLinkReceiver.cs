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
//Bu kod RF alıcı ile seriporttan aldığı IHA telemetri datasını ekranda gösterilmek üzere playerpref e yazıdırır. System.IO ve MAVLink kütüphaneleri kullanılmıştır.
// V1 mavlink ;mavlink kütüphanesi ilk kez seriport yaılımınıa eklendi
//V2 Mavlink (mavparser eklendi, serioku bölümüne memorystream eklendi, seriokuda mavlink readpackage kullanıldı,
//process bölümünde isim düzeltmesi yapıldı
//V3 test byteları eklendi, ms pozisyonu 0 a ayarlandı, debug loglar eklendi seri oku vs. catch'a
//V4 MAVLink i namespace olarak dahil edildi, memorystream kaldırıldı, seri port okuması değiştirildi 
//NOT V4 hatalı yapılmış kütüphane kullanım gereği memorystream kullanılması geekiyor. V3 e dönülmesi daha doğru V3 ten devam edilicektir.
//V5 V3'ün devamıdır, flag1 baglantı kurulduysa false yapıldı
using UnityEngine;
using System;
using System.IO.Ports;
using System.IO;
using UnityEngine.UI;

public class MAVLinkReceiver : MonoBehaviour
{
    SerialPort stream = new SerialPort();
    MAVLink mavlink = new MAVLink();
    MAVLink.MavlinkParse mavParser = new MAVLink.MavlinkParse();

    public Dropdown comportd;
    public Dropdown baudrated;
    public GameObject baglandi;
    public GameObject baglanmadi;
    public Text comptext;

    bool flag1;
    string receivedData = "";

    void Start()
    {
        flag1 = false;
        PlayerPrefs.SetString("seribaglanti", "0");

        stream.Close();
        stream.BaudRate = int.Parse(PlayerPrefs.GetString("BaudRate"));
        stream.PortName = PlayerPrefs.GetString("COMPort");
        stream.Parity = Parity.None;
        stream.DataBits = 8;
        stream.StopBits = StopBits.One;
        stream.Handshake = Handshake.None;
        stream.ReadTimeout = 1000;
        stream.WriteTimeout = 100;

        if (stream.IsOpen)
        {
            PlayerPrefs.SetString("seribaglanti", "1");
        }
        else
        {
            Baglantibaslat();
        }
    }

    public void Baglantibaslat()
    {
        try
        {
            stream.Close();
            stream.PortName = PlayerPrefs.GetString("COMPort");
            stream.BaudRate = int.Parse(PlayerPrefs.GetString("BaudRate"));
            stream.Open();
            PlayerPrefs.SetString("seribaglanti", "1");
            flag1 = false;
        }
        catch (Exception e)
        {
            Debug.LogError("Seri bağlantı açılamadı: " + e.Message);
            PlayerPrefs.SetString("seribaglanti", "0");
        }
    }

    public void StreamC()
    {
       stream.Close();
    }

    void SeriOku()
    {
        try
        {
            if (stream != null && stream.IsOpen && stream.BytesToRead > 0)
            {
                //Debug.Log("Data alindi"); //İŞLİYOR
                byte[] buffer = new byte[stream.BytesToRead];
                int bytesRead = stream.Read(buffer, 0, buffer.Length);
                //Debug.Log("Buffer içeriği: " + BitConverter.ToString(buffer));
                //Debug.Log("Okunan bayt sayısı: " + bytesRead);
                byte[] testV1 = new byte[] { 0xFE, 0x09, 0x01, 0x01, 0x01, 0x00, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09, 0xE1, 0xB7 };//FE 09 01 01 01 00 02 03 04 05 06 07 08 09 E8 D3
                byte[] testV2 = new byte[] { 0xFD, 0x09, 0x00, 0x01, 0x01, 0x01, 0x00, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09, 0x99, 0x99 };
                byte[] testData = new byte[] { 
                                            0xFE, // STX (MAVLink v1)
                                            0x09, // Payload uzunluğu
                                            0x01, // Sequence
                                            0x01, // System ID
                                            0x01, // Component ID
                                            0x00, // Mesaj ID (HEARTBEAT)
                                            0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09, 0x0A, // Payload
                                            0xE1, 0xB7 // CRC (little-endian: 0xB7E1)
                                            };
                
            using (MemoryStream ms = new MemoryStream(testData))
            {
                ms.Position = 0;
                var message = mavParser.ReadPacket(ms); // `mavParser` üzerinden çağır
                //Debug.Log("Okunan message: " + message.ToString() + "$");
                //Debug.Log(message.ToString());

                //Debug.Log("Okunan message: " + message.ToString() + "$");
                Debug.Log("Okunan message: " + (message != null ? message.ToString() : "null") + "$");
                if (message != null)
                {
                    ProcessMAVLinkMessage(message);
                    Debug.Log("ProcessMAVLinkMessage(message) calistirildi");
                }
                else{ 
                    Debug.Log("message null");
                    }
            }
            }
        }
        catch (Exception e)
        {
            receivedData = "EMP" + e;
            //Debug.Log("memorystream error");
            Debug.Log("Hata: " + e.Message + " | StackTrace: " + e.StackTrace);
        }
    }

    void Update()
    {
        if (PlayerPrefs.GetString("seribaglanti") == "1")
        {
            baglandi.SetActive(true);
            baglanmadi.SetActive(false);
            comptext.text = PlayerPrefs.GetString("COMPort") + " / " + baudrated.options[baudrated.value].text;
        }
        else
        {
            baglandi.SetActive(false);
            baglanmadi.SetActive(true);
            comptext.text = "Bağlantı İstenilen Port: " + PlayerPrefs.GetString("COMPort") + " / " + PlayerPrefs.GetString("BaudRate");
        }

        if (!stream.IsOpen && flag1 == false)
        {
            Debug.Log("Port açık değil!");
            flag1 = true;
            Baglantibaslat();
        }

        if (stream.IsOpen)
        {
            SeriOku();
        }
    }
    void ProcessMAVLinkMessage(MAVLink.MAVLinkMessage message)
    {
        switch (message.msgid)  // msgid ile kontrol et
        {
            case (uint)MAVLink.MAVLINK_MSG_ID.VFR_HUD:
             // var vfrHud = (MAVLink.mavlink_vfr_hud_t)message.ToStructure();
                var vfrHud = message.ToStructure<MAVLink.mavlink_vfr_hud_t>();
                float speed = vfrHud.groundspeed;
                float altitude = vfrHud.alt;

                PlayerPrefs.SetString("speed", speed.ToString("F1"));
                PlayerPrefs.SetString("altitude", altitude.ToString("F1"));
                break;

            case (uint)MAVLink.MAVLINK_MSG_ID.SYS_STATUS:
                //var status = (MAVLink.mavlink_sys_status_t)message.ToStructure();
                var status = message.ToStructure<MAVLink.mavlink_sys_status_t>();
                float battery = status.voltage_battery / 1000.0f;

                PlayerPrefs.SetString("battery", battery.ToString("F1"));
                break;

            case (uint)MAVLink.MAVLINK_MSG_ID.SCALED_IMU:
                //var imu = (MAVLink.mavlink_scaled_imu_t)message.ToStructure();
                var imu = message.ToStructure<MAVLink.mavlink_scaled_imu_t>();
                float accelX = imu.xacc / 1000.0f;
                float accelY = imu.yacc / 1000.0f;
                float accelZ = imu.zacc / 1000.0f;

                PlayerPrefs.SetString("accel", $"İvme: X:{accelX:F1} Y:{accelY:F1} Z:{accelZ:F1}");
                break;
        }
    }

    public void combutton()
    {
        PlayerPrefs.SetString("COMPort", comportd.options[comportd.value].text);
        PlayerPrefs.SetString("BaudRate", baudrated.options[baudrated.value].text);
        PlayerPrefs.Save();
        Baglantibaslat();
    }

    void OnApplicationQuit()
    {
        if (stream != null && stream.IsOpen)
        {
            stream.Close();
            Debug.Log("Seri bağlantı kapatıldı.");
        }
    }
}
