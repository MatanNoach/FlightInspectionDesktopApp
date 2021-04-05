using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Media;
using static System.Math;

namespace FlightInspectionDesktopApp.Graph
{
    class GraphModel : INotifyPropertyChanged
    {
        DataModel dm = DataModel.Instance;
        double height;
        double width;
        double stepX;
        double stepY;
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// GraphModel CTOR
        /// </summary>
        /// <param name="height">canvas height</param>
        /// <param name="width">canvas width</param>
        /// <param name="dataModel">datamodel instance</param>
        public GraphModel(double height, double width, DataModel dataModel)
        {
            this.height = height;
            this.width = width;
            this.stepX = this.width / dm.getDataSize();
            this.dm = dataModel;
            // when a property in GraphModel changes, indicate it
            dm.PropertyChanged += delegate (Object sender, PropertyChangedEventArgs e)
            {
                NotifyPropertyChanged(e.PropertyName);
            };

        }

        /// <summary>
        /// Creates points based on the chosen column's values.
        /// </summary>
        /// <param name="col">chosen property from the XML</param>
        /// <returns></returns>
        public PointCollection GetPointsByCol(string col)
        {
            // calculate the steps in the y axis
            this.stepY = this.height / 2.0 / Max(Abs(MinMaxVals[col][1]), Abs(MinMaxVals[col][0]));
            PointCollection points = new PointCollection();
            for (int x = 0; x <= dm.CurrentLineIndex; x++)
            {
                // create points in the ratios of the canvas
                points.Add(new Point(x * stepX, (this.height / 2) - (dm.getValueByKeyAndTime(col, x) * stepY)));
            }
            return points;
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

        public List<string> ColNames { get { return this.dm.ColNames; } }

        public Dictionary<string, List<double>> MinMaxVals { get { return dm.MinMaxVals; } }

        public int CurrentLineIndex
        { get { return dm.CurrentLineIndex; } }
    }
}
