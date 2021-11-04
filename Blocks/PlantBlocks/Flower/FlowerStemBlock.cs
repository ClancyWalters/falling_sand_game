using LocalResouces;
using System;

namespace CustomProgram
{
    class FlowerStemBlock : PlantBlock
    {
        readonly private RelativeCoordinate _parentCoordinate;

        private static readonly Random _rngGenerator = new Random();

        static public FlowerStemBlock InitalizeWithTemperatureLifetimeAndParent(double temperature, int growthLifetime, RelativeCoordinate parentCoordinate)
        {
            return new FlowerStemBlock(2, 0.01, temperature, growthLifetime, parentCoordinate, GeneralResources.GenerateColorVariance(vColor.HSV(105, 1, .80), vColor.HSV(105, .80, .6)), "Flower Stem Block");
        }
        static public FlowerStemBlock InitalizeWithTemperatureAndLifetime(double temperature, int growthLifetime)
        {
            return InitalizeWithTemperatureLifetimeAndParent(temperature, growthLifetime, RelativeCoordinate.Down);
        }
        private FlowerStemBlock(double specificHeatCapacity, double thermalConductivity, double temperature, int growthLifetime, RelativeCoordinate parentCoordinate, vColor color, string name) : base(specificHeatCapacity, thermalConductivity, temperature, growthLifetime, color, name)
        {
            _parentCoordinate = parentCoordinate;
        }

        protected override ActionHandler GrowthQuery(GridBlockAPI gridAPI, GridCoordinate coordinate)
        {
            if (GrowthLifetime > 1)
            {
                if (GeneralResources.GetRandomBool(0.8))
                {
                    Block potentialGrowth = gridAPI.GetBlock(RelativeCoordinate.Up, coordinate);
                    if (potentialGrowth is AirGasBlock)
                    {
                        FinishedGrowing = true;
                        FlowerStemBlock block = InitalizeWithTemperatureAndLifetime(Temperature, GrowthLifetime - 1);
                        (block as IActable).HasUpdated = true;
                        return new BlockChangeHandler(RelativeCoordinate.Up.GetGridCoordinate(coordinate), block);
                    }
                }
                else
                {
                    if (GeneralResources.GetRandomBool(0.5))
                    {
                        Block potentialGrowth = gridAPI.GetBlock(RelativeCoordinate.UpRight, coordinate);
                        if (potentialGrowth is AirGasBlock)
                        {
                            FlowerStemBlock block = InitalizeWithTemperatureLifetimeAndParent(Temperature, 3, RelativeCoordinate.UpRight.GetMirrorCoordinate());
                            (block as IActable).HasUpdated = true;
                            return new BlockChangeHandler(RelativeCoordinate.UpRight.GetGridCoordinate(coordinate), block);
                        }
                    }
                    else
                    {
                        Block potentialGrowth = gridAPI.GetBlock(RelativeCoordinate.UpLeft, coordinate);
                        if (potentialGrowth is AirGasBlock)
                        {
                            FlowerStemBlock block = InitalizeWithTemperatureLifetimeAndParent(Temperature, 3, RelativeCoordinate.UpLeft.GetMirrorCoordinate());
                            (block as IActable).HasUpdated = true;
                            return new BlockChangeHandler(RelativeCoordinate.UpLeft.GetGridCoordinate(coordinate), block);
                        }
                        //generate leaf;
                    }
                }
            }
            else if (GrowthLifetime == 1)
            {
                Block potentialGrowth = gridAPI.GetBlock(RelativeCoordinate.Up, coordinate);
                if (potentialGrowth is AirGasBlock)
                {
                    FinishedGrowing = true;
                    FlowerBlock block = FlowerBlock.InitalizeWithTemperatureAndLifetime(Temperature, _rngGenerator.Next(1, 3));
                    (block as IActable).HasUpdated = true;
                    return new BlockChangeHandler(RelativeCoordinate.Up.GetGridCoordinate(coordinate), block);
                }
            }
            else
            {
                FinishedGrowing = true;
            }
            return null;
        }

        //check is supported
        protected override ActionHandler PostDefaultQuery(GridBlockAPI gridAPI, GridCoordinate coordinate)
        {
            Block parentBlock = gridAPI.GetBlock(_parentCoordinate, coordinate);
            if (!(parentBlock is FlowerStemBlock || parentBlock is GrassSolidBlock || parentBlock is DirtSolidBlock || parentBlock is MudSolidBlock))
            {
                return new TemperatureConsistantBlockChangeHandler(coordinate, AirGasBlock.Initalize());
            }
            return null;
        }
    }
}
