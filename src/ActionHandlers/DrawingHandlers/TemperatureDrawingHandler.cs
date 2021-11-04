using LocalResouces;
using System.Collections.Generic;

namespace CustomProgram
{
    /// <summary>
    /// Changes the temperature of all blocks within its radius by an amount determined by the temperatureDifference
    /// </summary>
    class TemperatureDrawingHandler : DrawingHandler
    {
        private readonly double _thermalConductivity = 0.25;
        private readonly double _specificHeatCapacity = 15;
        private readonly double _temperatureDifference;
        public TemperatureDrawingHandler(AbsoluteCoordinate origin, List<RelativeCoordinate> coordinates, double temperatureDifference) : base(origin, coordinates)
        {
            _temperatureDifference = temperatureDifference;
        }

        internal override void ExecuteAction(GridHandlerAPI gridAPI)
        {
            GridCoordinate origin = MousePosition.GetGridCoordinate(gridAPI.Scale, gridAPI.Origin);
            if (gridAPI.GetBlockCheck(origin))
            {
                foreach (RelativeCoordinate r in IncludedPoints)
                {
                    if (gridAPI.GetBlockCheck(origin, r))
                    {
                        Block block = gridAPI.GetBlock(origin, r);
                        if (block is ITemperature temperatureBlock)
                        {
                            double temperatureTarget = temperatureBlock.Temperature + _temperatureDifference;
                            if (temperatureTarget <= 0)
                            {
                                temperatureTarget = 0.01;
                            }

                            double getTempChange = ProjectResource.CalculateEnergyExchange(temperatureBlock.Temperature, temperatureBlock.SpecificHeatCapacity, temperatureBlock.ThermalConductivity, temperatureTarget, _specificHeatCapacity, _thermalConductivity);

                            temperatureBlock.ChangeTemperature(getTempChange);
                        }
                    }
                }
            }
        }
    }
}
