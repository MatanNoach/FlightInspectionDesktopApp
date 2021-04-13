using System;
using System.Linq;
using System.Windows.Data;
using System.Globalization;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Controls;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace LinearRegressionDLL
{

    /// <summary>
    /// Interaction logic for LinearRegressionGraph.xaml
    /// </summary>
    public partial class LinearRegressionGraph : UserControl, IAbstractDetector
    {
        LinearGraphViewModel vm;
        double margin = 5;
        string feature;

        /// <summary>
        /// A constructor for the user control
        /// </summary>
        /// <param name="csvFilePath">The csv file path to detect anomalies</param>
        /// <param name="colNames"> the names of the csv's columns </param>        
        public LinearRegressionGraph(string csvFilePath, List<string> colNames)
        {
            InitializeComponent();
            try
            {
                this.vm = new LinearGraphViewModel(LinearRegressionDetector.GetInstance(csvFilePath, colNames));
                this.DataContext = this.vm;
                // draw x and y axis on the canvas
                Path xAxis = CreateAxis(new System.Windows.Point(margin, LinearGraph.Height / 2), new System.Windows.Point(LinearGraph.Width, LinearGraph.Height / 2));
                LinearGraph.Children.Add(xAxis);
                Path yAxis = CreateAxis(new System.Windows.Point(LinearGraph.Width / 2, LinearGraph.Height), new System.Windows.Point(LinearGraph.Width / 2, 0));
                LinearGraph.Children.Add(yAxis);
            }
            catch { }
        }
        /// <summary>
        /// The function draws the regression line and real data line
        /// </summary>
        public void DrawLines()
        {
            // create the regression line:
            Polyline polylineCorr = new Polyline
            {
                StrokeThickness = 1,
                Stroke = Brushes.IndianRed,
                Points = vm.GetLineRegPoints(Feature, LinearGraph.Height, LinearGraph.Width)
            };
            LinearGraph.Children.Add(polylineCorr);
            // get the correlated features points' to draw
            vm.LoadPointsByFeature(Feature, LinearGraph.Height, LinearGraph.Width);
        }
        /// <summary>
        /// The funcion deletes the regression line
        /// </summary>
        public void DeleteLine()
        {
            LinearGraph.Children.RemoveRange(4, LinearGraph.Children.Count - 4);
        }

        /// <summary>
        /// Property of field currentLineIndex.
        /// </summary>
        public int CurrentLineIndex
        {
            // setter of currentLineIndex.
            set
            {
                vm.UpdateCurrentLineIndex(value, Feature, LinearGraph.Height, LinearGraph.Width);
            }
        }

        /// <summary>
        /// Property of field feature.
        /// </summary>
        public string Feature
        {
            // getter of feature.
            get
            {
                return this.feature;
            }

            // setter of feature.
            set
            {
                string oldFeature = this.feature;
                this.feature = value;
                // if there is a new feature to present
                if (oldFeature != this.feature)
                {
                    // if it is not the startup page, delete the old lines
                    if (oldFeature != null)
                    {
                        DeleteLine();
                    }
                    // draw the regression line and data
                    DrawLines();
                }
            }
        }

        /// <summary>
        /// Draws x & y axis for all graphs.
        /// </summary>
        /// <param name="p1">point of axis line</param>
        /// <param name="p2">point of axis line</param>
        /// <returns></returns>
        private Path CreateAxis(System.Windows.Point p1, System.Windows.Point p2)
        {
            GeometryGroup xAxis = new GeometryGroup();
            xAxis.Children.Add(new LineGeometry(p1, p2));
            Path axisPath = new Path
            {
                StrokeThickness = 1,
                Stroke = Brushes.Black,
                Data = xAxis
            };
            return axisPath;
        }
    }
    public class RegLinePointsConverter : IValueConverter
    {
        /// <summary>
        /// Converts PointCollection into data points that can be drawn on canvas.
        /// </summary>
        /// <param name="value">VMCorrelatedPoints</param>
        /// <param name="targetType">none</param>
        /// <param name="parameter">none</param>
        /// <param name="culture">none</param>
        /// <returns>StreamGeometry that can be drawn using Path on canvas</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var geometry = new StreamGeometry();
            if (value is IEnumerable<DrawPoint> points && points.Any())
            {
                using (var sgc = geometry.Open())
                {
                    foreach (var point in points)
                    {
                        // if the points is regular, draw it on the canvas
                        if (!point.IsDeviated)
                        {
                            System.Windows.Point realPoint = new System.Windows.Point();
                            realPoint.X = point.X;
                            realPoint.Y = point.Y;
                            sgc.BeginFigure(realPoint, false, false);
                            sgc.LineTo(realPoint, true, false);
                        }
                    }
                }
            }
            return geometry;
        }

        /// <summary>
        /// Not implemented.
        /// </summary>
        /// <param name="value">none</param>
        /// <param name="targetType">none</param>
        /// <param name="parameter">none</param>
        /// <param name="culture">none</param>
        /// <returns></returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
    public class AnLinePointsConverter : IValueConverter
    {
        /// <summary>
        /// Converts PointCollection into anomalies points that can be drawn on canvas.
        /// </summary>
        /// <param name="value">VMCorrelatedPoints</param>
        /// <param name="targetType">none</param>
        /// <param name="parameter">none</param>
        /// <param name="culture">none</param>
        /// <returns>StreamGeometry that can be drawn using Path on canvas</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var geometry = new StreamGeometry();
            if (value is IEnumerable<DrawPoint> points && points.Any())
            {
                using (var sgc = geometry.Open())
                {
                    foreach (var point in points)
                    {
                        // if the points is anomaly, draw it on the canvas
                        if (point.IsDeviated)
                        {
                            System.Windows.Point realPoint = new System.Windows.Point();
                            realPoint.X = point.X;
                            realPoint.Y = point.Y;
                            sgc.BeginFigure(realPoint, false, false);
                            sgc.LineTo(realPoint, true, false);
                        }
                    }
                }
            }
            return geometry;
        }

        /// <summary>
        /// Not implemented.
        /// </summary>
        /// <param name="value">none</param>
        /// <param name="targetType">none</param>
        /// <param name="parameter">none</param>
        /// <param name="culture">none</param>
        /// <returns></returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
