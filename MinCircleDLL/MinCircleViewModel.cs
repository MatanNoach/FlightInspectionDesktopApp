using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using static System.Math;

namespace MinCircleDLL
{
    class MinCircleViewModel : INotifyPropertyChanged
    {
        MinCircleDetector model;
        List<DrawPoint> correlatedPoints;
        double xRegRatio, yRegRatio;
        int currentLineIndex;
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
        public event PropertyChangedEventHandler PropertyChanged;
        public List<DrawPoint> VMCorrelatedPoints
        {
            get
            {
                return this.correlatedPoints;
            }
            set
            {
                this.correlatedPoints = value;
                NotifyPropertyChanged("VMCorrelatedPoints");
            }
        }
        public void LoadPointsByFeature(string feature, double height, double width)
        {
            List<DrawPoint> allPoints = model.getPointsToDraw(feature);
            List<DrawPoint> pointsToShow = new List<DrawPoint>();
            double bestRatio = Min(xRegRatio, yRegRatio);
            for (int i = 0; i <= this.currentLineIndex; i++)
            {
                allPoints[i].X = (width / 2) + allPoints[i].X * bestRatio;
                allPoints[i].Y = (height / 2) - allPoints[i].Y * bestRatio;
                pointsToShow.Add(allPoints[i]);
            }
            VMCorrelatedPoints = pointsToShow;
        }
        public Dictionary<string, List<double>> MinMaxVals
        {
            get
            {
                return model.MinMaxVals;
            }
        }
        /// <summary>
        /// Returns two-points defining the linear regression line.
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
            Circle minCircle = model.GetCorrCircleByFeature(col);
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
            double bestRatio = Min(xRegRatio, yRegRatio);
            minCircle.center.x *= bestRatio;
            minCircle.center.y *= bestRatio;
            minCircle.radius *= bestRatio;
            return minCircle;
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
        public Circle GetCorrCircleByFeature(string feature)
        {
            return this.model.GetCorrCircleByFeature(feature);
        }
    }
}
