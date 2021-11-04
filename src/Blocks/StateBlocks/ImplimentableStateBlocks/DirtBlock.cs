using LocalResouces;
using System.Collections.Generic;

namespace CustomProgram
{
    class DirtSolidBlock : SolidStateBlock, IRandomActable
    {
        static public DirtSolidBlock InitalizeWithTemperature(double temperature)
        {
            return new DirtSolidBlock(1100, 0.8, 0.1, temperature, GeneralResources.GenerateColorVariance(vColor.HSV(32, .83, .57), vColor.HSV(32, .83, .50)), "Dirt");
        }
        static public DirtSolidBlock Initalize()
        {
            return DirtSolidBlock.InitalizeWithTemperature(298.15);
        }
        private DirtSolidBlock(double density, double specificHeatCapacity, double thermalConductivity, double temperature, vColor color, string name) : base(density, specificHeatCapacity, thermalConductivity, temperature, color, name) { }

        public ActionHandler RandomActionQuery(GridBlockAPI gridAPI, GridCoordinate coordinate)
        {
            if ((gridAPI.GetBlock(RelativeCoordinate.DownLeft, coordinate) is GrassSolidBlock || gridAPI.GetBlock(RelativeCoordinate.DownRight, coordinate) is GrassSolidBlock || gridAPI.GetBlock(RelativeCoordinate.Left, coordinate) is GrassSolidBlock || gridAPI.GetBlock(RelativeCoordinate.Right, coordinate) is GrassSolidBlock || gridAPI.GetBlock(RelativeCoordinate.UpRight, coordinate) is GrassSolidBlock || gridAPI.GetBlock(RelativeCoordinate.UpLeft, coordinate) is GrassSolidBlock) && gridAPI.GetBlock(RelativeCoordinate.Up, coordinate) is GasStateBlock)
            {
                return new BlockChangeHandler(coordinate, GrassSolidBlock.InitalizeWithTemperature(Temperature));
            }
            return null;
        }

        protected override ActionHandler PostDefaultQuery(GridBlockAPI gridAPI, GridCoordinate coordinate)
        {
            if (gridAPI.GetBlock(RelativeCoordinate.Up, coordinate) is WaterLiquidBlock)
            {
                return new BlockChangeHandler(coordinate, MudSolidBlock.InitalizeWithTemperature(Temperature));
            }
            if ((gridAPI.GetBlock(RelativeCoordinate.DownLeft, coordinate) is WaterLiquidBlock || gridAPI.GetBlock(RelativeCoordinate.DownRight, coordinate) is WaterLiquidBlock || gridAPI.GetBlock(RelativeCoordinate.Left, coordinate) is WaterLiquidBlock || gridAPI.GetBlock(RelativeCoordinate.Right, coordinate) is WaterLiquidBlock) && gridAPI.GetBlock(RelativeCoordinate.Up, coordinate) is GasStateBlock)
            {
                return new BlockChangeHandler(coordinate, GrassSolidBlock.InitalizeWithTemperature(Temperature));
            }
            return null;
        }

        public override Block Clone()
        {
            return InitalizeWithTemperature(Temperature);
        }
    }
    class GrassSolidBlock : SolidStateBlock
    {
        static public GrassSolidBlock InitalizeWithTemperature(double temperature)
        {
            return new GrassSolidBlock(1100, 0.8, 0.1, temperature, GeneralResources.GenerateColorVariance(vColor.HSV(119, .88, .63), vColor.HSV(119, .88, .73)), "Grass");
        }
        static public GrassSolidBlock Initalize()
        {
            return GrassSolidBlock.InitalizeWithTemperature(298.15);
        }
        private GrassSolidBlock(double density, double specificHeatCapacity, double thermalConductivity, double temperature, vColor color, string name) : base(density, specificHeatCapacity, thermalConductivity, temperature, color, name) { }


        protected override ActionHandler PostDefaultQuery(GridBlockAPI gridAPI, GridCoordinate coordinate)
        {
            Block block = gridAPI.GetBlock(RelativeCoordinate.Up, coordinate);

            if (!(block is GasStateBlock))
            {
                if (block is WaterLiquidBlock)
                {
                    return new BlockChangeHandler(coordinate, MudSolidBlock.InitalizeWithTemperature(Temperature));
                }
                return new BlockChangeHandler(coordinate, DirtSolidBlock.InitalizeWithTemperature(Temperature));
            }
            return null;
        }

        public override Block Clone()
        {
            return InitalizeWithTemperature(Temperature);
        }
    }
    class MudSolidBlock : SolidStateBlock
    {
        static readonly private List<RelativeCoordinate> _relavantBlocks = new List<RelativeCoordinate>()
        {
            RelativeCoordinate.Down, RelativeCoordinate.DownLeft, RelativeCoordinate.DownRight, RelativeCoordinate.Left, RelativeCoordinate.Right, RelativeCoordinate.Up, RelativeCoordinate.UpLeft, RelativeCoordinate.UpRight
        };

        static public MudSolidBlock InitalizeWithTemperature(double temperature)
        {
            return new MudSolidBlock(2700, 1480, 0.1, temperature, GeneralResources.GenerateColorVariance(vColor.HSV(32, .83, .17), vColor.HSV(32, .83, .26)), "Mud");
        }
        static public MudSolidBlock Initalize()
        {
            return MudSolidBlock.InitalizeWithTemperature(298.15);
        }
        private MudSolidBlock(double density, double specificHeatCapacity, double thermalConductivity, double temperature, vColor color, string name) : base(density, specificHeatCapacity, thermalConductivity, temperature, color, name) { }

        protected override ActionHandler PostDefaultQuery(GridBlockAPI gridAPI, GridCoordinate coordinate)
        {
            //Dictionary<RelativeCoordinate, Block> dict = gridAPI.GetBlockDictionary(_relavantBlocks, coordinate);
            bool isWater = false;
            foreach (RelativeCoordinate r in _relavantBlocks)
            {
                if (gridAPI.GetBlock(r, coordinate) is WaterLiquidBlock)
                {
                    isWater = true;
                }
            }
            if (!isWater)
            {
                return new BlockChangeHandler(coordinate, DirtSolidBlock.InitalizeWithTemperature(Temperature));
            }
            return null;
        }

        public override Block Clone()
        {
            return InitalizeWithTemperature(Temperature);
        }
    }
}
