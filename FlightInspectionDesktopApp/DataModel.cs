using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Xml;

namespace FlightInspectionDesktopApp
{
    class DataModel : INotifyPropertyChanged
    {
        private Dictionary<string, List<double>> dictData;
        private Dictionary<string, string> corrData;
        private Dictionary<string, List<double>> linRegData;
        private List<string> rawData;
        private int currentLineIndex;
        private static DataModel dataModelInstance;
        private int nextLine = 1;
        private List<string> colNames;
        private Dictionary<string, List<double>> minMaxVals;


        public event PropertyChangedEventHandler PropertyChanged;
        public void NotifyPropertyChanged(string propName)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propName));
            }
        }
        public int CurrentLineIndex
        {
            get
            {
                return currentLineIndex;
            }

            set
            {
                currentLineIndex = value;
                NotifyPropertyChanged("CurrentLineIndex");
            }
        }

        public int NextLine
        {
            get
            {
                return nextLine;
            }
            set
            {
                nextLine = value;
            }
        }

        public List<string> ColNames
        {
            get
            {
                return colNames;
            }
        }

        public Dictionary<string, List<double>> MinMaxVals
        {
            get { return this.minMaxVals; }
        }

        public static DataModel Instance
        {
            get
            {
                if (dataModelInstance == null)
                {
                    throw new Exception("DataModel was not created");
                }
                return dataModelInstance;
            }
        }
        public Dictionary<string, string> CorrData { get { return this.corrData; } }
        public static void CreateModel()
        {

            if (dataModelInstance != null)
            {
                throw new Exception("DataModel was created");
            }
            dataModelInstance = new DataModel();
        }

        public Dictionary<string, List<double>> LinRegData { get { return this.linRegData; } }

        public static void CreateModel(string csvPath, string xmlPath)
        {
            if (dataModelInstance != null)
            {
                throw new Exception("DataModel is already created");
            }
            dataModelInstance = new DataModel(csvPath, xmlPath);

            //LinearRegressionDetector detector = new LinearRegressionDetector(csvPath);
            //List<DrawPoint> points = detector.getPointsToDraw("airspeed-kt");
        }

        private DataModel() { }
        private DataModel(string csvPath, string xmlPath)
        {
            currentLineIndex = 0;
            rawData = new List<string>();
            corrData = new Dictionary<string, string>();
            dictData = new Dictionary<string, List<double>>();
            linRegData = new Dictionary<string, List<double>>();

            // get the chunks' names from the given xml file
            List<string> xmlColumns = getXmlColumns(xmlPath);

            // initialize dictionary's keys
            for (int i = 0; i < xmlColumns.Count; i++)
            {
                dictData.Add(xmlColumns[i], new List<double>());
            }

            // parse the data from the csv file
            using (StreamReader csvReader = new StreamReader(csvPath))
            {
                char colSeparator = getVarSeparator(xmlPath);
                string currentLine = csvReader.ReadLine();
                string[] lineCols = { };
                int index = 0;

                while ((currentLine = csvReader.ReadLine()) != null)
                {
                    index = 0;
                    lineCols = currentLine.Split(colSeparator);
                    foreach (string value in lineCols)
                    {
                        dictData[xmlColumns[index]].Add(double.Parse(value));
                        index += 1;
                    }

                    rawData.Add(currentLine);
                }
            }

            // calculate most correlative column for each column by pearson
            foreach (string key in dictData.Keys)
            {
                corrData.Add(key, mostCorrelativeKey(key));
                linRegData.Add(key, LinearReg(dictData[key], dictData[corrData[key]], dictData[key].Count));
            }
            CalcMinMax();
        }

        /// <summary>
        /// find the most correlative column for a given key.
        /// </summary>
        /// <param name="key"> the key which the function should find the most correlative column </param>
        /// <returns> the most correlative column name </returns>
        private string mostCorrelativeKey(string key)
        {
            string corrKey = string.Empty;
            double biggestPearsonVal = 0;
            double val = 0;

            foreach (string currKey in dictData.Keys)
            {
                if (currKey != key)
                {
                    val = pearson(dictData[key], dictData[currKey], dictData[currKey].Count);

                    // if the calculated pearson's values is Nan (as a result of dividing by 0) and there is not previous correlated column
                    if (Double.IsNaN(val) && biggestPearsonVal == 0)
                    {
                        corrKey = currKey;
                    }

                    // if the calculated pearson's value is not Nan and is bigger than the previous correlated column
                    else if (Math.Abs(val) > Math.Abs(biggestPearsonVal))
                    {
                        biggestPearsonVal = val;
                        corrKey = currKey;
                    }
                }
            }

            return corrKey;
        }

        /// <summary>
        ///  this function returns the Pearson correlation coefficient of X and Y.
        /// </summary>
        /// <param name="firstKeyValues"> first array of values </param>
        /// <param name="secondKeyValues"> second array of values </param>
        /// <param name="arrSize"> the size of both of the arrays </param>
        /// <returns> the Pearson correlation coefficient of the given arrays </returns>
        private double pearson(List<double> firstKeyValues, List<double> secondKeyValues, double arrSize)
        {
            return cov(firstKeyValues, secondKeyValues, arrSize) / (Math.Sqrt(var(firstKeyValues, arrSize)) * Math.Sqrt(var(secondKeyValues, arrSize)));
        }

        /// <summary>
        ///  this function returns the covariance of X and Y.
        /// </summary>
        /// <param name="firstKeyValues"> first array of values </param>
        /// <param name="secondKeyValues"> second array of values </param>
        /// <param name="arrSize"> the size of both of the arrays </param>
        /// <returns> the covariance of the given arrays </returns>
        private double cov(List<double> firstKeyValues, List<double> secondKeyValues, double arrSize)
        {
            double result = 0;
            double firstKeyAvg = firstKeyValues.Average();
            double secondKeyAvg = secondKeyValues.Average();

            for (int i = 0; i < arrSize; i++)
            {
                result += (firstKeyValues[i]) * (secondKeyValues[i]);
            }

            return (result / arrSize) - firstKeyAvg * secondKeyAvg;
        }

        /// <summary>
        ///  this function returns the variance of X.
        /// </summary>
        /// <param name="values"> array of doubles </param>
        /// <param name="arrSize"> the size of the array </param>
        /// <returns> the variance of the array </returns>
        private double var(List<double> values, double arrSize)
        {
            double coefficient = 1 / (double)arrSize;
            double sum = 0;

            for (int i = 0; i < arrSize; i++)
            {
                sum += Math.Pow(values[i], 2);
            }

            return (coefficient * sum) - Math.Pow(values.Average(), 2);

        }
        /// <summary>
        /// Performs linear regression and returns the line equation represented by a & b, where: y = ax + b.
        /// </summary>
        /// <param name="firstKeyValues">values of the first feature</param>
        /// <param name="secondKeyValues">values of the second feature</param>
        /// <param name="arrSize">amount of values</param>
        /// <returns>List where a is the first item and b is the second</returns>
        // 
        private List<double> LinearReg(List<double> firstKeyValues, List<double> secondKeyValues, double arrSize)
        {
            double a = cov(firstKeyValues, secondKeyValues, arrSize) / var(firstKeyValues, arrSize);
            double b = secondKeyValues.Average() - a * firstKeyValues.Average();
            return new List<double>
            {
                a,
                b
            };
        }


        /// <summary>
        /// this function reads the given xml file and retruns all the columns' names.
        /// </summary>
        /// <param name="xmlPath">
        ///     the path of the xml file.
        /// </param>
        /// <returns>
        ///     the columns' names.
        /// </returns>
        private List<String> getXmlColumns(string xmlPath)
        {
            FileStream fs = new FileStream(xmlPath, FileMode.Open, FileAccess.Read);
            XmlDocument xmlDoc = new XmlDocument();

            xmlDoc.Load(fs);
            // get all "chunk" tags from the given xml
            XmlNodeList xmlnode = xmlDoc.SelectNodes("//input//chunk");

            List<string> xmlCols = new List<string>();

            XmlNodeList childNodes;
            string digit = string.Empty;
            string colName = string.Empty;

            // iterate over the chunks
            for (int i = 0; i < xmlnode.Count; i++)
            {
                childNodes = xmlnode[i].ChildNodes;
                digit = string.Empty;
                colName = string.Empty;

                // get the name of the chunk, in order to process it as a csv column
                for (int j = 0; j < childNodes.Count; j++)
                {
                    if (childNodes[j].Name == "name")
                    {
                        colName = childNodes[j].InnerText;
                    }
                    else
                    {
                        if (childNodes[j].Name == "node" && childNodes[j].InnerText.Any(char.IsDigit))
                        {
                            for (int k = 0; k < childNodes[j].InnerText.Length; k++)
                            {
                                if (char.IsDigit(childNodes[j].InnerText[k]))
                                {
                                    digit += childNodes[j].InnerText[k];
                                }
                            }
                        }
                    }
                }

                // in order to differentiate between columns with the same name
                if (digit.Length > 0)
                {
                    colName += "_" + digit;
                }

                xmlCols.Add(colName);
            }
            this.colNames = xmlCols;
            return xmlCols;
        }

        /// <summary>
        /// this function reads the given xml and returns the correct separated value from the given xml file.
        /// </summary>
        /// <param name="xmlPath">
        ///     the path of the xml file.
        /// </param>
        /// <returns>
        ///     the separator character of "input" tag.
        /// </returns>
        private char getVarSeparator(string xmlPath)
        {
            List<string> cols = new List<string>();
            FileStream fs = new FileStream(xmlPath, FileMode.Open, FileAccess.Read);
            XmlDocument xmlDoc = new XmlDocument();

            xmlDoc.Load(fs);
            XmlNodeList xmlNode = xmlDoc.SelectNodes("//input//var_separator");

            if (xmlNode.Count > 0)
            {
                return char.Parse(xmlNode[0].InnerText);
            }
            return Properties.Settings.Default.varSeparator;
        }

        /// <summary>
        /// get a value from the csv file by the column name (key) and the row (time).
        /// </summary>
        /// <param name="key"> the column name in the xml file, corresponding the columns in the csv file </param>
        /// <param name="time"> the row in the csv file </param>
        /// <returns> a value from the csv file</returns>
        internal double getValueByKeyAndTime(string key, int time)
        {
            if (dictData.ContainsKey(key) && dictData[key].Count > time)
            {
                return dictData[key][time];
            }
            return 0;
        }

        /// <summary>
        /// get a line from the csv file by the row index.
        /// </summary>
        /// <param name="index"> the row index </param>
        /// <returns> a string which represents a line in the csv file </returns>
        internal string getLineByIndex(int index)
        {
            if (rawData.Count > index)
            {
                return rawData[index];
            }
            return string.Empty;
        }

        /// <summary>
        /// get the number of rows which were processed from the csv file.
        /// </summary>
        /// <returns> the number of the rows of the csv file </returns>
        internal int getDataSize()
        {
            return rawData.Count;
        }

        /// <summary>
        /// get the min value of the given key.
        /// </summary>
        /// <param name="key"> a column name </param>
        /// <returns> the minimal value </returns>
        internal double getMinValueByKey(string key)
        {
            if (dictData.ContainsKey(key))
            {
                return dictData[key].Min();
            }
            return -9999;
        }

        /// <summary>
        /// get the max value of the given key.
        /// </summary>
        /// <param name="key"> a column name </param>
        /// <returns> the maximal value </returns>
        internal double getMaxValueByKey(string key)
        {
            if (dictData.ContainsKey(key))
            {
                return dictData[key].Max();
            }
            return 9999;
        }
        public void moveNextLine()
        {
            // if the line is not zero in reverse or not max line in forward, read the next line
            if (!((currentLineIndex == 0 && nextLine < 0) || (currentLineIndex == (rawData.Count - 1) && nextLine > 0)))
            {
                CurrentLineIndex += nextLine;
            }
        }

        /// <summary>
        /// Calculate the minimal & maximal values of each propery.
        /// </summary>
        public void CalcMinMax()
        {
            minMaxVals = new Dictionary<string, List<double>>();
            foreach (string key in dictData.Keys)
            {
                minMaxVals[key] = new List<double>();
                minMaxVals[key].Add(dictData[key].Min());
                minMaxVals[key].Add(dictData[key].Max());
            }
        }
    }
}
