using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;

namespace MinCircleDLL
{
    public class Circle
    {
        // fields of Circle object
        public Point center;
        public double radius;

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

    class CorrelatedFeatures
    {
        // fields of CorrelatedFeatures object
        public string feature1;
        public string feature2;
        public Circle minCircle;
        public double corrlation;
        public double threshold;

        /// <summary>
        /// CTOR of CorrelatedFeatures
        /// </summary>
        public CorrelatedFeatures() { }
    }

    class AnomalyReport
    {
        // fields of AnomalyReport object
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
        // fields of DrawPoint object
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

        /// <summary>
        /// Property that represenets field x
        /// </summary>
        public double X
        {
            // getter of x
            get
            {
                return this.x;
            }

            // setter of x
            set
            {
                this.x = value;
            }
        }

        /// <summary>
        /// Property that represents field y
        /// </summary>
        public double Y
        {
            // getter of y
            get
            {
                return this.y;
            }

            // setter of y
            set
            {
                this.y = value;
            }
        }

        /// <summary>
        /// Property that represents field isDeviated
        /// </summary>
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
        // fields of MinCircleDetector object
        Timeseries flightData;
        List<CorrelatedFeatures> cf;
        List<AnomalyReport> anomalies;
        static MinCircleDetector instance;
        Dictionary<string, List<double>> minMaxVals;

        /// <summary>
        /// private CTOR for MinCircleDetector object (as singleton), that parses the model and detects anomalies in dataFile
        /// </summary>
        /// <param name="dataFile"></param>
        private MinCircleDetector(string dataFile)
        {
            this.flightData = new Timeseries(dataFile);
            this.cf = new List<CorrelatedFeatures>();
            this.anomalies = new List<AnomalyReport>();

            this.learnNormal();
            this.detect();
            this.minMaxVals = this.flightData.CalcMinMax();
        }

        /// <summary>
        /// a static function that creates an instance of MinCircleDetector object (if it was not created before)
        /// </summary>
        /// <param name="csvFilePath"> a csv file where the flight's data is </param>
        public static void CreateMinCircleDetector(string csvFilePath)
        {
            if (instance != null)
            {
                throw new Exception("instance already created");
            }
            instance = new MinCircleDetector(csvFilePath);
        }

        /// <summary>
        /// a static function that returns the single instance of MinCircleDetector object
        /// </summary>
        /// <returns></returns>
        public static MinCircleDetector GetInstance()
        {
            if (instance == null)
            {
                throw new Exception("instance not created");
            }
            return instance;
        }

        /// <summary>
        /// Property that represents field minMaxVals
        /// </summary>
        public Dictionary<string, List<double>> MinMaxVals
        {
            // getter of minMaxVals
            get
            {
                return this.minMaxVals;
            }
        }

        /// <summary>
        /// this function parses resource csv file in order to set list of CorrelatedFeatures.
        /// </summary>
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
                    CorrelatedFeatures corrFeature = new CorrelatedFeatures();
                    corrFeature.feature1 = lineData[0];
                    corrFeature.feature2 = lineData[1];
                    corrFeature.corrlation = double.Parse(lineData[2]);
                    corrFeature.threshold = double.Parse(lineData[3]);
                    corrFeature.minCircle = new Circle(new Point(double.Parse(lineData[4]), double.Parse(lineData[5])), double.Parse(lineData[6]));

                    this.cf.Add(corrFeature);
                }
            }
        }

        /// <summary>
        ///  this function detects anomalies from the given flight's data as a result of the CorrelatedFeatures.
        /// </summary>        
        void detect()
        {
            Dictionary<string, List<double>> data = this.flightData.getData();
            int dataSize = this.flightData.getSizeOfLines();

            foreach (CorrelatedFeatures feature in this.cf)
            {
                // look for deviations between the values of the correlated features
                for (int i = 0; i < dataSize; i++)
                {
                    Point colPoint = new Point(data[feature.feature1][i], data[feature.feature2][i]);
                    // if a deviation was detected
                    if (detectedDev(feature, colPoint))
                    {
                        // add the current line as an AnomalyReport
                        this.anomalies.Add(new AnomalyReport(feature.feature1, feature.feature2, i + 1));
                    }
                }
            }
        }

        /// <summary>
        ///  this function checks if there is a significant deviation between the given point and the regression line of the correalted features.
        /// </summary>
        /// <param name="corrFeature"> a correlated feature </param>
        /// <param name="p"> a point </param>
        /// <returns> true - if there is a significant deviation, false if not.  </returns>
        bool detectedDev(CorrelatedFeatures corrFeature, Point p)
        {
            return (corrFeature.minCircle.radius * 1.1 < distance(corrFeature.minCircle.center, p));
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
        ///  this function returns a list of anomalies' timesteps for the given correlated features.
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
        ///  this function returns the correlated feature of the given feature.
        /// </summary>
        /// <param name="feature"> a feature name </param>
        /// <returns> the name of the correalted feature </returns>
        public string getCorrelatedFeatureByFeature(string feature)
        {
            return this.cf[getIndexByFeature(feature)].feature2;
        }

        /// <summary>
        ///  this function returns the index of the feature in the cf list
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
        ///  this function returns a list of points which should be drawn for the given feature and it's correlated feature.
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
        public Circle GetCorrCircleByFeature(string feature)
        {
            return this.cf[getIndexByFeature(feature)].minCircle;
        }
    }
}
