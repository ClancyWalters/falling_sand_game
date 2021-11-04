using LocalResouces;

namespace CustomProgram
{
    class NitrogenGasBlock : GasStateBlock, ICondenseable
    {
        static readonly private double _condensingTemperature = 77.2;
        static public NitrogenGasBlock InitalizeWithTemperature(double temperature)
        {
            return new NitrogenGasBlock(0.808, 1, 0.01, temperature, vColor.RGB(252, 252, 252), "Nitrogen Gas");
        }
        static public NitrogenGasBlock Initalize()
        {
            return NitrogenGasBlock.InitalizeWithTemperature(298.15);
        }
        private NitrogenGasBlock(double density, double specificHeatCapacity, double thermalConductivity, double temperature, vColor color, string name) : base(density, specificHeatCapacity, thermalConductivity, temperature, color, name) { }

        protected override ActionHandler PreDefaultQuery(GridBlockAPI gridAPI, GridCoordinate coordinate)
        {
            if (QueryCondense())
            {
                return new BlockChangeHandler(coordinate, NitrogenLiquidBlock.InitalizeWithTemperature(Temperature));
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
    class NitrogenLiquidBlock : LiquidStateBlock, IBoilable, IFreezeable
    {
        static readonly private double _boilingTemperature = 77.2;
        static readonly private double _freezingTemperature = 63.5;

        static public NitrogenLiquidBlock InitalizeWithTemperature(double temperature)
        {
            return new NitrogenLiquidBlock(0.808, 1, 0.1, temperature, GeneralResources.GenerateColorVariance(vColor.HSV(180, .15, .85), vColor.HSV(180, .20, .85)), "Liquid Nitrogen");
        }
        private NitrogenLiquidBlock(double density, double specificHeatCapacity, double thermalConductivity, double temperature, vColor color, string name) : base(density, specificHeatCapacity, thermalConductivity, temperature, color, name) { }

        protected override ActionHandler PreDefaultQuery(GridBlockAPI gridAPI, GridCoordinate coordinate)
        {
            if (QueryBoil())
            {
                return new BlockChangeHandler(coordinate, NitrogenGasBlock.InitalizeWithTemperature(Temperature));
            }
            if (QueryFreeze())
            {
                return new BlockChangeHandler(coordinate, NitrogenSolidBlock.InitalizeWithTemperature(Temperature));
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
    class NitrogenSolidBlock : SolidStateBlock, IMeltable
    {
        static readonly private double _meltingTemperature = 63.5;
        static public NitrogenSolidBlock InitalizeWithTemperature(double temperature)
        {
            return new NitrogenSolidBlock(8500, 1, 0.08, temperature, GeneralResources.GenerateColorVariance(vColor.HSV(180, .44, .89), vColor.HSV(180, .44, 1)), "Solid Nitrogen");
        }
        private NitrogenSolidBlock(double density, double specificHeatCapacity, double thermalConductivity, double temperature, vColor color, string name) : base(density, specificHeatCapacity, thermalConductivity, temperature, color, name) { }

        protected override ActionHandler PreDefaultQuery(GridBlockAPI gridAPI, GridCoordinate coordinate)
        {
            if (QueryMelt())
            {
                return new BlockChangeHandler(coordinate, NitrogenLiquidBlock.InitalizeWithTemperature(Temperature));
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
