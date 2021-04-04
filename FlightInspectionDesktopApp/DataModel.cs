using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;

namespace FlightInspectionDesktopApp
{
    class DataModel
    {
        private Dictionary<string, List<double>> dictData;
        private List<string> rawData;
        private int currentLineIndex;
        private static DataModel dataModelInstance;
        private int nextLine = 1;

        public int CurrentLineIndex
        {
            get
            {
                return currentLineIndex;
            }

            set
            {
                currentLineIndex = value;
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
        public static void CreateModel()
        {

            if (dataModelInstance != null)
            {
                throw new Exception("DataModel was created");
            }
            dataModelInstance = new DataModel();
        }

        private DataModel() { }
        private DataModel(string csvPath, string xmlPath)
        {
            currentLineIndex = 0;
            rawData = new List<string>();
            dictData = new Dictionary<string, List<double>>();

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
                string currentLine;
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
        }

        public static void CreateModel(string csvPath, string xmlPath)
        {
            if (dataModelInstance != null)
            {
                throw new Exception("DataModel is already created");
            }
            dataModelInstance = new DataModel(csvPath, xmlPath);
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
            if (dictData.ContainsKey(key) && dictData[key].Count <= time)
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
            if (rawData.Count <= index)
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
        public void moveNextLine()
        {
            if (!((currentLineIndex == 0 && nextLine < 0) || (currentLineIndex == rawData.Count && nextLine > 0)))
            {
                currentLineIndex += nextLine;
            }
        }
    }
}
