using System;
using System.IO;
using System.Xml;
using System.Linq;
using System.Collections.Generic;

namespace FlightInspectionDesktopApp
{
    class DataModel
    {
        private Dictionary<string, List<double>> data;

        private static DataModel dataModelInstance;
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

        private DataModel() { }
        private DataModel(string csvPath, string xmlPath)
        {
            data = new Dictionary<string, List<double>>();

            // get the chunks' names from the given xml file
            List<string> xmlColumns = getXmlColumns(xmlPath);

            // initialize dictionary's keys
            for (int i = 0; i < xmlColumns.Count; i++)
            {
                data.Add(xmlColumns[i], new List<double>());
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
                        data[xmlColumns[index]].Add(double.Parse(value));
                        index += 1;
                    }
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

        internal double getValueByKeyAndTime(string key, int time)
        {
            return data[key][time];
        }
    }
}