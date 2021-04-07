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
        double margin = 2;

        /// <summary>
        /// Graph CTOR.
        /// </summary>
        public Graph()
        {
            InitializeComponent();
            vm = new GraphViewModel(new GraphModel(canGraph.Height, canGraph.Width, margin, DataModel.Instance));
            this.DataContext = vm;
            this.nextLine = vm.VMCurrentLineIndex;


            // Create the axises of both graphs:
            Path xAxisPath1 = CreateAxis(new Point(0, canGraph.Height / 2), new Point(canGraph.Width, canGraph.Height / 2));
            canGraph.Children.Add(xAxisPath1);
            Path xAxisPath2 = CreateAxis(new Point(0, corrGraph.Height / 2), new Point(corrGraph.Width, canGraph.Height / 2));
            corrGraph.Children.Add(xAxisPath2);
            Path yAxisPath1 = CreateAxis(new Point(margin, canGraph.Height), new Point(margin, 0));
            canGraph.Children.Add(yAxisPath1);
            Path yAxisPath2 = CreateAxis(new Point(margin, corrGraph.Height), new Point(margin, 0));
            corrGraph.Children.Add(yAxisPath2);

            DispatcherLoop();
        }

        /// <summary>
        /// Draws x & y axis for both graphs.
        /// </summary>
        /// <param name="p1">point of axis line</param>
        /// <param name="p2">point of axis line</param>
        /// <returns></returns>
        private Path CreateAxis(Point p1, Point p2)
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

        /// <summary>
        /// Updates the canvas constantly.
        /// </summary>
        private void DispatcherLoop()
        {
            DispatcherTimer dispatcher = new DispatcherTimer { Interval = TimeSpan.FromSeconds(1) };
            dispatcher.Tick += (s, e) =>
            {
                // create all points of the column up to this "time" for both feature & correlated feature
                PointCollection points = vm.GetPointsByCol((string)ColNames.SelectedItem);
                PointCollection pointsCorr = vm.GetPointsByCol(vm.CorrData[(string)ColNames.SelectedItem]);
                // connect them with a line
                Polyline polyline = new Polyline
                {
                    StrokeThickness = 0.5,
                    Stroke = Brushes.LightSkyBlue,
                    Points = points
                };
                canGraph.Children.Add(polyline);
                Polyline polyline1 = new Polyline
                {
                    StrokeThickness = 0.5,
                    Stroke = Brushes.LightSkyBlue,
                    Points = pointsCorr
                };
                corrGraph.Children.Add(polyline1);
                // delete the already-drawn graphs if user goes backwards
                if (nextLine > vm.VMCurrentLineIndex)
                {
                    canGraph.Children.RemoveRange(2, canGraph.Children.Count - 2);
                    corrGraph.Children.RemoveRange(2, corrGraph.Children.Count - 2);
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
            if (!start)
            {
                // delete everything from th graph except for axises
                canGraph.Children.RemoveRange(2, canGraph.Children.Count - 2);
                corrGraph.Children.RemoveRange(2, corrGraph.Children.Count - 2);
                // update the correlated feature
                vm.VMCorrCol = vm.CorrData[(string)ColNames.SelectedItem];

            }
            else
            {
                start = false;
                vm.VMCorrCol = vm.CorrData[(string)ColNames.SelectedItem];
            }
        }
    }
}

