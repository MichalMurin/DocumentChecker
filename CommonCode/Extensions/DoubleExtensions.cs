using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace CommonCode.Extensions
{
    public static class DoubleExtensions
    {
        public static double ConvertCmToPoints(this double cm)
        {
            double points = cm * 28.3464567;
            return points;
        }

        public static double ConvertPointsToCm(this double points)
        {
            double cm = points / 28.3464567;
            return Math.Round(cm, 2);
        }

        public static double GetLineSpacingInPoints(this double lineSpacing, double fontSize)
        {
            return lineSpacing * fontSize;
        }
    }
}
