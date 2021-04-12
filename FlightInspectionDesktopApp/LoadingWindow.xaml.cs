using System.IO;
using System.Windows;
using System.Threading;

namespace FlightInspectionDesktopApp
{
    /// <summary>
    /// Interaction logic for LoadingWindow.xaml
    /// </summary>
    public partial class LoadingWindow : Window
    {
        // The view model for the simulator
        FGViewModel vm;
        private string fileFG, fileXML, fileCSV, fileDLL;

        /// <summary>
        /// CTOR of LoadingWindow.
        /// </summary>
        /// <param name="fgFilePath"> flight gear file path </param>
        /// <param name="xmlFilePath"> the given xml file path </param>
        /// <param name="csvFilePath"> the given csv file path </param>
        /// <param name="dllFilePath"> the given dll file path </param>
        public LoadingWindow(string fgFilePath, string xmlFilePath, string csvFilePath, string dllFilePath)
        {
            InitializeComponent();
            fileFG = fgFilePath;
            fileXML = xmlFilePath;
            fileCSV = csvFilePath;
            fileDLL = dllFilePath;
        }
    }
}
