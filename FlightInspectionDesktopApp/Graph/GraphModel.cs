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
        double margin;
        double xRegRatio;
        double yRegRatio;
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// GraphModel CTOR
        /// </summary>
        /// <param name="height">canvas height</param>
        /// <param name="width">canvas width</param>
        /// <param name="dataModel">datamodel instance</param>
        public GraphModel(double height, double width, double margin, DataModel dataModel)
        {
            this.height = height;
            this.width = width;
            this.margin = margin;
            this.stepX = (this.width - margin) / dm.getDataSize();
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
            double maxVal = Max(Abs(MinMaxVals[col][1]), Abs(MinMaxVals[col][0]));
            if (maxVal == 0)
            {
                stepY = 0;
            }
            else
            {
                this.stepY = this.height / 2.0 / maxVal;
            }
            PointCollection points = new PointCollection();
            for (int x = 0; x <= dm.CurrentLineIndex; x++)
            {
                // create points in the ratios of the canvas
                points.Add(new Point(x * stepX + margin, (this.height / 2) - (dm.getValueByKeyAndTime(col, x) * stepY)));
            }
            return points;
        }

        /// <summary>
        /// Returns points of both correlated features from the last 30 seconds.
        /// </summary>
        /// <param name="col1">first feature</param>
        /// <param name="col2">second feature</param>
        /// <returns>point collection</returns>
        public PointCollection GetCorrelatedRegPoints(string col1, string col2)
        {
            PointCollection points = new PointCollection();
            // add points from the last 30 seconds
            for (int x = Max(dm.CurrentLineIndex - 300, 0); x <= dm.CurrentLineIndex; x++)
            {
                // create points in the ratios of the canvas
                Point p = new Point((width / 2) + dm.getValueByKeyAndTime(col1, x) * xRegRatio, (this.height / 2) - (dm.getValueByKeyAndTime(col2, x) * yRegRatio));
                points.Add(p);
            }
            CorrelatedPoints = points;
            return points;
        }

        /// <summary>
        /// Returns two-points defining the linear regression line.
        /// </summary>
        /// <param name="col">chosen column</param>
        /// <param name="height">size of the canvas</param>
        /// <param name="width">size of the canvas</param>
        /// <returns></returns>
        public PointCollection GetLineRegPoints(string col, double height, double width)
        {
            // calculate min & max values of correlated features
            double minXVal = MinMaxVals[col][0];
            double maxXVal = MinMaxVals[col][1];
            double absMaxXVal = Max(Abs(minXVal), Abs(maxXVal));
            double minYVal = MinMaxVals[dm.CorrData[col]][0];
            double maxYVal = MinMaxVals[dm.CorrData[col]][1];
            double absMaxYVal = Max(Abs(maxYVal), Abs(minYVal));
            List<double> l = dm.LinRegData[col];
            PointCollection points = new PointCollection();
            // calculate the desirable ratios of x & y axes
            if (absMaxXVal == 0)
            {
                xRegRatio = 0;
            }
            else
            {
                xRegRatio = (width / 2) / absMaxXVal;
            }
            if (absMaxYVal == 0)
            {
                yRegRatio = 0;
            }
            else
            {
                yRegRatio = (height / 2) / absMaxYVal;
            }
            // create two points defining the linear regression line, trying to draw in the scope of the canvas
            if (minXVal > minYVal)
            {
                Point p = new Point(minXVal * xRegRatio + (width / 2), (height / 2) - CalcY(minXVal, l) * yRegRatio);
                points.Add(p);
            }
            else
            {
                Point p = new Point((width / 2) + CalcX(minYVal, l) * xRegRatio, (height / 2) - minYVal * yRegRatio);
                points.Add(p);
            }
            if (maxXVal < maxYVal)
            {
                Point p = new Point(maxXVal * xRegRatio + (width / 2), (height / 2) - CalcY(maxXVal, l) * yRegRatio);
                points.Add(p);
            }
            else
            {
                Point p = new Point((width / 2) + CalcX(maxYVal, l) * xRegRatio, (height / 2) - maxYVal * yRegRatio);
                points.Add(p);
            }
            return points;
        }

        /// <summary>
        /// Calculate the value of x given y.
        /// </summary>
        /// <param name="y">value of y</param>
        /// <param name="l"></param>
        /// <returns></returns>
        private double CalcX(double y, List<double> l)
        {
            return ((y - l[1]) / l[0]);
        }

        /// <summary>
        /// Calculate the value of y given x.
        /// </summary>
        /// <param name="x">value of x</param>
        /// <param name="l">line equasion</param>
        /// <returns></returns>
        private double CalcY(double x, List<double> l)
        {
            return ((l[0] * x + l[1]));
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

        public int CurrentLineIndex { get { return dm.CurrentLineIndex; } }

        public Dictionary<string, string> CorrData { get { return dm.CorrData; } }

        private string corrCol;
        public string CorrCol { get { return corrCol; } set { corrCol = value; NotifyPropertyChanged("CorrCol"); } }

        public Dictionary<string, List<double>> LinRegData { get { return dm.LinRegData; } }

        private PointCollection correlatedPoints;
        public PointCollection CorrelatedPoints { get { return correlatedPoints; } set { correlatedPoints = value; NotifyPropertyChanged("CorrelatedPoints"); } }
    }
}
