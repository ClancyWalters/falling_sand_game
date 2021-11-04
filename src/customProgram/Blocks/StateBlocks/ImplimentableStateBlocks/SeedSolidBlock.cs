using LocalResouces;
using System;
namespace CustomProgram
{
    class SeedSolidBlock : SolidStateBlock
    {
        private static readonly Random _rngGenerator = new Random();
        private static readonly TrunkFactory _trunkFactory = new TrunkFactory();
        static public SeedSolidBlock InitalizeWithTemperature(double temperature)
        {
            return new SeedSolidBlock(1100, 1.76, 0.2, temperature, GeneralResources.GenerateColorVariance(vColor.HSV(80, .70, .79), vColor.HSV(95, .70, .79)), "Seed");
        }
        static public SeedSolidBlock Initalize()
        {
            return SeedSolidBlock.InitalizeWithTemperature(298.15);
        }
        private SeedSolidBlock(double density, double specificHeatCapacity, double thermalConductivity, double temperature, vColor color, string name) : base(density, specificHeatCapacity, thermalConductivity, temperature, color, name) { }

        protected override ActionHandler PreDefaultQuery(GridBlockAPI gridAPI, GridCoordinate coordinate)
        {
            Block block = gridAPI.GetBlock(RelativeCoordinate.Down, coordinate);
            if (block is GrassSolidBlock || block is MudSolidBlock || block is DirtSolidBlock)
            {
                if (GeneralResources.GetRandomBool(0.5))
                {
                    return new BlockChangeHandler(coordinate, _trunkFactory.CreateTrunk(_trunkFactory.TreeTypes[_rngGenerator.Next(0, _trunkFactory.TreeTypes.Count)]));
                }
                else
                {
                    return new BlockChangeHandler(coordinate, FlowerStemBlock.InitalizeWithTemperatureAndLifetime(Temperature, 20));
                }
            }
            else if (block is WaterLiquidBlock)
            {
                return new BlockChangeHandler(coordinate, AlgueSolidBlock.InitalizeWithTemperature(Temperature));
            }
            return null;
        }
        protected override ActionHandler MovementQuery(GridBlockAPI gridAPI, GridCoordinate coordinate) //seeds fall straight down
        {
            Block block = gridAPI.GetBlock(RelativeCoordinate.Down, coordinate);

            if (block is StateBlock && !(block is SolidStateBlock))
            {
                if (!(block as StateBlock).HasUpdated || block is GasStateBlock)
                {
                    if ((block as StateBlock).Density < Density)
                    {
                        return new BlockSwitchHandler(coordinate, RelativeCoordinate.Down);
                    }
                }
            }
            return null;
        }
        protected override ActionHandler PostDefaultQuery(GridBlockAPI gridAPI, GridCoordinate coordinate)
        {
            Block block = gridAPI.GetBlock(RelativeCoordinate.Down, coordinate);
            if (!(block is GasStateBlock)) //if it is stopped by some liquid / solid and does not become a plant it will just dissapear
            {
                return new TemperatureConsistantBlockChangeHandler(coordinate, AirGasBlock.Initalize());
            }
            return null;
        }

        public override Block Clone()
        {
            return InitalizeWithTemperature(Temperature);
        }
    }
}
