using LocalResouces;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace CustomProgram
{
    class TemperatureComponent : ITemperature //using composition
    {
        readonly private double _specificHeatCapacity;
        readonly private double _thermalConductivity;
        private double _temperature;
        private vColor _savedColor;
        private double _savedTemperature;
        private int _savedMidpoint;
        public TemperatureComponent(double specifcHeatCapacity, double thermalConductivity, double temperature)
        {
            if (specifcHeatCapacity <= 0)
            {
                Debug.WriteLine("Specific Heat Capacity was entered by: " + this + " that was less than 0");
                _specificHeatCapacity = 0.1;
            }
            _specificHeatCapacity = specifcHeatCapacity;
            if (thermalConductivity >= 0.25) //thermalConductivity is a rate at which heat can be transfered from a block. 1 is 100%
            {
                Debug.WriteLine("Thermal Conductivity was entered by: " + this + " that was greater than 25!");
                _thermalConductivity = 0.245;
            }
            else if (thermalConductivity < 0)
            {
                Debug.WriteLine("Thermal Conductivity was entered by: " + this + " that was less than 0!");
                _thermalConductivity = 0;
            }
            else
            {
                _thermalConductivity = thermalConductivity;
            }
            if (temperature < 0)
            {
                _temperature = 0;
            }
            _temperature = temperature;
            _savedTemperature = Temperature;
            _savedColor = GetColor(500, 1000);
            _savedMidpoint = 500;
        }

        static private readonly List<RelativeCoordinate> _temperatureCheckedBlocks = new List<RelativeCoordinate>()
        {
            RelativeCoordinate.Up,
            RelativeCoordinate.Down,
            RelativeCoordinate.Left,
            RelativeCoordinate.Right
        };
        /// <summary>
        /// Performs temperature calculations
        /// </summary>
        public void TemperatureQuery(GridBlockAPI gridAPI, GridCoordinate blockLocation) //for each block around block it calculates temp change with it.
        {
            double totalTempChange = 0;
            List<CoordinateTemperaturePair> coordTempList = new List<CoordinateTemperaturePair>();

            foreach (RelativeCoordinate r in _temperatureCheckedBlocks)
            {
                if (gridAPI.GetBlock(r, blockLocation) is ITemperature temperatureB)
                {
                    double bTempChange = ProjectResource.CalculateEnergyExchange(temperatureB.Temperature, temperatureB.SpecificHeatCapacity, temperatureB.ThermalConductivity, Temperature, SpecificHeatCapacity, ThermalConductivity);
                    if (Math.Abs(bTempChange) > 0.1) //this alone saves 5ms/t due to not calculating tiny temperature variations
                    {
                        totalTempChange += ProjectResource.CalculateEnergyExchange(Temperature, SpecificHeatCapacity, ThermalConductivity, temperatureB.Temperature, temperatureB.SpecificHeatCapacity, temperatureB.ThermalConductivity);
                        CoordinateTemperaturePair pair = new CoordinateTemperaturePair(r, bTempChange);
                        coordTempList.Add(pair);
                    }
                }
            }
            if (totalTempChange > 0)
            {
                coordTempList.Add(new CoordinateTemperaturePair(new RelativeCoordinate(0, 0), totalTempChange));
            }
            gridAPI.AddAction(new TemperatureChangeHandler(coordTempList, blockLocation));
        }

        /// <summary>
        /// Changes block temperature
        /// </summary>
        public void ChangeTemperature(double amount)
        {
            if (Double.IsNaN(amount))
            {
                throw new Exception("Tried to change temperature by NaN");
            }
            else if (_temperature + amount < 0)
            {
                _temperature = 0;
            }
            else
            {
                _temperature += amount;
            }
        }
        /// <summary>
        /// Passes block color for temperature view
        /// </summary>
        public vColor DrawTemperature(int midPoint)
        {
            vColor color;

            if (_savedTemperature == Temperature && midPoint == _savedMidpoint)
            {
                color = _savedColor;
            }
            else
            {
                color = GetColor(midPoint, 1000);
                _savedColor = color;
                _savedMidpoint = midPoint;
                _savedTemperature = Temperature;
            }
            return color;
        }
        private vColor GetColor(int midPoint, int range)
        {
            int highpoint = midPoint + range / 2;
            int lowpoint = midPoint - range / 2;
            return GeneralResources.GenerateColorVariance(GeneralResources.Map(Temperature, lowpoint, highpoint, 0, 1), vColor.HSV(240, 1, 0.1), vColor.HSV(0, 1, 1));
        }
        public double Temperature
        {
            get => _temperature;
            set
            {
                if (value > 0)
                {
                    _temperature = value;
                }
            }
        }
        public double ThermalConductivity { get => _thermalConductivity; }
        public double SpecificHeatCapacity { get => _specificHeatCapacity; }
    }
}
