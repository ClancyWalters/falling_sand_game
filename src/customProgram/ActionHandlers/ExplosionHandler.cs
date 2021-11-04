using LocalResouces;
using System;
using System.Collections.Generic;

namespace CustomProgram
{
    class ExplosionHandler : ActionHandler
    {
        private static readonly Random _random = new Random();
        private static readonly double _specificHeatCapacity = 4;
        private static readonly double _thermalConductivity = 0.24;
        private readonly int _explosionRadius;
        private readonly double _explosionTemperature;
        private readonly GridCoordinate _centerPoint;

        public ExplosionHandler(int explosionRadius, double explosionTemperature, GridCoordinate centerPoint)
        {
            _explosionRadius = explosionRadius;
            _explosionTemperature = explosionTemperature;
            _centerPoint = centerPoint;
        }
        internal override void ExecuteAction(GridHandlerAPI gridAPI)
        {
            //get a circle of radius x which consists of points
            //the circle should possibly have randomness around the edges

            //for each point
            //if solid
            //if will liquidfy / evaporate then superheat
            //else remove
            //if liquid
            //if will evaporate then superheat
            //else remove
            //if gas
            //superheat it

            List<RelativeCoordinate> explosionCircle = ProjectResource.GenerateCoordinateCircle(_explosionRadius);
            List<RelativeCoordinate> limitedExplosionCircle = gridAPI.LimitCoordinates(explosionCircle, _centerPoint);
            Dictionary<RelativeCoordinate, Block> blockDict = gridAPI.GetBlockDictionary(limitedExplosionCircle, _centerPoint);
            foreach (RelativeCoordinate r in limitedExplosionCircle)
            {
                if (blockDict[r] is SolidStateBlock solidStateBlockR)
                {
                    double getTempChange = ProjectResource.CalculateEnergyExchange(solidStateBlockR.Temperature, solidStateBlockR.SpecificHeatCapacity, solidStateBlockR.ThermalConductivity, _explosionTemperature, _specificHeatCapacity, _thermalConductivity);
                    solidStateBlockR.ChangeTemperature(getTempChange);
                    if (blockDict[r] is IMeltable meltableR)
                    {
                        if (!meltableR.QueryMelt())
                        {
                            BlockReplacement(solidStateBlockR, r, gridAPI);
                        }
                    }
                    else if (blockDict[r] is IBoilable boilableR)
                    {
                        if (boilableR.QueryBoil())
                        {
                            BlockReplacement(solidStateBlockR, r, gridAPI);
                        }
                    }
                    else
                    {
                        BlockReplacement(solidStateBlockR, r, gridAPI);
                    }
                }
                else if (blockDict[r] is LiquidStateBlock liquidStateBlockR)
                {
                    double getTempChange = ProjectResource.CalculateEnergyExchange(liquidStateBlockR.Temperature, liquidStateBlockR.SpecificHeatCapacity, liquidStateBlockR.ThermalConductivity, _explosionTemperature, _specificHeatCapacity, _thermalConductivity);
                    liquidStateBlockR.ChangeTemperature(getTempChange);
                    if (blockDict[r] is IBoilable boilableR)
                    {
                        if (!boilableR.QueryBoil())
                        {
                            BlockReplacement(liquidStateBlockR, r, gridAPI);
                        }
                    }
                    else
                    {
                        BlockReplacement(liquidStateBlockR, r, gridAPI);
                    }
                }
                else if (blockDict[r] is GasStateBlock gasStateBlockR)
                {
                    double getTempChange = ProjectResource.CalculateEnergyExchange(gasStateBlockR.Temperature, gasStateBlockR.SpecificHeatCapacity, gasStateBlockR.ThermalConductivity, _explosionTemperature, _specificHeatCapacity, _thermalConductivity);
                    gasStateBlockR.ChangeTemperature(getTempChange);
                    if (GeneralResources.GetRandomBool(0.4))
                    {
                        BlockReplacement(gasStateBlockR, r, gridAPI);
                    }
                }
                else if (blockDict[r] is PlantBlock plantBlockR)
                {
                    BlockReplacement(plantBlockR, r, gridAPI);
                }
            }
        }

        private void BlockReplacement(ITemperature inputBlock, RelativeCoordinate r, GridHandlerAPI gridAPI)
        {
            StateBlock block = GenerateNewBlock(inputBlock.Temperature);
            UpdateTemperature(block);
            gridAPI.SetBlock(r.GetGridCoordinate(_centerPoint), block);
        }
        private StateBlock GenerateNewBlock(double temperature)
        {
            return (_random.Next(0, 4)) switch
            {
                0 => AirGasBlock.InitalizeWithTemperature(temperature),
                1 => CarbonDioxideGasBlock.InitalizeWithTemperature(temperature),
                2 => SmokeGasBlock.InitalizeWithTemperature(temperature),
                _ => EmberBlock.InitalizeWithTemperature(temperature),
            };
        }
        private void UpdateTemperature(ITemperature block)
        {
            double getTempChange = ProjectResource.CalculateEnergyExchange(block.Temperature, block.SpecificHeatCapacity, block.ThermalConductivity, _explosionTemperature, _specificHeatCapacity, _thermalConductivity);
            block.ChangeTemperature(getTempChange);
        }
    }
}
