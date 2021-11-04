using LocalResouces;
namespace CustomProgram

{
    class StoneSolidBlock : SolidStateBlock, IMeltable
    {
        static readonly private double _meltingTemperature = 1450;

        static public StoneSolidBlock InitalizeWithTemperature(double temperature)
        {
            return new StoneSolidBlock(3000, 1, 0.1, temperature, GeneralResources.GenerateColorVariance(vColor.HSV(0, 0, .15), vColor.HSV(0, 0, .45)), "Stone");
        }
        static public StoneSolidBlock Initalize()
        {
            return StoneSolidBlock.InitalizeWithTemperature(298.15);
        }
        private StoneSolidBlock(double density, double specificHeatCapacity, double thermalConductivity, double temperature, vColor color, string name) : base(density, specificHeatCapacity, thermalConductivity, temperature, color, name) { }

        protected override ActionHandler PreDefaultQuery(GridBlockAPI gridAPI, GridCoordinate coordinate)
        {
            if (QueryMelt())
            {
                return new BlockChangeHandler(coordinate, LavaLiquidBlock.InitalizeWithTemperature(Temperature));
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
    class LavaLiquidBlock : LiquidStateBlock, IBurningInterface, IFreezeable
    {
        static readonly private double _freezingTemperature = 1400;

        public bool Burning { get => true; }

        static public LavaLiquidBlock InitalizeWithTemperature(double temperature)
        {
            return new LavaLiquidBlock(2000, 0.840, 0.01, temperature, GeneralResources.GenerateColorVariance(vColor.HSV(18, .97, .93), vColor.HSV(30, .97, .93)), "Lava");
        }
        private LavaLiquidBlock(double density, double specificHeatCapacity, double thermalConductivity, double temperature, vColor color, string name) : base(density, specificHeatCapacity, thermalConductivity, temperature, color, name) { }

        protected override ActionHandler PreDefaultQuery(GridBlockAPI gridAPI, GridCoordinate coordinate)
        {
            if (QueryFreeze())
            {
                return new BlockChangeHandler(coordinate, StoneSolidBlock.InitalizeWithTemperature(Temperature));
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
}