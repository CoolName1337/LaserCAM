﻿using System.Windows;

namespace LaserCAM.Extensions
{
    public static class PointExstensions
    {
        public static Point Multiply(this Point p, double d)
        {
            p.X *= d;
            p.Y *= d;
            return p;
        }
        public static Point Divide(this Point p, double d)
        {
            p.X /= d;
            p.Y /= d;
            return p;
        }
        public static Point Round(this Point p, int digits = 2) => new Point(p.X.Round(digits), p.Y.Round(digits));
    }

}
