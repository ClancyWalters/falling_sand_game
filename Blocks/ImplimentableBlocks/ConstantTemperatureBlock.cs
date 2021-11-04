using LocalResouces;
using System;
using System.Collections.Generic;
namespace CustomProgram
{
    class ConstantTemperatureBlock : Block, ICloneable, ITemperature //this block will maintain a constant temperature meaning it causing its surroundings to heat and cool appropriately
    {
        readonly private TemperatureComponent _temperatureComponent;

        static private readonly List<RelativeCoordinate> _temperatureCheckedBlocks = new List<RelativeCoordinate>()
        {
            new RelativeCoordinate(0, -1),
            new RelativeCoordinate(0, 1),
            new RelativeCoordinate(-1, 0),
            new RelativeCoordinate(1, 0)
        };

        static public ConstantTemperatureBlock InitalizeWithTemperature(double temperature)
        {
            return new ConstantTemperatureBlock(4, 0.245, temperature, GeneralResources.GenerateColorVariance(GeneralResources.Map(temperature, 20, 600, 0, 1), vColor.HSV(240, .94, 0.5), vColor.HSV(359, 1, 1)), "Constant Temperature Block (" + temperature + "K)"); //color based on temperature
        }
        public ConstantTemperatureBlock(double specificHeatCapacity, double thermalConductivity, double temperature, vColor color, string name) : base(color, name)
        {
            _temperatureComponent = new TemperatureComponent(specificHeatCapacity, thermalConductivity, temperature);
        }

        public void TemperatureQuery(GridBlockAPI gridAPI, GridCoordinate blockLocation) //different from the component
        {
            List<CoordinateTemperaturePair> coordTempList = new List<CoordinateTemperaturePair>();

            foreach (RelativeCoordinate r in _temperatureCheckedBlocks)
            {
                Block b = gridAPI.GetBlock(r, blockLocation);
                if (b is StateBlock)
                {
                    if (Math.Abs((b as StateBlock).Temperature - Temperature) > 0.1)
                    {
                        double bTempChange = ProjectResource.CalculateEnergyExchange((b as StateBlock).Temperature, (b as StateBlock).SpecificHeatCapacity, (b as StateBlock).ThermalConductivity, Temperature, SpecificHeatCapacity, ThermalConductivity);

                        CoordinateTemperaturePair pair = new CoordinateTemperaturePair(r, bTempChange);
                        coordTempList.Add(pair);
                    }
                }
            }
            gridAPI.AddAction(new TemperatureChangeHandler(coordTempList, blockLocation));
        }

        public void ChangeTemperature(double Temperature) { } //different from the component
        public vColor DrawTemperature(int midPoint) //different from component
        {
            return _temperatureComponent.DrawTemperature(midPoint);
        }

        public Block Clone()
        {
            return InitalizeWithTemperature(Temperature);
        }

        public double Temperature { get => _temperatureComponent.Temperature; set => _temperatureComponent.Temperature = value; }
        public double SpecificHeatCapacity { get => _temperatureComponent.SpecificHeatCapacity; }
        public double ThermalConductivity { get => _temperatureComponent.ThermalConductivity; }


    }
}
