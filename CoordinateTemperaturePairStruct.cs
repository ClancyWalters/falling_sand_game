namespace CustomProgram
{
    public struct CoordinateTemperaturePair
    {
        readonly private RelativeCoordinate _coordinate;
        readonly private double _temperature;
        public CoordinateTemperaturePair(RelativeCoordinate coordinate, double deltaT)
        {
            _coordinate = coordinate;
            _temperature = deltaT;
        }
        public RelativeCoordinate Coordinate { get => _coordinate; }
        public double Temperature { get => _temperature; }
    }
}
