
namespace CommonCode.Extensions
{
    /// <summary>
    /// Provides extension methods for double data type.
    /// </summary>
    public static class DoubleExtensions
    {
        /// <summary>
        /// The constant value for line spacing in points.
        /// </summary>
        private const double LINE_SPACING_CONSTANT = 12.0;

        /// <summary>
        /// Converts centimeters to points.
        /// </summary>
        /// <param name="cm">The value in centimeters.</param>
        /// <returns>The value in points.</returns>
        public static double ConvertCmToPoints(this double cm)
        {
            double points = cm * 28.3464567;
            return points;
        }

        /// <summary>
        /// Converts points to centimeters.
        /// </summary>
        /// <param name="points">The value in points.</param>
        /// <returns>The value in centimeters.</returns>
        public static double ConvertPointsToCm(this double points)
        {
            double cm = points / 28.3464567;
            return Math.Round(cm, 2);
        }

        /// <summary>
        /// Gets the line spacing in points.
        /// </summary>
        /// <param name="lineSpacing">The line spacing value.</param>
        /// <returns>The line spacing value in points.</returns>
        public static double GetLineSpacingInPoints(this double lineSpacing)
        {
            return lineSpacing * LINE_SPACING_CONSTANT;
        }
    }
}
