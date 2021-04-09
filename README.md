# FlightInspectionDesktopApp
## 1) Overview
FlightInspectionDesktopApp connects to FlightGear and displays given flight data in a nice graphic way.
It detects anomalies based on dynamically-loaded anomaly-detection algorithms and displays them.
### Special Features
- The program validates the user's input files.
- FlightGear starts up on its own and start flying, without the user having to configure any settings. Closing the Inspector window closes FlightGear automatically.
- FlightGear window and the program's Inspector open automatically side by side, matching the user's screen size.
- ...
## 2) Project Hierarchy
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


## 3) Technical Requirements
...
## 4) Installation Guide for Clean Environment
1. Download FlightGear from https://www.flightgear.org/download/
2. ...
## 5) Further Documentation
![alternative text](http://www.plantuml.com/plantuml/proxy?cache=no&src=https://raw.github.com/plantuml/plantuml-server/master/src/main/webapp/resource/test2diagrams.txt)
## 6) Demo
...
