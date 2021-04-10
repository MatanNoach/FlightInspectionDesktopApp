﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Reflection;

namespace FlightInspectionDesktopApp.Plugins
{
    class AbstractAnomalyDetector
    {
        UserControl detector;
        Type type;
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
        public string Feature
        {
            set
            {
                type.GetProperty("Feature").SetValue(detector, value);
            }
        }
        public int CurrentLineIndex
        {
            set
            {
                type.GetProperty("CurrentLineIndex").SetValue(detector, value);
            }
        }
        public UserControl Detector
        {
            get
            {
                return this.detector;
            }
        }
    }
}