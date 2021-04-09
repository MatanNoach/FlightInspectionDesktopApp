﻿using System.IO;
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
        public Timeseries(string file)
        {
            this.csvFile = file;
            this.features = new List<string>();
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
                string currLine = csvReader.ReadLine();
                string[] columns = { };
                List<double> lineData = new List<double>();
                char colSeparator = ',';
                int index = 0;

                // if the first line is not null and there are no previous features
                if (currLine != null && this.features.Count == 0)
                {
                    // parse the columns of the first row and insert them into the features' list
                    columns = currLine.Split(colSeparator);
                    foreach (string col in columns)
                    {
                        this.features.Add(col);
                    }

                    this.createDataDict();
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
        ///  this function sets the dictionary's keys as a result of the parsed features (columns' names)
        ///  if there are duplicated names - this function fix it by adding digits
        /// </summary>
        void createDataDict()
        {
            Dictionary<string, int> dupKeys = new Dictionary<string, int>();
            Dictionary<string, int> updateKeys = new Dictionary<string, int>();

            foreach (string key in this.features)
            {
                // if the key is new
                if (!dupKeys.ContainsKey(key))
                {
                    dupKeys.Add(key, 1);
                    this.data.Add(key, new List<double>());
                }
                else
                {
                    dupKeys[key]++;
                }
            }

            List<string> keys = dupKeys.Keys.ToList();

            foreach (string key in keys)
            {
                // if the key is unique
                if (dupKeys[key] == 1)
                {
                    dupKeys.Remove(key);
                }
                else
                {
                    updateKeys.Add(key, 0);
                }
            }

            // foreach feature in the list
            for (int i = 0; i < this.features.Count; i++)
            {
                // if the currnet feature is duplicated
                if (dupKeys.ContainsKey(this.features[i]))
                {
                    // edit it's name properly
                    string oldName = this.features[i];
                    this.features[i] = oldName + "_" + updateKeys[oldName];

                    if (this.data.ContainsKey(oldName))
                    {
                        this.data.Remove(oldName);
                    }

                    this.data.Add(this.features[i], new List<double>());
                    updateKeys[oldName]++;
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
    }
}
