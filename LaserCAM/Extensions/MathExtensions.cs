using System;

namespace LaserCAM.Extensions
{
    public static class MathExtensions
    {
        public static double Round(this double value, int digits = 2) => Math.Round(value, digits);
    }
}
