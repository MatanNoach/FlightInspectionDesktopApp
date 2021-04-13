using static System.Math;
using System.ComponentModel;
using System.Collections.Generic;

namespace MinCircleDLL
{
    class MinCircleViewModel : INotifyPropertyChanged
    {
        int currentLineIndex;
        MinCircleDetector model;
        double xRegRatio, yRegRatio;
        List<DrawPoint> correlatedPoints;
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// CTOR of MinCircleVIewModel.
        /// </summary>
        /// <param name="model"> an object of MinCircleDetector </param>
        public MinCircleViewModel(MinCircleDetector model)
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
        /// The funcion loads a new set of points by a feature
        /// </summary>
        /// <param name="feature">The feature to load</param>
        /// <param name="height">The canvas's height</param>
        /// <param name="width">The canvas's width</param>
        public void LoadPointsByFeature(string feature, double height, double width)
        {
            List<DrawPoint> allPoints = model.getPointsToDraw(feature);
            List<DrawPoint> pointsToShow = new List<DrawPoint>();
            // find the minimum ratio and normalize by it
            double bestRatio = Min(xRegRatio, yRegRatio);
            for (int i = 0; i <= this.currentLineIndex; i++)
            {
                allPoints[i].X = (width / 2) + allPoints[i].X * bestRatio;
                allPoints[i].Y = (height / 2) - allPoints[i].Y * bestRatio;
                pointsToShow.Add(allPoints[i]);
            }
            VMCorrelatedPoints = pointsToShow;
        }

        /// <summary>
        /// Property of field minMaxVals.
        /// </summary>
        public Dictionary<string, List<double>> MinMaxVals
        {
            // getter of minMaxVals.
            get
            {
                return model.MinMaxVals;
            }
        }

        /// <summary>
        /// Returns two-points defining the correlation circle.
        /// </summary>
        /// <param name="col">chosen column</param>
        /// <param name="height">size of the canvas</param>
        /// <param name="width">size of the canvas</param>
        /// <returns></returns>
        public Circle GetCorrCircle(string col, double height, double width)
        {
            // calculate min & max values of correlated features
            double minXVal = MinMaxVals[col][0];
            double maxXVal = MinMaxVals[col][1];
            double absMaxXVal = Max(Abs(minXVal), Abs(maxXVal));
            double minYVal = MinMaxVals[model.getCorrelatedFeatureByFeature(col)][0];
            double maxYVal = MinMaxVals[model.getCorrelatedFeatureByFeature(col)][1];
            double absMaxYVal = Max(Abs(maxYVal), Abs(minYVal));
            // the min circle from the model
            Circle minCircle = model.GetCorrCircleByFeature(col);
            //The function copies the data from the minCircle
            Circle testCircle = new Circle(new Point(minCircle.center.x, minCircle.center.y), minCircle.radius);
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
            // find the minimum ratio and normalize by it
            double bestRatio = Min(xRegRatio, yRegRatio);
            testCircle.center.x *= bestRatio;
            testCircle.center.y *= bestRatio;
            testCircle.radius *= bestRatio;
            return testCircle;
        }

        /// <summary>
        /// The function updates the current line index and loads more points to present 
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
        /// <summary>
        /// The function returns the correlation circle by a certain feature
        /// </summary>
        /// <param name="feature"></param>
        /// <returns></returns>
        public Circle GetCorrCircleByFeature(string feature)
        {
            return this.model.GetCorrCircleByFeature(feature);
        }
    }
}
