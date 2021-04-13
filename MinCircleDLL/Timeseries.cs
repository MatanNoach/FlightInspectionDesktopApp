using System.IO;
using System.Linq;
using System.Collections.Generic;

namespace MinCircleDLL
{
    class Timeseries
    {
        // fields of Timeseries object
        private string csvFile;
        private List<string> features;
        private List<List<double>> lines;
        private Dictionary<string, List<double>> data;

        /// <summary>
        /// CTOR of Timeseries
        /// </summary>
        /// <param name="file"> a csv file which should be converted into Timeseries object </param>
        /// <param name="colNames"> the names of the csv's columns </param>
        public Timeseries(string file, List<string> colNames)
        {
            this.csvFile = file;
            this.features = colNames;
            this.lines = new List<List<double>>();
            this.data = new Dictionary<string, List<double>>();

            this.parseCsvFile();
        }

        /// <summary>
        ///  this function parses the csv file into a Timeseries object.
        /// </summary>
        private void parseCsvFile()
        {
            using (StreamReader csvReader = new StreamReader(this.csvFile))
            {
                string currLine = string.Empty;
                //string[] columns = { };
                List<double> lineData = new List<double>();
                char colSeparator = ',';
                int index = 0;

                // add features as keys in data
                foreach (string feature in this.features)
                {
                    this.data.Add(feature, new List<double>());
                }

                // while there are more lines to read from the csv file
                while ((currLine = csvReader.ReadLine()) != null)
                {
                    index = 0;
                    lineData = currLine.Split(colSeparator).Select(x => double.Parse(x)).ToList();

                    // insert the data into a dictinary
                    foreach (double value in lineData)
                    {
                        this.data[this.features[index]].Add(value);
                        index++;
                    }

                    // insert raw data into a list of the rows
                    this.lines.Add(lineData);
                    lineData = new List<double>();
                }
            }
        }

        /// <summary>
        ///  getter for the features of the Timeseries object.
        /// </summary>
        /// <returns> the features of the Timeseries object </returns>
        public List<string> getFeatures()
        {
            return this.features;
        }

        /// <summary>
        ///  getter for the number of lines which were parsed from the csv file
        /// </summary>
        /// <returns> number of lines which were parsed from the csv file </returns>
        public int getSizeOfLines()
        {
            return this.lines.Count;
        }

        /// <summary>
        ///  getter for the dictionary of the Timseries object's data.
        /// </summary>
        /// <returns> the dictionary of the Timseries object's data </returns>
        public Dictionary<string, List<double>> getData()
        {
            return this.data;
        }
        /// <summary>
        /// Calculate the minimal & maximal values of each propery.
        /// </summary>
        public Dictionary<string, List<double>> CalcMinMax()
        {
            Dictionary<string, List<double>> minMaxVals = new Dictionary<string, List<double>>();
            foreach (string key in data.Keys)
            {
                minMaxVals[key] = new List<double>();
                minMaxVals[key].Add(data[key].Min());
                minMaxVals[key].Add(data[key].Max());
            }
            return minMaxVals;
        }
    }
}
