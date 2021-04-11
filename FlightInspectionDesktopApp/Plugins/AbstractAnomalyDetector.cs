using System;
using System.Reflection;
using System.Windows.Controls;

namespace FlightInspectionDesktopApp.Plugins
{
    class AbstractAnomalyDetector
    {
        // fields of AbstractAnomalyDetector object.
        Type type;
        UserControl detector;

        /// <summary>
        /// CTOR of AbstractAnomalyDetector object.
        /// </summary>
        /// <param name="csvFilePath"> a csv file which will be sent to the .dll file </param>
        /// <param name="dllPath"> a .dll file </param>
        public AbstractAnomalyDetector(string csvFilePath, string dllPath)
        {
            // load the dll file
            var DLL = Assembly.LoadFile(@dllPath);
            // for each type found in the dll
            foreach (Type type in DLL.GetExportedTypes())
            {
                // if the type implements IAbstractDetector interface
                if (type.GetInterface("IAbstractDetector") != null)
                {
                    try
                    {
                        // try to create an instance of the type object, and send csvFilePath to the constructor
                        var c = Activator.CreateInstance(type, new Object[] { csvFilePath });
                        //try to run GetUserControl function on object c, and send csvFilePath as argument
                        detector = (UserControl)type.InvokeMember("GetUserControl", BindingFlags.InvokeMethod, null, c, new object[] { csvFilePath });
                        // set the type
                        this.type = type;
                        // if everything succedded, break the loop
                        break;
                    }
                    catch { }

                }
            }
        }

        /// <summary>
        /// Property of field feature.
        /// </summary>
        public string Feature
        {
            // setter of feature.
            set
            {
                type.GetProperty("Feature").SetValue(detector, value);
            }
        }

        /// <summary>
        /// Property of field currentLineIndex.
        /// </summary>
        public int CurrentLineIndex
        {
            // setter of currentLineIndex.
            set
            {
                type.GetProperty("CurrentLineIndex").SetValue(detector, value);
            }
        }

        /// <summary>
        /// Property of detector.
        /// </summary>
        public UserControl Detector
        {
            // getter of detector.
            get
            {
                return this.detector;
            }
        }
    }
}
