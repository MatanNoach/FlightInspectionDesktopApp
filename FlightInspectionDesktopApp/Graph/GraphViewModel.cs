using System;
using System.Windows.Media;
using System.ComponentModel;
using System.Collections.Generic;

namespace FlightInspectionDesktopApp.Graph
{
    class GraphViewModel : INotifyPropertyChanged
    {
        // fields of GraphViewModel object.
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

        /// <summary>
        /// Property of field ColNames.
        /// </summary>
        public List<string> ColNames
        {
            // getter of ColNames.
            get
            {
                return this.model.ColNames;
            }
        }

        /// <summary>
        /// Property of field MinMaxVals.
        /// </summary>
        public Dictionary<string, List<double>> MinMaxVals
        {
            // getter of MinMaxVals.
            get
            {
                return this.model.MinMaxVals;
            }
        }

        /// <summary>
        /// Property of CurrentLineIndex.
        /// </summary>
        public int VMCurrentLineIndex
        {
            // getter of CurrentLineIndex.
            get
            {
                return model.CurrentLineIndex;
            }
        }

        /// <summary>
        /// Property of CorrData.
        /// </summary>
        public Dictionary<string, string> CorrData
        {
            // getter of CorrData.
            get
            {
                return model.CorrData;
            }
        }

        /// <summary>
        /// Property of CorrCol.
        /// </summary>
        public string VMCorrCol
        {
            // getter of CorrCol.
            get
            {
                return model.CorrCol;
            }

            // setter of CorrCol.
            set
            {
                model.CorrCol = value;
            }
        }

        /// <summary>
        /// Property of LinRegData.
        /// </summary>
        public Dictionary<string, List<double>> LinRegData
        {
            // getter of LinRegData.
            get
            {
                return model.LinRegData;
            }
        }

        /// <summary>
        /// Property of CorrelatedPoints.
        /// </summary>
        public PointCollection VMCorrelatedPoints
        {
            // getter of CorrelatedPoints.
            get
            {
                return model.CorrelatedPoints;
            }
        }
    }
}
