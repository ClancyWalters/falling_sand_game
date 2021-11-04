using LocalResouces;

namespace CustomProgram
{
    class GlassLiquidBlock : LiquidStateBlock, IFreezeable
    {
        static readonly private double _freezingTemperature = 1973.15;

        static public GlassLiquidBlock InitalizeWithTemperature(double temperature)
        {
            return new GlassLiquidBlock(2500, 0.840, 0.1, temperature, GeneralResources.GenerateColorVariance(vColor.HSV(0, .0, .86), vColor.HSV(0, .0, .93)), "Liquid Glass");
        }
        private GlassLiquidBlock(double density, double specificHeatCapacity, double thermalConductivity, double temperature, vColor color, string name) : base(density, specificHeatCapacity, thermalConductivity, temperature, color, name) { }

        protected override ActionHandler PreDefaultQuery(GridBlockAPI gridAPI, GridCoordinate coordinate)
        {
            if (QueryFreeze())
            {
                return new BlockChangeHandler(coordinate, GlassSolidBlock.InitalizeWithTemperature(Temperature));
            }
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
    class GlassSolidBlock : SolidStateBlock, IMeltable
    {
        static readonly private double _meltingTemperature = 1973.15;
        static public GlassSolidBlock InitalizeWithTemperature(double temperature)
        {
            return new GlassSolidBlock(2500, 0.840, 0.001, temperature, GeneralResources.GenerateColorVariance(vColor.HSV(0, .0, .86), vColor.HSV(0, .0, .93)), "Glass");
        }
        static public GlassSolidBlock Initalize()
        {
            return GlassSolidBlock.InitalizeWithTemperature(298.15);
        }
        private GlassSolidBlock(double density, double specificHeatCapacity, double thermalConductivity, double temperature, vColor color, string name) : base(density, specificHeatCapacity, thermalConductivity, temperature, color, name) { }
        protected override ActionHandler PreDefaultQuery(GridBlockAPI gridAPI, GridCoordinate coordinate)
        {
            if (QueryMelt())
            {
                return new BlockChangeHandler(coordinate, GlassLiquidBlock.InitalizeWithTemperature(Temperature));
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
