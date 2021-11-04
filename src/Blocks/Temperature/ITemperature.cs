using CustomProgram.Blocks.Temperature;

namespace CustomProgram
{
    interface ITemperature : IPublicTemperature
    {
        /// <summary>
        /// Passes block color for temperature view
        /// </summary>
        vColor DrawTemperature(int midPoint);
        /// <summary>
        /// Performs temperature calculations
        /// </summary>
        void TemperatureQuery(GridBlockAPI gridAPI, GridCoordinate blockLocation);
        /// <summary>
        /// Changes block temperature
        /// </summary>
        void ChangeTemperature(double Temperature);
        new double Temperature { get; set; }
        double ThermalConductivity { get; }
        double SpecificHeatCapacity { get; }
    }
}
