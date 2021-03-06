using System;
using System.Threading;
using System.ComponentModel;

namespace FlightInspectionDesktopApp
{
    public class FGViewModel : INotifyPropertyChanged
    {
        // fields of FGViewModel
        private IFGModel model;
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// FGViewModel Constructor.
        /// </summary>
        /// <param name="model">FGModel</param>
        public FGViewModel(IFGModel model)
        {
            this.model = model;
            // when a property in FGModel changes, indicate it changed in FGViewModel as well
            model.PropertyChanged += delegate (Object sender, PropertyChangedEventArgs e)
             {
                 NotifyPropertyChanged("VM" + e.PropertyName);
             };
        }

        /// <summary>
        /// Evokes all subscribed methods of PropertyChanged.
        /// </summary>
        /// <param name="propName">name of the property that's been changed</param>
        public void NotifyPropertyChanged(string propName)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propName));
            }
        }

        /// <summary>
        /// Runs the program autonomously by opening FG with specific configurations, connecting to it via sockets, 
        /// </summary>
        /// <param name="binFolder">location of fgfs.exe</param>
        /// <param name="PathFG">full path of fgfs.exe</param>
        /// <param name="XMLFileName">settings file's name</param>        
        public void Run(string binFolder, string PathFG, string XMLFileName)
        {
            model.RunFG(binFolder, PathFG, XMLFileName);
            // wait 10 seconds before trying to connect to FG
            Thread.Sleep(10000);
            model.Connect();
        }

        /// <summary>
        /// this function starts our application.
        /// </summary>
        /// <param name="PathCSV">path of CSV file</param>
        public void Start(string PathCSV)
        {
            model.Start(PathCSV);
        }

        /// <summary>
        /// Disconnects all connections.
        /// </summary>
        public void Disconnect()
        {
            model.Disconnect();
        }
    }
}
