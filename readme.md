# UAV Telemetry System in Unity

This project is a **UAV telemetry system** developed in **Unity**, designed to receive telemetry data via the **MAVLink protocol** over a **serial port**. The system displays real-time flight data and provides warnings when predefined thresholds are exceeded.

## 🚀 Features

- 📡 **MAVLink Communication** – Receives and processes telemetry data over a serial connection.
- 🎛 **Real-Time Data Display** – Shows important flight parameters on the screen.
- ⚠ **Threshold Alerts** – Provides warnings when sensor values exceed safety limits.
- 📊 **Customizable UI** – Adjust data visualization elements for better monitoring.
- 🔌 **Plug-and-Play** – Easily connects to any MAVLink-compatible UAV system.

## 🔧 Installation

1. Clone this repository:
   git clone https://github.com/Ketonkeko/UAV-Telemetry-System.git

2. Open the project in **Unity** (tested with Unity version _your version_).
3. Ensure you have a MAVLink-compatible UAV sending telemetry data.
4. Connect the UAV via a serial port.
5. Run the project and monitor real-time data.

## 📖 Usage

- Select the correct **serial port** and **baud rate** in the UI.
- Start the telemetry system to receive and display live data.
- If any value exceeds its safety threshold, a **warning message** will appear.
- Modify threshold values in the settings file (_if applicable_).

## 🛠 Dependencies

- Unity (version _your version_)
- MAVLink Protocol
- Serial Communication Library (e.g., `System.IO.Ports` in C#)

## 📝 To-Do

 Data logs can be exproting in detail.
 will be supported by different protocols
 Improve UI with more visual indicators.

## 📜 License

This project is licensed under the **MIT License**. See the [LICENSE](LICENSE) file for details.

## 🤝 Contributing

Contributions are welcome! Feel free to submit **pull requests** or open **issues**.
