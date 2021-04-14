# ✈️ FlightInspectionDesktopApp

## 🔎 Overview
FlightInspectionDesktopApp connects to FlightGear and displays given flight data in a nice graphic way.
It detects anomalies based on dynamically-loaded anomaly-detection algorithms and displays them.
### Special Features
- The program **validates the user's input files**:
  -  Ensures that only files of the correct extentions were inserted.
  -  Makes sure that all paths were provided.
  -  Verifies that the XML file is in the correct path. 
  -  Alerts if the user has the FlightGear executable or the provided CSV file open already.
- **FlightGear starts up on its own** and start flying, without the user having to configure any settings. 
- While waiting for FlightGear to start, a **loading window** appears.
- Closing the Inspector window **closes FlightGear automatically**.
- FlightGear window and the program's Inspector **open automatically side by side**, matching the user's screen size.
- Flight data is displayed in a **cool, graphic way**.
## 👪 Project Hierarchy
To view the detailed hierarchy, expand the sections below:
<details>
<summary>FlightInspectionDesktopApp</summary>
<p>

```
├── Altimeter
│   ├── AltimeterModel.cs
│   └── AltimeterViewModel.cs
├── DataModel.cs
├── FG
│   ├── FGModelImp.cs
│   ├── FGViewModel.cs
│   ├── IFGModel.cs
│   └── TelnetClient.cs
├── Graph
│   ├── GraphModel.cs
│   └── GraphViewModel.cs
├── InspectorWindow.xaml
├── InspectorWindow.xaml.cs
├── LoadingWindow.xaml
├── LoadingWindow.xaml.cs
├── MainWindow.xaml
├── MainWindow.xaml.cs
├── Metadata
│   ├── MetadataModel.cs
│   └── MetadataViewModel.cs
├── Player
│   ├── PlayerModel.cs
│   └── PlayerViewModel.cs
├── Plugins
│   └── LinearRegressionDLL.dll
├── PreInstall
│   └── BlendWPFSDK_en.msi
├── Speedometer
│   ├── SpeedometerModel.cs
│   └── SpeedometerViewModel.cs
├── Steering
│   ├── SteeringModel.cs
│   └── SteeringViewModel.cs
├── UserControls
    ├── Altimeter.xaml
    ├── Altimeter.xaml.cs
    ├── Graph.xaml
    ├── Graph.xaml.cs
    ├── Metadata.xaml
    ├── Metadata.xaml.cs
    ├── Player.xaml
    ├── Player.xaml.cs  
    ├── Speedometer.xaml
    ├── Speedometer.xaml.cs
    ├── Steering.xaml
    └── Steering.xaml.cs
```

</p>
</details>
<details>
<summary>LinearRegressionDLL</summary>
<p>

```
├── AnomalyDetectionUtil.cs
├── IAbstractDetector.cs
├── LinearGraphViewModel.cs
├── LinearRegressionDetector.cs
├── LinearRegressionGraph.xaml
├── LinearRegressionGraph.xaml.cs
├── Resources
│   └── reg_flight_model.csv
└── Timeseries.cs
```

</p>
</details>

</p>
</details>
<details>
<summary>MinCircleDLL</summary>
<p>

```
├── AnomalyDetectionUtil.cs
├── IAbstractDetector.cs
├── MinCircleDetector.cs
├── MinCircleGraph.xaml
├── MinCircleGraph.xaml.cs
└── MinCircleViewModel.cs
```

</p>
</details>

Each component has its own folder, and implements the MVVM architecture approach.
The main components of the product are:
- **FG** for connecting to FlightGear and running the flight
- **DataModel** parses CSV data, analyzes and stores it in convenient data structures.
- **Player** allows the user to control the flight simulation presentation.
- **Steering** presents the joystick and moves it according to the aircraft's state.
- **Metadata** provides further information regarding the flight, such as speed and altitude.
- **Graph** calculates coorelations and presents graphs with flight data.
- **UserControls** contains all user defined controls.
- **InspectorWindow** displays all UserControls.
- **Plugins** for the anomaly-detection algorithms DLLs.

## 🔧 Technical Requirements
1. Download Visual Studio 2019 with .NET framework v4.7.2
2. Download Nuget package [XamlAnimatedGif v1.2.3.0](https://www.nuget.org/packages/XamlAnimatedGif/1.2.3)
## 📋 Installation Guide for Clean Environment
1. Download & install [FlightGear v2020.3.6](https://sourceforge.net/projects/flightgear/files/release-2020.3/FlightGear-2020.3.6.exe/download)
2. Install BlendWPFSDK_en.msi from the PreInstall folder.
3. Download Nuget package [XamlAnimatedGif v1.2.3.0](https://www.nuget.org/packages/XamlAnimatedGif/1.2.3)

⚠️ Notice: In case XamlAnimatedGif already exists in References after cloning, please uninstall and reinstall it again.

4. Run using the DLLs provided in Plugins folder.
## 📚 Further Documentation
For more info regarding the main classes of the project, information flow and UML diagrams, please refer to our [Wiki site](https://github.com/MatanNoach/FlightInspectionDesktopApp/wiki).
## 🎥 Demo
[Our demo video](https://www.youtube.com/watch?v=dzWjBTXAows)
