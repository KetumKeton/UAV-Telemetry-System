using UnityEngine;
using System;
using System.IO.Ports;
using System.IO;
using UnityEngine.UI;

public class MAVLinkReceiver : MonoBehaviour
{
    SerialPort stream = new SerialPort();
    MAVLink mavlink = new MAVLink();  // Eğer MAVLink sınıfın böyleyse
    //MavlinkParse mavParser = new MavlinkParse();
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
        }
        catch (Exception e)
        {
            Debug.LogError("Seri bağlantı açılamadı: " + e.Message);
            PlayerPrefs.SetString("seribaglanti", "0");
        }
    }

    public void StreamC()
    {
       // stream.Close();
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
            using (MemoryStream ms = new MemoryStream(buffer))
            {
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
//    void ProcessMAVLinkMessage(MAVLinkMessage message)
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
