using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Shapes;

namespace LinearRegressionDLL
{
    /// <summary>
    /// Interaction logic for LinearRegressionGraph.xaml
    /// </summary>
    public partial class LinearRegressionGraph : UserControl
    {
        LinearGraphViewModel vm;
        double margin = 5;
        string feature;
        public LinearRegressionGraph(string csvFilePath, string feature)
        {
            InitializeComponent();
            try
            {
                LinearRegressionDetector.CreateLinearRegressionDetector(csvFilePath);
                vm = new LinearGraphViewModel(LinearRegressionDetector.GetInstance());
                this.DataContext = vm;
                // create the x and y axis
                Path xAxis = CreateAxis(new System.Windows.Point(margin, LinearGraph.Height / 2), new System.Windows.Point(LinearGraph.Width, LinearGraph.Height / 2));
                LinearGraph.Children.Add(xAxis);
                Path yAxis = CreateAxis(new System.Windows.Point(LinearGraph.Width / 2, LinearGraph.Height), new System.Windows.Point(LinearGraph.Width / 2, 0));
                LinearGraph.Children.Add(yAxis);
                this.feature = feature;
                DrawLine();
            }
            catch
            {

            }
        }
        public void DrawLine()
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
            vm.GetPointsByFeature(Feature, LinearGraph.Height, LinearGraph.Width);
        }

        public void DeleteLine()
        {
            LinearGraph.Children.RemoveAt(4);
        }
        public string Feature
        {
            get
            {
                return this.feature;
            }
            set
            {
                string oldFeatre = this.feature;
                this.feature = value;
                if (oldFeatre != this.feature)
                {
                    DeleteLine();
                    DrawLine();
                }
            }
        }
        public static UserControl GetUserControl(string feature, string csvFilePath)
        {
            LinearRegressionGraph graph = new LinearRegressionGraph(csvFilePath, feature);
            return graph;
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
        /// Converts PointCollection into points that can be drawn on canvas.
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
        /// Converts PointCollection into points that can be drawn on canvas.
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
