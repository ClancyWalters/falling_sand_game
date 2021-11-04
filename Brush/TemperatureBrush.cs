namespace CustomProgram
{
    public class TemperatureBrush : Brush
    {
        private double _temperatureDifferential;
        /// <summary>
        /// Creates a TemperatureBrush
        /// </summary>
        public TemperatureBrush(int width, int density, double temperatureDifferential) : base(width, density)
        {
            _temperatureDifferential = temperatureDifferential;
        }
        /// <summary>
        /// Adds a TemperatureDrawingHandler to the model's action list
        /// </summary>
        public override void Draw(IModel model, AbsoluteCoordinate coord)
        {
            model.AddAction(new TemperatureDrawingHandler(coord, CashedBrush, _temperatureDifferential));
        }
        /// <summary>
        /// Returns the difference in temperature that will be used by the TemperatureDrawingHandler
        /// </summary>
        public double TemperatureDifferential
        {
            get => _temperatureDifferential;
            set { if (_temperatureDifferential != value) { _temperatureDifferential = value; }; }
        }
    }
}
