namespace CustomProgram
{
    /// <summary>
    /// Holds an x and y relative to 0, 0
    /// </summary>
    public struct RelativeCoordinate
    {
        //Inspired by Xna.Framework.Color
        //These are static relative coordinates that can be referened
        //like RelativeCoordinate.Down
        static RelativeCoordinate()
        {
            Down = new RelativeCoordinate(0, 1);
            DownLeft = new RelativeCoordinate(-1, 1);
            DownRight = new RelativeCoordinate(1, 1);
            Left = new RelativeCoordinate(-1, 0);
            Right = new RelativeCoordinate(1, 0);
            Up = new RelativeCoordinate(0, -1);
            UpLeft = new RelativeCoordinate(-1, -1);
            UpRight = new RelativeCoordinate(1, -1);
            Center = new RelativeCoordinate(0, 0);
        }
        static public RelativeCoordinate Down { get; private set; }
        static public RelativeCoordinate DownLeft { get; private set; }
        static public RelativeCoordinate DownRight { get; private set; }
        static public RelativeCoordinate Left { get; private set; }
        static public RelativeCoordinate Right { get; private set; }
        static public RelativeCoordinate Up { get; private set; }
        static public RelativeCoordinate UpLeft { get; private set; }
        static public RelativeCoordinate UpRight { get; private set; }
        static public RelativeCoordinate Center { get; private set; }


        public readonly int X;
        public readonly int Y;
        /// <summary>
        /// Creates a relative coordinate
        /// </summary>
        public RelativeCoordinate(int x, int y)
        {
            X = x;
            Y = y;
        }
        /// <summary>
        /// Converts this relative coordinate into a GridCoordinate.
        /// </summary>
        /// <returns>A Grid Coordinate</returns>
        public GridCoordinate GetGridCoordinate(GridCoordinate originLocation)
        {
            int newX = originLocation.X + X;
            int newY = originLocation.Y + Y;
            return new GridCoordinate(newX, newY);
        }
        /// <summary>
        /// Calculates the coordinate with opposite values with respect to 0,0
        /// </summary>
        /// <returns>Returns a RelativeCoordinate</returns>
        public RelativeCoordinate GetMirrorCoordinate()
        {
            return new RelativeCoordinate(X * -1, Y * -1);
        }
    }
}
