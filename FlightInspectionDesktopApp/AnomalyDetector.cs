using System;
using System.Runtime.InteropServices;

namespace FlightInspectionDesktopApp
{
    class AnomalyDetector
    {
        [DllImport("LinearRegressionAnomalyDetector.dll", EntryPoint = "TimeSeriesCaller")]
        public static extern IntPtr CreateTimeSeries(string csvFileName);
    }
}
