using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MinCircleDLL
{
    /// <summary>
    /// Interaction logic for MinCircleGraph.xaml
    /// </summary>
    public partial class MinCircleGraph : UserControl, IAbstractDetector
    {
        string feature;
        MinCircleViewModel vm;
        double margin = 5;
        public MinCircleGraph(string csvFilePath)
        {
            InitializeComponent();
            // try use the MinCircleDetector instance in the vm constuctor
            try
            {
                vm = new MinCircleViewModel(MinCircleDetector.GetInstance());
            }
            // in case of a failure, create the instance, and try to set it again
            catch
            {
                MinCircleDetector.CreateMinCircleDetector(csvFilePath);
                vm = new MinCircleViewModel(MinCircleDetector.GetInstance());
            }
            this.DataContext = vm;
            // draw x and y axis on the canvas
            Path xAxis = CreateAxis(new System.Windows.Point(margin, CircleGraph.Height / 2), new System.Windows.Point(CircleGraph.Width, CircleGraph.Height / 2));
            CircleGraph.Children.Add(xAxis);
            Path yAxis = CreateAxis(new System.Windows.Point(CircleGraph.Width / 2, CircleGraph.Height), new System.Windows.Point(CircleGraph.Width / 2, 0));
            CircleGraph.Children.Add(yAxis);

        }

        public string Feature
        {
            get
            {
                return this.feature;
            }
            set
            {
                this.feature = value;
            }
        }
        public int CurrentLineIndex
        {

        }
        public DrawLinesAndCircle()
        {
            Circle c = vm.GetCorrCircleByFeature();
            Ellipse e = new Ellipse();
            e.Stroke = System.Windows.Media.Brushes.Red;
            e.Width = c.radius * 2;
            e.Height = c.radius * 2;
            CircleGraph.children.add(e);
            vm.LoadPointsByFeature(Feature, CircleGraph.Height, CircleGraph.Width);
        }
        // The function returns the user control
        public UserControl GetUserControl(string csvFilePath)
        {
            MinCircleGraph graph = new MinCircleGraph(csvFilePath);

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
