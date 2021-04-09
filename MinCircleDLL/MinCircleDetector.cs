using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinCircleDLL
{
    class Circle
    {
        public Point center;
        public double radius;

        /// <summary>
        /// default CTOR of Circle
        /// </summary>
        public Circle()
        {
            this.center = new Point(0, 0);
            this.radius = 0;
        }

        /// <summary>
        /// CTOR of circle
        /// </summary>
        /// <param name="c"> the center point of the circle </param>
        /// <param name="r"> the radius of the circle </param>
        public Circle(Point c, double r)
        {
            this.center = c;
            this.radius = r;
        }
    }

    class correlatedFeatures
    {
        public string feature1;
        public string feature2;
        public Line regression_line;
        public double corrlation;
        public double threshold;

        /// <summary>
        /// CTOR of correlatedFeatures
        /// </summary>
        public correlatedFeatures() { }
    }

    class AnomalyReport
    {
        string feature1;
        string feature2;
        long timestep;

        /// <summary>
        /// CTOR of AnomalyReport
        /// </summary>
        /// <param name="feat1"> the first feature </param>
        /// <param name="feat2"> the second feature </param>
        /// <param name="time"> the row number in the csv file </param>
        public AnomalyReport(string feat1, string feat2, long time)
        {
            this.feature1 = feat1;
            this.feature2 = feat2;
            this.timestep = time;
        }

        /// <summary>
        /// getter for the first feature attribute.
        /// </summary>
        /// <returns> the first feature value </returns>
        public string getFeature1()
        {
            return this.feature1;
        }

        /// <summary>
        ///  getter for the second feature attribute.
        /// </summary>
        /// <returns> the second feature value </returns>
        public string getFeature2()
        {
            return this.feature2;
        }

        /// <summary>
        ///  getter for the timestep of the anomaly.
        /// </summary>
        /// <returns> the timestep of the anomaly </returns>
        public long getTimestep()
        {
            return this.timestep;
        }
    }

    public class DrawPoint
    {
        double x;
        double y;
        bool isDeviated;

        /// <summary>
        /// CTOR of the DrawPoint object
        /// </summary>
        /// <param name="x"> x value </param>
        /// <param name="y"> y value </param>
        /// <param name="status"> is the point deviated </param>
        public DrawPoint(double x, double y, bool status)
        {
            this.x = x;
            this.y = y;
            this.isDeviated = status;
        }
    }

    public class MinCircleDetector
    {
        Timeseries flightData;
        List<correlatedFeatures> cf;
        List<AnomalyReport> anomalies;

        public MinCircleDetector(string dataFile)
        {
            this.flightData = new Timeseries(dataFile);
            this.cf = new List<correlatedFeatures>();
            this.anomalies = new List<AnomalyReport>();
        }

        void learnNormal()
        {

        }

        /// <summary>
        ///  this function finds a minimal circle from two lists of doubles (which will be converted into points)
        /// </summary>
        /// <param name="x"> the first list of doubles </param>
        /// <param name="y"> the second list of doubles </param>
        /// <param name="size"> the size of the lists </param>
        /// <returns> the minimal circle which can be drawn from the given points </returns>
        Circle findMinCircle(List<double> x, List<double> y, int size)
        {
            List<Point> points = new List<Point>();

            for (int i = 0; i < size; i++)
            {
                points.Add(new Point(x[i], y[i]));
            }
            return findMinCircle(points);
        }

        /// <summary>
        ///  this function finds a minimcal circle from a list of points.
        /// </summary>
        /// <param name="points"> a list of points </param>
        /// <returns> the minimal circle which can be drawn from the given points </returns>
        Circle findMinCircle(List<Point> points)
        {
            List<Point> welzlPoints = new List<Point>();
            return welzlAlgorithm(points, welzlPoints, points.Count);
        }

        /// <summary>
        ///  this recursive function chooses a random point, swap it with another point, and tries to find the minimal circle which can be drawn from a smaller of points.
        /// </summary>
        /// <param name="points"> a list of points </param>
        /// <param name="welzlPoints"> an inner list of points, which is in use by the recursive function </param>
        /// <param name="size"> the size of the points' list </param>
        /// <returns> the minimal circle which can be drawn </returns>
        Circle welzlAlgorithm(List<Point> points, List<Point> welzlPoints, int size)
        {
            if (size == 0 || welzlPoints.Count == 3)
            {
                return trivialMinCircle(welzlPoints);
            }

            // find a random point and swap it with another point in the list
            int num = new Random().Next() % size;
            Point randomPoint = points[num];
            // swap
            (points[num], points[size - 1]) = (points[size - 1], points[num]);

            // find the minimal circle which can be drawn from a smaller list of points
            size--;
            Circle minCircle = welzlAlgorithm(points, welzlPoints, size);

            if (distance(randomPoint, minCircle.center) <= minCircle.radius)
            {
                return minCircle;
            }

            welzlPoints.Add(randomPoint);
            return welzlAlgorithm(points, welzlPoints, size);
        }

        /// <summary>
        ///  this function finds the minimal circle from a given list of points, which contains 0,1,2,3 points.
        /// </summary>
        /// <param name="points"> a list of points </param>
        /// <returns> the minimal circle which can be drawn from the given list of points </returns>
        Circle trivialMinCircle(List<Point> points)
        {
            int radius = 0;
            switch (points.Count)
            {
                // return trivial point
                case 0: return new Circle(new Point(0, 0), radius);
                // return the point as the center of the circle, with radius of 0
                case 1: return new Circle(points[0], radius);
                case 2: return minCircleFrom2Points(points[0], points[1]);
                default: break;
            }

            // try to find the minimal circle from all combinations of 2 points
            for (int i = 0; i < 2; i++)
            {
                for (int j = i + 1; j <= 2; j++)
                {
                    Circle minCircle = minCircleFrom2Points(points[i], points[j]);
                    int k = 3 - (i + j);
                    if (distance(points[k], minCircle.center) <= minCircle.radius)
                    {
                        return minCircle;
                    }
                }
            }

            // find the minimal circle from a given list of 3 points
            return minCircleFrom3Points(points);
        }

        /// <summary>
        ///  this function find the minimal circle which can be drawn from a given two points.
        /// </summary>
        /// <param name="p1"> the first point </param>
        /// <param name="p2"> the second point </param>
        /// <returns> the minimal circle which can be drawn from the given points </returns>
        Circle minCircleFrom2Points(Point p1, Point p2)
        {
            double x = (p1.x + p2.x) / 2;
            double y = (p1.y + p2.y) / 2;
            double radius = distance(p1, p2) / 2;
            return new Circle(new Point(x, y), radius);
        }

        /// <summary>
        ///  this function finds the minimal circle which can be drawn from the given list of points, which contains 3 points.
        /// </summary>
        /// <param name="points"> a list of 3 points </param>
        /// <returns> the minimal circle which can be drawn from the given list of points </returns>
        Circle minCircleFrom3Points(List<Point> points)
        {
            Point p1 = points[0];
            Point p2 = points[1];
            Point p3 = points[2];

            // calculate the properties of the first line, which is built from the first and the second points
            Point midPoint1 = getLineMidPoint(p1, p2);
            double slope1 = getLineSlope(p1, p2);
            double pSlop1 = -(1 / slope1);

            // calculate the properties of the second line, which is built from the second and the third points
            Point midPoint2 = getLineMidPoint(p2, p3);
            double slope2 = getLineSlope(p2, p3);
            double pSlop2 = -(1 / slope2);

            // find the circle from the calculated lines
            double circleX = ((-pSlop2 * midPoint2.x + midPoint2.y) + pSlop1 * midPoint1.x - midPoint1.y) / (pSlop1 - pSlop2);
            double circleY = pSlop1 * (circleX - midPoint1.x) + midPoint1.y;
            Point circleCenter = new Point(circleX, circleY);
            double radius = distance(circleCenter, p1);

            return new Circle(circleCenter, radius);
        }

        /// <summary>
        ///  this function returns the distance from a given 2 points.
        /// </summary>
        /// <param name="p1"> the first point </param>
        /// <param name="p2"> the second point </param>
        /// <returns> the distance between the points </returns>
        double distance(Point p1, Point p2)
        {
            double x = p1.x - p2.x;
            double y = p1.y - p2.y;
            return Math.Sqrt(Math.Pow(x, 2) + Math.Pow(y, 2));
        }

        /// <summary>
        ///  this function return the middle of the line which is drawn from the given points.
        /// </summary>
        /// <param name="p1"> the first point </param>
        /// <param name="p2"> the second point </param>
        /// <returns> the middle point </returns>
        Point getLineMidPoint(Point p1, Point p2)
        {
            double x = p1.x + p2.x;
            double y = p1.y + p2.y;
            return new Point(x, y);
        }

        /// <summary>
        ///  this function returns the slope of the line, which is drawn from the given points.
        /// </summary>
        /// <param name="p1"> the first point </param>
        /// <param name="p2"> the second point </param>
        /// <returns> the slope of the line </returns>
        double getLineSlope(Point p1, Point p2)
        {
            return ((p2.y - p1.y) / (p2.x - p1.x));
        }
    }
}