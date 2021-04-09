using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Media;

namespace FlightInspectionDesktopApp.Graph
{
    class GraphViewModel : INotifyPropertyChanged
    {
        GraphModel model;
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// GraphViewModel CTOR.
        /// </summary>
        /// <param name="model"></param>
        public GraphViewModel(GraphModel model)
        {
            this.model = model;
            // when a property in GraphModel changes, indicate it changed in GraphViewModel as well
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
        /// Returns points based on a given column.
        /// </summary>
        /// <param name="colName">XML column</param>
        /// <returns></returns>
        public PointCollection GetPointsByCol(string colName) { return this.model.GetPointsByCol(colName); }

        /// <summary>
        /// Returns two-points defining the linear regression line.
        /// </summary>
        /// <param name="col">chosen column</param>
        /// <param name="height">size of the canvas</param>
        /// <param name="width">size of the canvas</param>
        /// <returns></returns>
        public PointCollection GetLineRegPoints(string col, double height, double width) { return model.GetLineRegPoints(col, height, width); }

        /// <summary>
        /// Returns points of both correlated features from the last 30 seconds.
        /// </summary>
        /// <param name="col1">first feature</param>
        /// <param name="col2">second feature</param>
        /// <returns>point collection</returns>
        public PointCollection GetCorrelatedRegPoints(string col1, string col2) { model.GetCorrelatedRegPoints(col1, col2); return VMCorrelatedPoints; }

        // properties:
        public List<string> ColNames { get { return this.model.ColNames; } }

        public Dictionary<string, List<double>> MinMaxVals { get { return this.model.MinMaxVals; } }

        public int VMCurrentLineIndex { get { return model.CurrentLineIndex; } }

        public Dictionary<string, string> CorrData { get { return model.CorrData; } }

        public string VMCorrCol { get { return model.CorrCol; } set { model.CorrCol = value; } }

        public Dictionary<string, List<double>> LinRegData { get { return model.LinRegData; } }

        public PointCollection VMCorrelatedPoints { get { return model.CorrelatedPoints; } }
    }
}
