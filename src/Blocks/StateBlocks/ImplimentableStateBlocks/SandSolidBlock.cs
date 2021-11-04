using LocalResouces;
namespace CustomProgram
{
    class SandSolidBlock : SolidStateBlock, IMeltable
    {
        static readonly private double _meltingTemperature = 1973.15;
        static public SandSolidBlock InitalizeWithTemperature(double temperature)
        {
            return new SandSolidBlock(1470, 0.84, 0.16, temperature, GeneralResources.GenerateColorVariance(vColor.HSV(48, .36, .98), vColor.HSV(48, .55, .90)), "Sand");
        }
        static public SandSolidBlock Initalize()
        {
            return SandSolidBlock.InitalizeWithTemperature(298.15);
        }
        private SandSolidBlock(double density, double specificHeatCapacity, double thermalConductivity, double temperature, vColor color, string name) : base(density, specificHeatCapacity, thermalConductivity, temperature, color, name) { }
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
