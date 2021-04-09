using System.Collections.Generic;
using System.Windows.Media;
using static System.Math;

namespace LinearRegressionDLL
{
    class LinearGraphViewModel
    {
        LinearRegressionDetector model;
        List<DrawPoint> correlatedPoints;
        double xRegRatio, yRegRatio;
        public LinearGraphViewModel(LinearRegressionDetector model)
        {
            this.model = model;
        }
        public List<DrawPoint> VMCorrelatedPoints
        {
            get
            {
                return this.correlatedPoints;
            }
        }
        public void GetPointsByFeature(string feature, double height, double width)
        {
            this.correlatedPoints = model.getPointsToDraw(feature);
            foreach (DrawPoint point in correlatedPoints)
            {
                point.X = (width / 2) + point.X * xRegRatio;
                point.Y = (height / 2) - point.Y * yRegRatio;
            }
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
        public PointCollection GetLineRegPoints(string col, double height, double width)
        {

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
    }
}
