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

        public PointCollection GetRegPoints(string col, double margin, double height, double width) { return model.GetRegPoints(col, margin, height, width); }
        public PointCollection GetCorrelatedRegPoints(string col1, string col2) { model.GetCorrelatedRegPoints(col1, col2); return VMCorrelatedPoints; }

        public List<string> ColNames { get { return this.model.ColNames; } }

        public Dictionary<string, List<double>> MinMaxVals { get { return this.model.MinMaxVals; } }

        public int VMCurrentLineIndex { get { return model.CurrentLineIndex; } }

        public Dictionary<string, string> CorrData { get { return model.CorrData; } }

        public string VMCorrCol { get { return model.CorrCol; } set { model.CorrCol = value; } }

        public Dictionary<string, List<double>> LinRegData { get { return model.LinRegData; } }

        public PointCollection VMCorrelatedPoints { get { return model.CorrelatedPoints; } }
    }
}
