using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;
using FlightInspectionDesktopApp.Graph;

namespace FlightInspectionDesktopApp.UserControls
{
    /// <summary>
    /// Interaction logic for Graph.xaml
    /// </summary>
    public partial class Graph : UserControl
    {
        bool start = true;
        GraphViewModel vm;
        int nextLine;

        /// <summary>
        /// Graph CTOR.
        /// </summary>
        public Graph()
        {
            InitializeComponent();
            vm = new GraphViewModel(new GraphModel(canGraph.Height, canGraph.Width, DataModel.Instance));
            this.DataContext = vm;
            this.nextLine = vm.VMCurrentLineIndex;

            // Create the X axis
            GeometryGroup xAxis = new GeometryGroup();
            xAxis.Children.Add(new LineGeometry(new Point(0, canGraph.Height / 2), new Point(canGraph.Width, canGraph.Height / 2)));
            Path xAxisPath = new Path
            {
                StrokeThickness = 1,
                Stroke = Brushes.Black,
                Data = xAxis
            };
            canGraph.Children.Add(xAxisPath);

            // Create the Y axis
            GeometryGroup yAxis = new GeometryGroup();
            yAxis.Children.Add(new LineGeometry(new Point(2, canGraph.Height), new Point(2, 0)));
            Path yAxisPath = new Path
            {
                StrokeThickness = 1,
                Stroke = Brushes.Black,
                Data = yAxis
            };
            canGraph.Children.Add(yAxisPath);
            DispatcherLoop();
        }

        /// <summary>
        /// Updates the canvas constantly.
        /// </summary>
        private void DispatcherLoop()
        {
            DispatcherTimer dispatcher = new DispatcherTimer { Interval = TimeSpan.FromSeconds(1) };
            dispatcher.Tick += (s, e) =>
            {
                // create all points of the column up to this "time"
                PointCollection points = vm.GetPointsByCol((string)ColNames.SelectedItem);
                // connect them with a line
                Polyline polyline = new Polyline
                {
                    StrokeThickness = 1,
                    Stroke = Brushes.BlueViolet,
                    Points = points
                };
                canGraph.Children.Add(polyline);
                // delete the already-drawn graph if user goes backwards
                if (nextLine > vm.VMCurrentLineIndex)
                {
                    canGraph.Children.RemoveRange(2, canGraph.Children.Count - 2);
                }
                nextLine = vm.VMCurrentLineIndex;
            };
            dispatcher.Start();
        }

        /// <summary>
        /// Deletes the drawn graph in case a different column is selected.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ColNames_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // delete everything from th graph except for axises
            if (!start)
            {
                canGraph.Children.RemoveRange(2, canGraph.Children.Count - 2);

            }
            start = false;
        }
    }
}

