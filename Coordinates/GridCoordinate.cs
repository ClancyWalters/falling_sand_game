namespace CustomProgram
{
    /// <summary>
    /// Holds an X and Y relative to the Grid
    /// </summary>
    public struct GridCoordinate
    {
        public readonly int X;
        public readonly int Y;
        /// <summary>
        /// Creates a Grid Coordinate. each X, Y corresponds to a location within the grid object
        /// </summary>
        public GridCoordinate(int x, int y)
        {
            X = x;
            Y = y;
        }
        /// <summary>
        /// Calculates an AbsoluteCoordinate given the grid's origin and scale.
        /// </summary>
        /// <returns>Returns an absolute coordinate corresponding to the top left pixel of the grid coordinate</returns>
        public AbsoluteCoordinate GetAbsoluteCoordinate(AbsoluteCoordinate origin, int scale)
        {
            //start by translating to origin
            int x = origin.X;
            int y = origin.Y;
            //get point from coord*scale
            x += (X * scale);
            y += (Y * scale);
            return new AbsoluteCoordinate(x, y);
        }
    }
}

