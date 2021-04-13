using System;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using System.Globalization;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Controls;
using System.Collections.Generic;

namespace MinCircleDLL
{
    /// <summary>
    /// Interaction logic for MinCircleGraph.xaml
    /// </summary>
    public partial class MinCircleGraph : UserControl, IAbstractDetector
    {
        // fields of MinCircleGraph
        string feature;
        MinCircleViewModel vm;
        double margin = 5;

        /// <summary>
        /// CTOR of MinCircleGraph.
        /// </summary>        
        /// <param name="csvFilePath"> a csv file which should be converted into Timeseries object </param>
        /// <param name="colNames"> the names of the csv's columns  </param>
        public MinCircleGraph(string csvFilePath, List<string> colNames)
        {
            InitializeComponent();
            // try use the MinCircleDetector instance in the vm constuctor
            vm = new MinCircleViewModel(MinCircleDetector.GetInstance(csvFilePath, colNames));
            this.DataContext = vm;
            // draw x and y axis on the canvas
            Path xAxis = CreateAxis(new System.Windows.Point(margin, CircleGraph.Height / 2), new System.Windows.Point(CircleGraph.Width, CircleGraph.Height / 2));
            CircleGraph.Children.Add(xAxis);
            Path yAxis = CreateAxis(new System.Windows.Point(CircleGraph.Width / 2, CircleGraph.Height), new System.Windows.Point(CircleGraph.Width / 2, 0));
            CircleGraph.Children.Add(yAxis);
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
                if (oldFeature != this.feature)
                {
                    // if it is not the startup page, delete the old lines
                    if (oldFeature != null)
                    {
                        DeleteLinesAndCircle();
                    }
                    // draw the Circle and data
                    DrawLinesAndCircle();
                }
            }
        }

        /// <summary>
        /// Property of field currentLineIndex.
        /// </summary>
        public int CurrentLineIndex
        {
            // setter of currentLineIndex.
            set
            {
                vm.UpdateCurrentLineIndex(value, feature, CircleGraph.Height, CircleGraph.Width);
            }
        }

        /// <summary>
        /// The function deletes the line and circle from the canvas
        /// </summary>
        public void DeleteLinesAndCircle()
        {
            CircleGraph.Children.RemoveRange(4, CircleGraph.Children.Count - 4);
        }

        /// <summary>
        /// The function draws the correlated circle and the points
        /// </summary>
        public void DrawLinesAndCircle()
        {
            // get the circle
            Circle c = vm.GetCorrCircle(feature, CircleGraph.Height, CircleGraph.Width);
            // draw the a circle in the right place
            Ellipse e = new Ellipse();
            e.Stroke = Brushes.Red;
            e.Width = c.radius * 2;
            e.Height = c.radius * 2;
            double left = c.center.x + (CircleGraph.Width / 2) - c.radius;
            double top = (CircleGraph.Height / 2) - c.center.y - c.radius;
            e.Margin = new Thickness(left, top, 0, 0);
            CircleGraph.Children.Add(e);
            // load new points by the feature
            vm.LoadPointsByFeature(Feature, CircleGraph.Height, CircleGraph.Width);
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
