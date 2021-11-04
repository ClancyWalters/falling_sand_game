using LocalResouces;
using System.Collections.Generic;

namespace CustomProgram
{
    class AlgueSolidBlock : SolidStateBlock, IBurningInterface
    {
        readonly private FireComponent _fire;
        private bool _burning;
        private int _lifetime = 5;

        static readonly private List<RelativeCoordinate> _algueGrowthBlocks = new List<RelativeCoordinate>() //asks for all the blocks around itself to check oxygen
        {
            RelativeCoordinate.DownLeft, RelativeCoordinate.DownRight, RelativeCoordinate.Left, RelativeCoordinate.Right, RelativeCoordinate.UpLeft, RelativeCoordinate.UpRight
        };
        static public AlgueSolidBlock InitalizeWithTemperature(double temperature)
        {
            return new AlgueSolidBlock(800, 1.76, 0.02, temperature, GeneralResources.GenerateColorVariance(vColor.HSV(136, .96, .2), vColor.HSV(136, .81, .39)), "Algue");
        }
        static public AlgueSolidBlock Initalize()
        {
            return AlgueSolidBlock.InitalizeWithTemperature(298.15);
        }
        private AlgueSolidBlock(double density, double specificHeatCapacity, double thermalConductivity, double temperature, vColor color, string name) : base(density, specificHeatCapacity, thermalConductivity, temperature, color, name)
        {
            _fire = new FireComponent(30);
            _burning = false;
        }
        protected override ActionHandler PreDefaultQuery(GridBlockAPI gridAPI, GridCoordinate coordinate)
        {
            if (_lifetime < 1)
            {
                return new TemperatureConsistantBlockChangeHandler(coordinate, AirGasBlock.Initalize());
            }
            if (_burning)
            {
                return _fire.CheckHasOxygen(gridAPI, coordinate, Temperature);
            }
            else if (_fire.CanIgnite(gridAPI, coordinate))
            {
                _burning = true;
                _fire.Ignite(this);
            }
            if (_lifetime > 0)
            {
                if (!(gridAPI.GetBlock(RelativeCoordinate.Down, coordinate) is WaterLiquidBlock))
                {
                    _lifetime--;
                }
            }
            if (GeneralResources.GetRandomBool(0.1))
            {
                //Dictionary<RelativeCoordinate, Block> dict = gridAPI.GetBlockDictionary(_algueGrowthBlocks, coordinate);
                //check l r
                if (GeneralResources.GetRandomBool(0.5))
                {
                    if (gridAPI.GetBlock(RelativeCoordinate.DownLeft, coordinate) is WaterLiquidBlock && gridAPI.GetBlock(RelativeCoordinate.Left, coordinate) is AirGasBlock)
                    {
                        return new BlockChangeHandler(RelativeCoordinate.Left.GetGridCoordinate(coordinate), AlgueSolidBlock.InitalizeWithTemperature(Temperature));
                    }
                    if (gridAPI.GetBlock(RelativeCoordinate.DownRight, coordinate) is WaterLiquidBlock && gridAPI.GetBlock(RelativeCoordinate.Right, coordinate) is AirGasBlock)
                    {
                        return new BlockChangeHandler(RelativeCoordinate.Right.GetGridCoordinate(coordinate), AlgueSolidBlock.InitalizeWithTemperature(Temperature));
                    }
                }
                else
                {
                    if (gridAPI.GetBlock(RelativeCoordinate.DownRight, coordinate) is WaterLiquidBlock && gridAPI.GetBlock(RelativeCoordinate.Right, coordinate) is AirGasBlock)
                    {
                        return new BlockChangeHandler(RelativeCoordinate.Right.GetGridCoordinate(coordinate), AlgueSolidBlock.InitalizeWithTemperature(Temperature));
                    }
                    if (gridAPI.GetBlock(RelativeCoordinate.DownLeft, coordinate) is WaterLiquidBlock && gridAPI.GetBlock(RelativeCoordinate.Left, coordinate) is AirGasBlock)
                    {
                        return new BlockChangeHandler(RelativeCoordinate.Left.GetGridCoordinate(coordinate), AlgueSolidBlock.InitalizeWithTemperature(Temperature));
                    }
                }
                //check ul ur
                if (GeneralResources.GetRandomBool(0.5))
                {
                    if (gridAPI.GetBlock(RelativeCoordinate.Left, coordinate) is WaterLiquidBlock && gridAPI.GetBlock(RelativeCoordinate.UpLeft, coordinate) is AirGasBlock)
                    {
                        return new BlockChangeHandler(RelativeCoordinate.UpLeft.GetGridCoordinate(coordinate), AlgueSolidBlock.InitalizeWithTemperature(Temperature));
                    }
                    if (gridAPI.GetBlock(RelativeCoordinate.Right, coordinate) is WaterLiquidBlock && gridAPI.GetBlock(RelativeCoordinate.UpRight, coordinate) is AirGasBlock)
                    {
                        return new BlockChangeHandler(RelativeCoordinate.UpRight.GetGridCoordinate(coordinate), AlgueSolidBlock.InitalizeWithTemperature(Temperature));
                    }
                }
                else
                {
                    if (gridAPI.GetBlock(RelativeCoordinate.Right, coordinate) is WaterLiquidBlock && gridAPI.GetBlock(RelativeCoordinate.UpRight, coordinate) is AirGasBlock)
                    {
                        return new BlockChangeHandler(RelativeCoordinate.UpRight.GetGridCoordinate(coordinate), AlgueSolidBlock.InitalizeWithTemperature(Temperature));
                    }
                    if (gridAPI.GetBlock(RelativeCoordinate.Left, coordinate) is WaterLiquidBlock && gridAPI.GetBlock(RelativeCoordinate.UpLeft, coordinate) is AirGasBlock)
                    {
                        return new BlockChangeHandler(RelativeCoordinate.UpLeft.GetGridCoordinate(coordinate), AlgueSolidBlock.InitalizeWithTemperature(Temperature));
                    }
                }
            }
            return null;
        }
        protected override ActionHandler PostDefaultQuery(GridBlockAPI gridAPI, GridCoordinate coordinate)
        {
            if (_burning)
            {
                return _fire.GenerateParticles(gridAPI, coordinate, Temperature);
            }
            return null;
        }

        public override Block Clone()
        {
            return InitalizeWithTemperature(Temperature);
        }

        public FireComponent Fire { get => _fire; }
        public bool Burning { get => _burning; }
    }
}
