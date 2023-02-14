using System;

namespace LaserCAM.Extensions
{
    public static class MathExtensions
    {
        public static double Round(this double value) => Math.Round(value, 2);
    }
}
