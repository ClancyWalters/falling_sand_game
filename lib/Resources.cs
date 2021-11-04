using CustomProgram;
using System;
using System.Collections.Generic;

namespace LocalResouces
{
    /// <summary>
    /// Resources that may be applied to any c# project
    /// </summary>
    public static class GeneralResources
    {
        private static readonly Random _random = new Random();

        //Other people's functions:

        /// <summary>
        /// Maps a value in one range to another. From: <see href="https://forum.unity.com/threads/re-map-a-number-from-one-range-to-another.119437/">(click here)</see>
        /// </summary>
        public static double Map(double valueOf1, double low1, double high1, double low2, double high2)
        {
            return low2 + (valueOf1 - low1) * (high2 - low2) / (high1 - low1);
        }
        /// <summary>
        /// Generates a random number between a minimum and maximum value. From: <see href="https://stackoverflow.com/questions/1064901/random-number-between-2-double-numbers">(click here)</see>
        /// </summary>
        public static double GetRandomNumber(double minimum, double maximum)
        {
            return _random.NextDouble() * (maximum - minimum) + minimum;
        }
        /// <summary>
        /// Shuffles a list. From: <see href="https://stackoverflow.com/questions/273313/randomize-a-listt">(click here)</see>
        /// </summary>
        public static void Shuffle<T>(this IList<T> list)
        {
            for (var i = list.Count; i > 0; i--)
                list.Swap(0, _random.Next(0, i));
        }
        /// <summary>
        /// Swaps two points in a list. From: <see href="https://stackoverflow.com/questions/273313/randomize-a-listt">(click here)</see>
        /// </summary>
        public static void Swap<T>(this IList<T> list, int i, int j)
        {
            var temp = list[i];
            list[i] = list[j];
            list[j] = temp;
        }

        //My functions:
        /// <summary>
        /// Get a random bool from a probability between 0 and 1
        /// </summary>
        public static bool GetRandomBool(double probability)
        {
            if (probability >= 1)
            {
                return true;
            }
            if (probability <= 0)
            {
                return false;
            }
            double randomNumber = GetRandomNumber(0, 1);
            if (randomNumber < probability)
            {
                return true;
            }
            return false;
        }
        /// <summary>
        /// Keeps a value between min and max including both values
        /// </summary>
        public static double KeepWithinRange(double value, double min, double max)
        {
            if (value < min)
            {
                return min;
            }
            else if (value > max)
            {
                return max;
            }
            else
            {
                return value;
            }
        }
        /// <summary>
        /// Generates a color given a point within a range between two colors
        /// </summary>
        public static vColor GenerateColorVariance(double colorPoint, vColor firstColor, vColor secondColor)
        {
            if (colorPoint > 1)
            {
                colorPoint = 1;
            }
            if (colorPoint < 0)
            {
                colorPoint = 0;
            }

            double hueF = firstColor.H;
            double saturationF = firstColor.S;
            double brightnessF = firstColor.V;

            double hueS = secondColor.H;
            double saturationS = secondColor.S;
            double brightnessS = secondColor.V;

            double hue = Map(colorPoint, 0, 1, hueF, hueS);
            double saturation = Map(colorPoint, 0, 1, saturationF, saturationS);
            double brightness = Map(colorPoint, 0, 1, brightnessF, brightnessS);
            return vColor.HSV(hue, saturation, brightness);
        }
        /// <summary>
        /// Generates a color within a range between two colors
        /// </summary>
        public static vColor GenerateColorVariance(vColor firstColor, vColor secondColor)
        {
            return GenerateColorVariance(_random.NextDouble(), firstColor, secondColor);
        }
    }
    /// <summary>
    /// Resources specific to the falling sand game project
    /// </summary>
    public static class ProjectResource //
    {
        private static readonly Random _random = new Random();

        //Other people's functions:

        /// <summary>
        /// Generates a circle given a width.  From: <see href="https://stackoverflow.com/questions/17163636/filled-circle-in-matrix2d-array">(click here)</see>
        /// </summary>
        public static List<RelativeCoordinate> GenerateCoordinateCircle(int width)
        {
            if (width < 1)
            {
                return new List<RelativeCoordinate>() { RelativeCoordinate.Center };
            }
            List<RelativeCoordinate> _pointsInCircle = new List<RelativeCoordinate>();
            for (int i = 0 - width; i <= 0 + width; i++)
            {
                for (int j = 0 - width; j <= 0 + width; j++)
                {
                    if ((i) * (i) + (j) * (j) <= width * width)
                    {
                        _pointsInCircle.Add(new RelativeCoordinate(i, j));
                    }
                }
            }
            return _pointsInCircle;
        }

        //My functions: 
        /// <summary>
        /// Calculates the change in temperature for a block when two blocks interact for a single time step
        /// </summary>
        public static double CalculateEnergyExchange(double thisTemp, double thisSpecificHeatCapacity, double thisThermalConductivity, double otherTemp, double otherSpecificHeatCapacity, double otherThermalConductivity)
        {
            double finalTemp = (otherSpecificHeatCapacity * otherTemp + thisSpecificHeatCapacity * thisTemp) / (thisSpecificHeatCapacity + otherSpecificHeatCapacity); //calculates the final temp of the system
            double changeInTemp = finalTemp - thisTemp; //calculates the change in temp for this block
            double heatTransferAmount = (thisThermalConductivity + otherThermalConductivity) / 2; //calculates the proportional heat transfer that can occur
            return changeInTemp * heatTransferAmount; //the actual amount that is transfered is dependant on the amount that needs the be transfered and the ability of the two blocks to transfer heat
        }
        /// <summary>
        /// Removes a list of coordinates from a list of coordinates
        /// </summary>
        public static List<RelativeCoordinate> RemoveCoordinates(List<RelativeCoordinate> list, double density)
        {
            if (density == 1) //no need to waste time calulating for removing 0 blocks
            {
                return list;
            }
            List<RelativeCoordinate> returnList = list;
            int pointsToRemove = (int)Math.Floor(list.Count - list.Count * density);
            for (int i = 0; i < pointsToRemove; i++)
            {
                returnList.RemoveAt(_random.Next(0, returnList.Count));
            }
            return returnList;
        }
    }
}
