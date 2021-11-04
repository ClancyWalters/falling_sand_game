namespace CustomProgram
{
    class TemperaturePermiableBarrierBlock : Block, ICloneable, ITemperature //pretty much the simplest implementation of ITemperature possible
    {
        readonly private TemperatureComponent _temperatureComponent;
        static public TemperaturePermiableBarrierBlock InitalizeWithAllTemperatureValues(double specificHeatCapacity, double thermalConductivity, double temperature)
        {
            return new TemperaturePermiableBarrierBlock(specificHeatCapacity, thermalConductivity, temperature, vColor.RGB(0, 0, 0), "Temperature Permiable Barrier Block"); //color based on temperature
        }
        static public TemperaturePermiableBarrierBlock InitalizeWithTemperature(double temperature)
        {
            return InitalizeWithAllTemperatureValues(4, 0.245, temperature); //color based on temperature
        }
        public TemperaturePermiableBarrierBlock(double specificHeatCapacity, double thermalConductivity, double temperature, vColor color, string name) : base(color, name)
        {
            _temperatureComponent = new TemperatureComponent(specificHeatCapacity, thermalConductivity, temperature);
        }

        public void TemperatureQuery(GridBlockAPI gridAPI, GridCoordinate blockLocation)
        {
            _temperatureComponent.TemperatureQuery(gridAPI, blockLocation);
        }

        public void ChangeTemperature(double Temperature)
        {
            _temperatureComponent.ChangeTemperature(Temperature);
        }

        public vColor DrawTemperature(int midpoint)
        {
            return _temperatureComponent.DrawTemperature(midpoint);
        }

        public Block Clone()
        {
            return InitalizeWithAllTemperatureValues(SpecificHeatCapacity, ThermalConductivity, Temperature);
        }

        public double Temperature { get => _temperatureComponent.Temperature; set => _temperatureComponent.Temperature = value; }
        public double SpecificHeatCapacity { get => _temperatureComponent.SpecificHeatCapacity; }
        public double ThermalConductivity { get => _temperatureComponent.ThermalConductivity; }


    }
}
