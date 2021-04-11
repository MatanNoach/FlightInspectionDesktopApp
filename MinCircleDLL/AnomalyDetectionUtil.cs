using System;
using System.Collections.Generic;

namespace MinCircleDLL
{
    class Line
    {
        // fields of Line object
        public double a, b;

        /// <summary>
        /// CTOR of Line object
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        public Line(double a, double b)
        {
            this.a = a;
            this.b = b;
        }

        /// <summary>
        ///  this function returns the calculated y value from the line equation.
        /// </summary>
        /// <param name="x"> the x value which should be set in the equation </param>
        /// <returns> the y value </returns>
        public double f(double x)
        {
            return this.a * x + this.b;
        }
    }

    public class Point
    {
        // fields of Point object
        public double x, y;

        /// <summary>
        /// CTOR of Point object
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public Point(double x, double y)
        {
            this.x = x;
            this.y = y;
        }
    }

    static class AnomalyDetectionUtil
    {
        /// <summary>
        ///  this function returns an average of a list of doubles.
        /// </summary>
        /// <param name="x"> list of doubles </param>
        /// <returns> the average if the list </returns>
        static double Avg(List<double> x)
        {
            double sum = 0;
            int size = x.Count;

            for (int i = 0; i < size; i++)
            {
                sum += x[i];
            }

            return sum / size;
        }

        /// <summary>
        ///  this function returns the variance of a list of doubles.
        /// </summary>
        /// <param name="x"> a list of doubles </param>
        /// <returns> the variance of the list </returns>
        static double Var(List<double> x)
        {
            int size = x.Count;
            double coefficient = 1 / (double)size;
            double sum = 0;

            for (int i = 0; i < size; i++)
            {
                sum += Math.Pow(x[i], 2);
            }

            return (coefficient * sum) - Math.Pow(Avg(x), 2);
        }

        /// <summary>
        ///  this function returns the covariance of two lists of doubles.
        /// </summary>
        /// <param name="x"> the first list of doubles </param>
        /// <param name="y"> the second list of doubles </param>
        /// <returns> the covariance of the given lists </returns>
        static double cov(List<double> x, List<double> y)
        {
            double result = 0;
            int size = x.Count;

            for (int i = 0; i < size; i++)
            {
                result += (x[i] - Avg(x)) * (y[i] - Avg(y));
            }

            return result / size;
        }

        /// <summary>
        ///  this function returns the Pearson correlation coefficient of two lists of doubles.
        /// </summary>
        /// <param name="x"> the first list of doubles </param>
        /// <param name="y"> the second list of doubles </param>
        /// <returns> the pearson correlation coefficient of the lists </returns>
        public static double pearson(List<double> x, List<double> y)
        {
            return cov(x, y) / (Math.Sqrt(Var(x)) * Math.Sqrt(Var(y)));
        }

        /// <summary>
        ///  this function performs a linear regression from two lists of doubles, and returns it's line equation.
        /// </summary>
        /// <param name="x"> the first list of doubles </param>
        /// <param name="y"> the second list of doubles </param>
        /// <returns> the line equation of the linear regression which was performed on the lists </returns>
        public static Line linear_reg(List<double> x, List<double> y)
        {
            double a = cov(x, y) / Var(x);
            double b = Avg(y) - (a * Avg(x));

            return new Line(a, b);
        }

        /// <summary>
        ///  this function performs a linear regression from a lists of points (which are converted into two lists of doubles), 
        ///  and returns it's line equation.
        /// </summary>
        /// <param name="points"> a list of points </param>
        /// <returns> the line equation of the linear regression which was performed on the list </returns>
        static Line linear_reg(List<Point> points)
        {
            List<double> x = new List<double>();
            List<double> y = new List<double>();
            int size = points.Count;

            for (int i = 0; i < size; i++)
            {
                x.Add(points[i].x);
                y.Add(points[i].y);
            }

            return linear_reg(x, y);
        }

        /// <summary>
        ///  this function returns the deviation between a point and a line.
        /// </summary>
        /// <param name="p"> a point </param>
        /// <param name="l"> a line </param>
        /// <returns> the deviation between the point and the line </returns>
        public static double dev(Point p, Line l)
        {
            double y = l.f(p.x);
            return Math.Abs(y - p.y);
        }

        /// <summary>
        ///  this function returns the deviation between a point and a list of points (which are converted into a line).
        /// </summary>
        /// <param name="p"> a given point </param>
        /// <param name="points"> a list of point </param>
        /// <returns> the deviation between the point and the list of points </returns>
        static double dev(Point p, List<Point> points)
        {
            Line line = linear_reg(points);
            return dev(p, line);
        }
    }
}
