using LocalResouces;
using System.Collections.Generic;

namespace CustomProgram
{
    class HydrogenGasBlock : GasStateBlock, ICondenseable
    {
        static readonly private List<RelativeCoordinate> _fireChecks = new List<RelativeCoordinate>() //asks for all the blocks around itself to check oxygen
        {
            RelativeCoordinate.Down, RelativeCoordinate.DownLeft, RelativeCoordinate.DownRight, RelativeCoordinate.Left, RelativeCoordinate.Right, RelativeCoordinate.Up, RelativeCoordinate.UpLeft, RelativeCoordinate.UpRight
        };

        static readonly private double _condensingTemperature = 20.28;

        static public HydrogenGasBlock InitalizeWithTemperature(double temperature)
        {
            return new HydrogenGasBlock(0.082, 0.01312, 0.001, temperature, GeneralResources.GenerateColorVariance(vColor.HSV(0, .02, .96), vColor.HSV(0, .07, .96)), "Hydrogen Gas");
        }
        static public HydrogenGasBlock Initalize()
        {
            return HydrogenGasBlock.InitalizeWithTemperature(298.15);
        }
        private HydrogenGasBlock(double density, double specificHeatCapacity, double thermalConductivity, double temperature, vColor color, string name) : base(density, specificHeatCapacity, thermalConductivity, temperature, color, name) { }

        protected override ActionHandler PreDefaultQuery(GridBlockAPI gridAPI, GridCoordinate coordinate)
        {
            //Dictionary<RelativeCoordinate, Block> dict = gridAPI.GetBlockDictionary(_fireChecks, coordinate);
            foreach (RelativeCoordinate r in _fireChecks)
            {
                if (gridAPI.GetBlock(r, coordinate) is IBurningInterface)
                {
                    if ((gridAPI.GetBlock(r, coordinate) as IBurningInterface).Burning)
                    {
                        return new ExplosionHandler(7, 1500, coordinate);
                    }
                }
            }
            if (QueryCondense())
            {
                return new BlockChangeHandler(coordinate, HydrogenLiquidBlock.InitalizeWithTemperature(Temperature));
            }
            return null;
        }
        public override Block Clone()
        {
            return InitalizeWithTemperature(Temperature);
        }
        public bool QueryCondense()
        {
            return Temperature < _condensingTemperature;
        }
    }
    class HydrogenLiquidBlock : LiquidStateBlock, IBoilable, IBurningInterface
    {
        static readonly private double _boilingTemperature = 20.28;

        readonly private FireComponent _fire;
        private bool _burning;
        static public HydrogenLiquidBlock InitalizeWithTemperature(double temperature)
        {
            return new HydrogenLiquidBlock(70, 0.01312, 0.01, temperature, GeneralResources.GenerateColorVariance(vColor.HSV(0, .2, .8), vColor.HSV(0, .32, .91)), "Liquid Hydrogen");
        }
        private HydrogenLiquidBlock(double density, double specificHeatCapacity, double thermalConductivity, double temperature, vColor color, string name) : base(density, specificHeatCapacity, thermalConductivity, temperature, color, name)
        {
            _burning = false;
            _fire = new FireComponent(200);
        }

        protected override ActionHandler PreDefaultQuery(GridBlockAPI gridAPI, GridCoordinate coordinate)
        {
            if (_burning)
            {
                return _fire.CheckHasOxygen(gridAPI, coordinate, Temperature);
            }
            else if (_fire.CanIgnite(gridAPI, coordinate))
            {
                _burning = true;
                _fire.Ignite(this);
            }
            if (QueryBoil())
            {
                return new BlockChangeHandler(coordinate, HydrogenGasBlock.InitalizeWithTemperature(Temperature));
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
        public bool QueryBoil()
        {
            return Temperature > _boilingTemperature;
        }
        public bool Burning { get => _burning; }
    }
}
