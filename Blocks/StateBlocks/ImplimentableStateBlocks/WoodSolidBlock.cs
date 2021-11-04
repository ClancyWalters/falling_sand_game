using LocalResouces;
namespace CustomProgram
{
    class WoodSolidBlock : SolidStateBlock, IBurningInterface
    {
        private readonly FireComponent _fire;
        private bool _burning;

        static public WoodSolidBlock InitalizeWithTemperature(double temperature)
        {
            return new WoodSolidBlock(1100, 1.76, 0.2, temperature, GeneralResources.GenerateColorVariance(vColor.HSV(33, .94, .19), vColor.HSV(33, .94, .25)), "Wood Block");
        }
        static public WoodSolidBlock Initalize()
        {
            return WoodSolidBlock.InitalizeWithTemperature(298.15);
        }
        private WoodSolidBlock(double density, double specificHeatCapacity, double thermalConductivity, double temperature, vColor color, string name) : base(density, specificHeatCapacity, thermalConductivity, temperature, color, name)
        {
            _fire = new FireComponent(120);
            _burning = false;
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
