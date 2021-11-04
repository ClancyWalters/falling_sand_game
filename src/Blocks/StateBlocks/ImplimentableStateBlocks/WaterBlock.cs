using LocalResouces;
namespace CustomProgram
{
    class SteamGasBlock : GasStateBlock, ICondenseable
    {
        readonly private double _condensingTemperature = 373.15;

        static public SteamGasBlock InitalizeWithTemperature(double temperature)
        {
            return new SteamGasBlock(0.590, 4.18, 0.001, temperature, GeneralResources.GenerateColorVariance(vColor.HSV(191, .15, .80), vColor.HSV(201, .10, .88)), "Steam");
        }
        private SteamGasBlock(double density, double specificHeatCapacity, double thermalConductivity, double temperature, vColor color, string name) : base(density, specificHeatCapacity, thermalConductivity, temperature, color, name) { }

        protected override ActionHandler PreDefaultQuery(GridBlockAPI gridAPI, GridCoordinate coordinate)
        {
            if (QueryCondense())
            {
                return new BlockChangeHandler(coordinate, WaterLiquidBlock.InitalizeWithTemperature(Temperature));
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
    class WaterLiquidBlock : LiquidStateBlock, IBoilable, IFreezeable
    {

        readonly private double _boilingTemperature = 373.15;
        readonly private double _freezingTemperature = 273.15;

        static public WaterLiquidBlock InitalizeWithTemperature(double temperature)
        {
            return new WaterLiquidBlock(1000, 4.18, 0.1, temperature, GeneralResources.GenerateColorVariance(vColor.HSV(214, .70, .65), vColor.HSV(214, .80, .75)), "Water");
        }
        static public WaterLiquidBlock Initalize()
        {
            return WaterLiquidBlock.InitalizeWithTemperature(298.15);
        }
        private WaterLiquidBlock(double density, double specificHeatCapacity, double thermalConductivity, double temperature, vColor color, string name) : base(density, specificHeatCapacity, thermalConductivity, temperature, color, name) { }

        protected override ActionHandler PreDefaultQuery(GridBlockAPI gridAPI, GridCoordinate coordinate)
        {
            if (QueryBoil())
            {
                return new BlockChangeHandler(coordinate, SteamGasBlock.InitalizeWithTemperature(Temperature));
            }
            if (QueryFreeze())
            {
                return new BlockChangeHandler(coordinate, IceSolidBlock.InitalizeWithTemperature(Temperature));
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
        public bool QueryFreeze()
        {
            return Temperature < _freezingTemperature;
        }
    }
    class IceSolidBlock : SolidStateBlock, IMeltable
    {
        readonly private double _meltingTemperature = 273.15;
        static public IceSolidBlock InitalizeWithTemperature(double temperature)
        {
            return new IceSolidBlock(920, 4.18, 0.08, temperature, GeneralResources.GenerateColorVariance(vColor.HSV(193, .7, 1), vColor.HSV(193, .7, .90)), "Ice");
        }
        private IceSolidBlock(double density, double specificHeatCapacity, double thermalConductivity, double temperature, vColor color, string name) : base(density, specificHeatCapacity, thermalConductivity, temperature, color, name) { }

        protected override ActionHandler PreDefaultQuery(GridBlockAPI gridAPI, GridCoordinate coordinate)
        {
            if (QueryMelt())
            {
                return new BlockChangeHandler(coordinate, WaterLiquidBlock.InitalizeWithTemperature(Temperature));
            }
            return null;
        }

        public override Block Clone()
        {
            return InitalizeWithTemperature(Temperature);
        }

        public bool QueryMelt()
        {
            return Temperature > _meltingTemperature;
        }
    }
}
