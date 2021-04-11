using System.Windows.Controls;

namespace LinearRegressionDLL
{
    /// <summary>
    /// this interface should be implemented in this .DLL in order to load it properly in FlightInspectionDesktopApp application
    /// </summary>
    interface IAbstractDetector
    {
        UserControl GetUserControl(string csvFileName);
        string Feature { get; set; }
        int CurrentLineIndex { set; }
    }
}
