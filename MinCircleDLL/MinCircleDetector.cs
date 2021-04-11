using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MinCircleDLL
{
    public class Circle
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
        public Circle minCircle;
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
        public double X
        {
            get
            {
                return this.x;
            }
            set
            {
                this.x = value;
            }
        }
        public double Y
        {
            get
            {
                return this.y;
            }
            set
            {
                this.y = value;
            }
        }
        public bool IsDeviated
        {
            get
            {
                return this.isDeviated;
            }
        }
    }

    public class MinCircleDetector
    {
        Timeseries flightData;
        List<correlatedFeatures> cf;
        List<AnomalyReport> anomalies;
        static MinCircleDetector instance;
        Dictionary<string, List<double>> minMaxVals;
        private MinCircleDetector(string dataFile)
        {
            this.flightData = new Timeseries(dataFile);
            this.modelData = data;
            this.cf = new List<correlatedFeatures>();
            this.anomalies = new List<AnomalyReport>();
            this.minMaxVals = this.flightData.CalcMinMax();
        }
        public static void CreateMinCircleDetector(string csvFilePath)
        {
            if (instance != null)
            {
                throw new Exception("instance already created");
            }
            instance = new MinCircleDetector(csvFilePath);
        }
        public static MinCircleDetector GetInstance()
        {
            if (instance == null)
            {
                throw new Exception("instance not created");
            }
            return instance;
        }
        public Dictionary<string, List<double>> MinMaxVals
        {
            get
            {
                return this.minMaxVals;
            }
        }
        void learnNormal()
        {
            // read correlatedFeatures from resource csv
            using (var stream = Assembly
            .GetExecutingAssembly()
            .GetManifestResourceStream("MinCircleDLL.regFlightCsvModel.resources"))
            using (StreamReader csvReader = new StreamReader(stream))
            {
                // skip the first row, where the columns names are mentioned.
                string currLine = csvReader.ReadLine();
                string[] columns = { };
                List<string> lineData = new List<string>();
                char colSeparator = ',';

                // for each row in the csv file
                while ((currLine = csvReader.ReadLine()) != null)
                {
                    lineData = currLine.Split(colSeparator).Select(x => x.ToString()).ToList();

                    // if the new line is empty
                    if (lineData[0] == "")
                    {
                        break;
                    }

                    // parse the row into a correlatedFeatures object
                    correlatedFeatures corrFeature = new correlatedFeatures();
                    corrFeature.feature1 = lineData[0];
                    corrFeature.feature2 = lineData[1];
                    corrFeature.corrlation = double.Parse(lineData[2]);
                    corrFeature.threshold = double.Parse(lineData[3]);
                    corrFeature.minCircle = findMinCircle(this.modelData[corrFeature.feature1], this.modelData[corrFeature.feature2]);

                    Console.WriteLine(corrFeature.feature1);
                    Console.WriteLine(corrFeature.feature2);
                    Console.WriteLine(corrFeature.corrlation);
                    Console.WriteLine(corrFeature.threshold);
                    Console.WriteLine(corrFeature.minCircle.center.x);
                    Console.WriteLine(corrFeature.minCircle.center.y);
                    Console.WriteLine(corrFeature.minCircle.radius);
                    Console.WriteLine("----------");

                    this.cf.Add(corrFeature);
                }
            }
        }

        /// <summary>
        ///  this function finds a minimal circle from two lists of doubles (which will be converted into points)
        /// </summary>
        /// <param name="x"> the first list of doubles </param>
        /// <param name="y"> the second list of doubles </param>
        /// <param name="size"> the size of the lists </param>
        /// <returns> the minimal circle which can be drawn from the given points </returns>
        Circle findMinCircle(List<double> x, List<double> y)
        {
            List<Point> points = new List<Point>();
            int size = x.Count;

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

        /// <summary>
        ///  get anomalies timesteps of the given correlated features.
        /// </summary>
        /// <param name="feature"> the first feature </param>
        /// <param name="corrFeature"> the second feature </param>
        /// <returns> a list of timesteps </returns>
        List<long> getCorrelatedFeaturesAnomalies(string feature, string corrFeature)
        {
            List<long> indexes = new List<long>();

            for (int i = 0; i < this.anomalies.Count; i++)
            {
                if (this.anomalies[i].getFeature1() == feature && this.anomalies[i].getFeature2() == corrFeature)
                {
                    indexes.Add(this.anomalies[i].getTimestep());
                }
            }
            return indexes;
        }

        /// <summary>
        ///  get the correlated feature of the given feature.
        /// </summary>
        /// <param name="feature"> a feature name </param>
        /// <returns> the name of the correalted feature </returns>
        public string getCorrelatedFeatureByFeature(string feature)
        {
            return this.cf[getIndexByFeature(feature)].feature2;
        }

        /// <summary>
        ///  get the index of the feature in the cf list
        /// </summary>
        /// <param name="feature"> a feature name </param>
        /// <returns> an index of the feature in the cf list </returns>
        int getIndexByFeature(string feature)
        {
            for (int i = 0; i < this.cf.Count; i++)
            {
                if (this.cf[i].feature1 == feature)
                {
                    return i;
                }
            }
            return 0;
        }
        /// <summary>
        /// The function returns a correlation circle by feature name
        /// </summary>
        /// <param name="feature">The feature to get it's line</param>
        /// <returns>A circle object represnets the correlation between two features</returns>
        public Circle GetCorrCircleByFeature(string feature)
        {
            return this.cf[getIndexByFeature(feature)].minCircle;
        }
        /// <summary>
        ///  get the points which should be drawn for the given feature and it's correlated feature.
        /// </summary>
        /// <param name="feature"> a feature name </param>
        /// <returns> a list of points which should be drawn </returns>
        public List<DrawPoint> getPointsToDraw(string feature)
        {
            string corrFeature = this.getCorrelatedFeatureByFeature(feature);
            List<long> anomaliesIndexes = this.getCorrelatedFeaturesAnomalies(feature, corrFeature);

            List<DrawPoint> points = new List<DrawPoint>();
            Dictionary<string, List<double>> data = this.flightData.getData();
            int dataSize = this.flightData.getSizeOfLines();

            for (int i = 0; i < dataSize; i++)
            {
                points.Add(new DrawPoint(data[feature][i], data[corrFeature][i], anomaliesIndexes.Contains(i)));
            }

            return points;
        }
    }
}
