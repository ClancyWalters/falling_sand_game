namespace CustomProgram
{
    /// <summary>
    /// Base block for blocks with density
    /// </summary>
    abstract class StateBlock : Block, ITemperature, ICloneable, IPublicDensity
    {
        readonly private TemperatureComponent _tempeatureComponent;

        private bool _hasUpdated = false;
        readonly protected double _density;

        public StateBlock(double density, double specificHeatCapacity, double thermalConductivity, double temperature, vColor color, string name) : base(color, name)
        {
            _density = density;
            _tempeatureComponent = new TemperatureComponent(specificHeatCapacity, thermalConductivity, temperature);
        }
        /// <summary>
        /// Performs temperature calculations
        /// </summary>
        public void TemperatureQuery(GridBlockAPI gridAPI, GridCoordinate blockLocation) //for each block around block it calculates temp change with it.
        {
            _tempeatureComponent.TemperatureQuery(gridAPI, blockLocation);
        }
        /// <summary>
        /// Passes block color for temperature view
        /// </summary>
        public vColor DrawTemperature(int midpoint)
        {
            return _tempeatureComponent.DrawTemperature(midpoint);
        }
        /// <summary>
        /// Changes block temperature
        /// </summary>
        public void ChangeTemperature(double amount)
        {
            _tempeatureComponent.ChangeTemperature(amount);
        }

        public abstract Block Clone();

        public bool HasUpdated { get => _hasUpdated; set => _hasUpdated = value; }
        public double Density { get => _density; }
        public double ThermalConductivity { get => _tempeatureComponent.ThermalConductivity; }
        public double SpecificHeatCapacity { get => _tempeatureComponent.SpecificHeatCapacity; }
        public double Temperature { get => _tempeatureComponent.Temperature; set => _tempeatureComponent.Temperature = value; }
    }
}