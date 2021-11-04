using LocalResouces;
namespace CustomProgram
{
    class SmokeGasBlock : GasStateBlock, IFreezeable
    {
        private static readonly double _freezingTemperature = 350;
        static public SmokeGasBlock InitalizeWithTemperature(double temperature)
        {
            return new SmokeGasBlock(0.2, 0.7171, 0.05, temperature, GeneralResources.GenerateColorVariance(vColor.HSV(0, 0, .20), vColor.HSV(0, 0, .29)), "Smoke");
        }
        private SmokeGasBlock(double density, double specificHeatCapacity, double thermalConductivity, double temperature, vColor color, string name) : base(density, specificHeatCapacity, thermalConductivity, temperature, color, name) { }

        protected override ActionHandler PreDefaultQuery(GridBlockAPI gridAPI, GridCoordinate coordinate)
        {
            if (QueryFreeze())
            {
                if (GeneralResources.GetRandomBool(0.1))
                {
                    return new BlockChangeHandler(coordinate, AshSolidBlock.InitalizeWithTemperature(Temperature));
                }
                else
                {
                    return new TemperatureConsistantBlockChangeHandler(coordinate, CarbonDioxideGasBlock.Initalize());
                }

            }
            //turn to ash based on temperature
            return null;
        }

        public override Block Clone()
        {
            return InitalizeWithTemperature(Temperature);
        }
        public bool QueryFreeze()
        {
            return Temperature < _freezingTemperature;
        }
    }
}
