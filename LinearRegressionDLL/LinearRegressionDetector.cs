using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;

namespace LinearRegressionDLL
{
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

    public class LinearRegressionDetector
    {
        Timeseries flightData;
        List<correlatedFeatures> cf;
        List<AnomalyReport> anomalies;
        private const double significantCorrelation = 0.9;
        static LinearRegressionDetector instance;
        Dictionary<string, List<double>> minMaxVals;

        /// <summary>
        /// CTOR of LinearRegressionDetector
        /// </summary>
        /// <param name="dataFile"> a csv file with the data of the current flight </param>
        private LinearRegressionDetector(string dataFile)
        {
            this.flightData = new Timeseries(dataFile);
            this.cf = new List<correlatedFeatures>();
            this.anomalies = new List<AnomalyReport>();

            this.learnNormal();
            this.anomalies = this.detect();
            this.minMaxVals = flightData.CalcMinMax();
        }
        public static void CreateLinearRegressionDetector(string csvFilePath)
        {
            if (instance != null)
            {
                throw new System.Exception("Instance already created");
            }
            instance = new LinearRegressionDetector(csvFilePath);
        }
        public static LinearRegressionDetector GetInstance()
        {
            if (instance == null)
            {
                throw new System.Exception("Instance was not created");
            }
            return instance;
        }
        public Dictionary<string, List<double>> MinMaxVals
        {
            get
            {
                return minMaxVals;
            }
        }

        /// <summary>
        /// learn the model in order to write the csv
        /// </summary>
        void learnNormalToCsv()
        {
            Dictionary<string, List<double>> data = this.flightData.getData();
            List<string> features = this.flightData.getFeatures();
            int dataSize = this.flightData.getSizeOfLines();

            foreach (string feature in features)
            {
                string feature2 = string.Empty;
                double correlation = 0;
                double threashold = 0;
                Line reg_line;

                foreach (string corrFeature in features)
                {
                    if (feature != corrFeature)
                    {
                        double featuresCorr = AnomalyDetectionUtil.pearson(data[feature], data[corrFeature]);

                        if (Math.Abs(featuresCorr) > Math.Abs(correlation) && Math.Abs(featuresCorr) > significantCorrelation)
                        {
                            feature2 = corrFeature;
                            correlation = featuresCorr;
                        }

                        else if (correlation == 0)
                        {
                            feature2 = corrFeature;
                        }
                    }
                }

                double maxDev = 0, currDev = 0;
                reg_line = AnomalyDetectionUtil.linear_reg(data[feature], data[feature2]);

                for (int i = 0; i < dataSize; i++)
                {
                    currDev = AnomalyDetectionUtil.dev(new Point(data[feature][i], data[feature2][i]), reg_line);
                    if (currDev > maxDev)
                    {
                        maxDev = currDev;
                    }
                }

                threashold = maxDev;

                Console.WriteLine(feature);
                Console.WriteLine(feature2);
                Console.WriteLine(correlation);
                Console.WriteLine(threashold);
                Console.WriteLine(reg_line.a);
                Console.WriteLine(reg_line.b);
                Console.WriteLine("-----------------");
            }
        }

        /// <summary>
        /// Parse resource csv file in order to set list of CorrelatedFeatures.
        /// </summary>
        void learnNormal()
        {
            // read correlatedFeatures from resource csv
            using (var stream = Assembly
            .GetExecutingAssembly()
            .GetManifestResourceStream("LinearRegressionDLL.regFlightModelCsv.resources"))
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
                    corrFeature.regression_line = new Line(double.Parse(lineData[4]), double.Parse(lineData[5]));

                    this.cf.Add(corrFeature);
                }
            }
        }

        /// <summary>
        ///  detect anomalies from the given flightData as a result of the CorrelatedFeatures.
        /// </summary>
        /// <returns> list of anomalies </returns>
        List<AnomalyReport> detect()
        {
            List<AnomalyReport> report = new List<AnomalyReport>();
            Dictionary<string, List<double>> data = this.flightData.getData();
            List<string> features = this.flightData.getFeatures();
            int dataSize = this.flightData.getSizeOfLines();

            foreach (correlatedFeatures feature in this.cf)
            {
                // if there is a correlatedFeature to the current feature
                if (feature.feature2 != null)
                {
                    // look for deviations between the values of the correlated features
                    for (int i = 0; i < dataSize; i++)
                    {
                        Point colPoint = new Point(data[feature.feature1][i], data[feature.feature2][i]);
                        // if a deviation was detected
                        if (detectedDev(feature, colPoint))
                        {
                            // add the current line as an AnomalyReport
                            report.Add(new AnomalyReport(feature.feature1, feature.feature2, i + 1));
                        }
                    }
                }
            }

            return report;
        }

        /// <summary>
        ///  check if there is a significant deviation between the given point and the regression line of the correalted features.
        /// </summary>
        /// <param name="corrFeature"> a correlated feature </param>
        /// <param name="p"> a point </param>
        /// <returns> true - if there is a significant deviation, false if not.  </returns>
        bool detectedDev(correlatedFeatures corrFeature, Point p)
        {
            double detected_dev = AnomalyDetectionUtil.dev(p, corrFeature.regression_line);
            return (detected_dev > corrFeature.threshold);
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
        /// The function get a regression line's equation by feature name
        /// </summary>
        /// <param name="feature">The feature to get it's line</param>
        /// <returns>The line as a list of a and b when y=ax+b</returns>
        public List<double> GetLineByFeature(string feature)
        {
            List<double> lineEquation = new List<double>();
            Line line = this.cf[getIndexByFeature(feature)].regression_line;
            lineEquation.Add(line.a);
            lineEquation.Add(line.b);
            return lineEquation;
        }
    }
}
