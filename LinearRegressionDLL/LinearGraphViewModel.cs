using System;
using static System.Math;
using System.Windows.Media;
using System.ComponentModel;
using System.Collections.Generic;

namespace LinearRegressionDLL
{
    class LinearGraphViewModel : INotifyPropertyChanged
    {
        LinearRegressionDetector model;
        List<DrawPoint> correlatedPoints;
        double xRegRatio, yRegRatio;
        int currentLineIndex;

        public event PropertyChangedEventHandler PropertyChanged;

        public LinearGraphViewModel(LinearRegressionDetector model)
        {
            this.model = model;
        }
        /// <summary>
        /// The function notifes a certain property change by it's name
        /// </summary>
        /// <param name="propName">The properite's name</param>
        public void NotifyPropertyChanged(string propName)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propName));
            }
        }

        /// <summary>
        /// Property of field correlatedPoints.
        /// </summary>
        public List<DrawPoint> VMCorrelatedPoints
        {
            // getter of correlatedPoints.
            get
            {
                return this.correlatedPoints;
            }

            // setter of correlatedPoints.
            set
            {
                this.correlatedPoints = value;
                NotifyPropertyChanged("VMCorrelatedPoints");
            }
        }

        /// <summary>
        /// this function loads all the points which should be drawn for the given feature and it's miost correlated feature.
        /// </summary>
        /// <param name="feature"> name of the feature </param>
        /// <param name="height"> the height of the canvas </param>
        /// <param name="width"> the width of the canvas </param>
        public void LoadPointsByFeature(string feature, double height, double width)
        {
            List<DrawPoint> allPoints = model.getPointsToDraw(feature);
            List<DrawPoint> pointsToShow = new List<DrawPoint>();
            for (int i = 0; i <= this.currentLineIndex; i++)
            {
                allPoints[i].X = (width / 2) + allPoints[i].X * xRegRatio;
                allPoints[i].Y = (height / 2) - allPoints[i].Y * yRegRatio;
                pointsToShow.Add(allPoints[i]);
            }
            VMCorrelatedPoints = pointsToShow;
        }

        /// <summary>
        /// Property of field MinMaxVals.
        /// </summary>
        public Dictionary<string, List<double>> MinMaxVals
        {
            // getter of MinMaxVals.
            get
            {
                return model.MinMaxVals;
            }
        }

        /// <summary>
        /// Returns two-points defining the linear regression line.
        /// </summary>
        /// <param name="col">chosen column</param>
        /// <param name="height">the height of the canvas</param>
        /// <param name="width">the width of the canvas</param>
        /// <returns></returns>
        public PointCollection GetLineRegPoints(string col, double height, double width)
        {
            Console.WriteLine(col);
            Console.WriteLine(model.getCorrelatedFeatureByFeature(col));
            // calculate min & max values of correlated features
            double minXVal = MinMaxVals[col][0];
            double maxXVal = MinMaxVals[col][1];
            double absMaxXVal = Max(Abs(minXVal), Abs(maxXVal));
            double minYVal = MinMaxVals[model.getCorrelatedFeatureByFeature(col)][0];
            double maxYVal = MinMaxVals[model.getCorrelatedFeatureByFeature(col)][1];
            double absMaxYVal = Max(Abs(maxYVal), Abs(minYVal));
            List<double> l = model.GetLineByFeature(col);
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
                System.Windows.Point p = new System.Windows.Point(minXVal * xRegRatio + (width / 2), (height / 2) - CalcY(minXVal, l) * yRegRatio);

                points.Add(p);
            }
            else
            {
                System.Windows.Point p = new System.Windows.Point((width / 2) + CalcX(minYVal, l) * xRegRatio, (height / 2) - minYVal * yRegRatio);
                points.Add(p);
            }
            if (maxXVal < maxYVal)
            {
                System.Windows.Point p = new System.Windows.Point(maxXVal * xRegRatio + (width / 2), (height / 2) - CalcY(maxXVal, l) * yRegRatio);
                points.Add(p);
            }
            else
            {
                System.Windows.Point p = new System.Windows.Point((width / 2) + CalcX(maxYVal, l) * xRegRatio, (height / 2) - maxYVal * yRegRatio);
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
        /// The function updates the line index and the correlated points
        /// </summary>
        /// <param name="newLine">The new line index</param>
        /// <param name="feature">The feature to show it's points</param>
        /// <param name="height">The canvas height's </param>
        /// <param name="width">The canvas width's </param>
        public void UpdateCurrentLineIndex(int newLine, string feature, double height, double width)
        {
            this.currentLineIndex = newLine;
            LoadPointsByFeature(feature, height, width);
        }
    }
}
