using LocalResouces;

namespace CustomProgram
{
    class AirGasBlock : GasStateBlock, ICondenseable
    {
        static readonly private double _condensingTemperature = 90.5;

        static public AirGasBlock InitalizeWithTemperature(double temperature)
        {
            return new AirGasBlock(0.645, 0.7171, 0.001, temperature, vColor.RGB(252, 252, 252), "Air");
        }
        static public AirGasBlock Initalize()
        {
            return AirGasBlock.InitalizeWithTemperature(298.15);
        }
        private AirGasBlock(double density, double specificHeatCapacity, double thermalConductivity, double temperature, vColor color, string name) : base(density, specificHeatCapacity, thermalConductivity, temperature, color, name) { }

        protected override ActionHandler PreDefaultQuery(GridBlockAPI gridAPI, GridCoordinate coordinate)
        {
            if (QueryCondense())
            {
                return new BlockChangeHandler(coordinate, AirLiquidBlock.InitalizeWithTemperature(Temperature));
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
    class AirLiquidBlock : LiquidStateBlock, IBoilable, IFreezeable
    {
        static readonly private double _boilingTemperature = 90.5;
        static readonly private double _freezingTemperature = 54.36;

        static public AirLiquidBlock InitalizeWithTemperature(double temperature)
        {
            return new AirLiquidBlock(0.645, 0.7171, 0.01, temperature, GeneralResources.GenerateColorVariance(vColor.HSV(0, 0, .87), vColor.HSV(0, 0, .93)), "Liquid Air");
        }
        private AirLiquidBlock(double density, double specificHeatCapacity, double thermalConductivity, double temperature, vColor color, string name) : base(density, specificHeatCapacity, thermalConductivity, temperature, color, name) { }

        protected override ActionHandler PreDefaultQuery(GridBlockAPI gridAPI, GridCoordinate coordinate)
        {
            if (QueryBoil())
            {
                return new BlockChangeHandler(coordinate, AirGasBlock.InitalizeWithTemperature(Temperature));
            }
            if (QueryFreeze())
            {
                return new BlockChangeHandler(coordinate, AirSolidBlock.InitalizeWithTemperature(Temperature));
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
    class AirSolidBlock : SolidStateBlock, IMeltable
    {
        static readonly private double _meltingTemperature = 54.36;
        static public AirSolidBlock InitalizeWithTemperature(double temperature)
        {
            return new AirSolidBlock(21000, 0.7171, 0.08, temperature, GeneralResources.GenerateColorVariance(vColor.HSV(186, .13, .54), vColor.HSV(186, .17, .54)), "Solid Air");
        }
        private AirSolidBlock(double density, double specificHeatCapacity, double thermalConductivity, double temperature, vColor color, string name) : base(density, specificHeatCapacity, thermalConductivity, temperature, color, name) { }

        protected override ActionHandler PreDefaultQuery(GridBlockAPI gridAPI, GridCoordinate coordinate)
        {
            if (QueryMelt())
            {
                return new BlockChangeHandler(coordinate, AirLiquidBlock.InitalizeWithTemperature(Temperature));
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
