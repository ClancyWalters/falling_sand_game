using System;

namespace CustomProgram
{
    /// <summary>
    /// Holds an X and Y corresponding to a pixel on the window
    /// </summary>
    public struct AbsoluteCoordinate
    {
        public readonly int X;
        public readonly int Y;
        /// <summary>
        /// Creates an AbsoluteCoordinate that corresponds to a pixel position on the window
        /// </summary>
        public AbsoluteCoordinate(int x, int y)
        {
            X = x;
            Y = y;
        }
        /// <summary>
        /// Calculate the corresponding GridCoordinate using the origin and scale of the grid. This coordinate may be outside the bounds of the grid.
        /// </summary>
        /// <returns>Returns a GridCoordinate corresponding to the absolute coordinate entered</returns>
        public GridCoordinate GetGridCoordinate(int scale, AbsoluteCoordinate origin) //calculating a grid coordinate from a absolute coordinate requires: a scale and a origin
        {
            double x = X;
            double y = Y;
            //add absolute values
            x -= origin.X;
            y -= origin.Y;
            //divide by scale
            x /= scale;
            y /= scale;
            //floor it
            x = Math.Floor(x);
            y = Math.Floor(y);
            //convert to int
            int intx = Convert.ToInt32(x);
            int inty = Convert.ToInt32(y);
            //return as a grid coordinate
            return new GridCoordinate(intx, inty);
        }
    }
}
