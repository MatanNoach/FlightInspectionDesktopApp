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


        public PointCollection GetCorrelatedRegPoints(string col1, string col2)
        {
            PointCollection points = new PointCollection();
            for (int x = 0; x <= dm.CurrentLineIndex; x++)
            {
                // create points in the ratios of the canvas
                points.Add(new Point((width / 2) + dm.getValueByKeyAndTime(col1, x) * xRegRatio, (this.height / 2) - (dm.getValueByKeyAndTime(col2, x) * yRegRatio)));
            }
            CorrelatedPoints = points;
            return points;
        }

        public PointCollection GetRegPoints(string col, double margin, double height, double width)
        {
            double minXVal = MinMaxVals[col][0];
            double maxXVal = MinMaxVals[col][1];
            double absMaxXVal = Max(Abs(minXVal), Abs(maxXVal));
            double minYVal = MinMaxVals[dm.CorrData[col]][0];
            double maxYVal = MinMaxVals[dm.CorrData[col]][1];
            double absMaxYVal = Max(Abs(maxYVal), Abs(minYVal));
            List<double> l = dm.LinRegData[col];
            PointCollection points = new PointCollection();
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
            if (minXVal > minYVal)
            {
                points.Add(new Point(minXVal * xRegRatio, CalcY(height, minXVal * xRegRatio, l)));
            }
            else
            {
                points.Add(new Point(CalcX(width, minYVal, l), minYVal * yRegRatio));
            }
            if (maxXVal < maxYVal)
            {
                points.Add(new Point(maxXVal * xRegRatio, CalcY(height, maxXVal * xRegRatio, l)));
            }
            else
            {
                points.Add(new Point(CalcX(width, maxYVal, l), maxYVal * yRegRatio));
            }
            return points;
        }

        private double CalcX(double width, double y, List<double> l)
        {
            return (width / 2) + ((y - l[1]) / l[0]);
        }

        private double CalcY(double height, double x, List<double> l)
        {
            return (height / 2) - ((l[0] * x + l[1]));
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
