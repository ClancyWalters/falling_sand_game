using LocalResouces;
namespace CustomProgram
{
    class CoalSolidBlock : SolidStateBlock, IBurningInterface
    {
        readonly private FireComponent _fire;
        private bool _burning;

        static public CoalSolidBlock InitalizeWithTemperature(double temperature)
        {
            return new CoalSolidBlock(1100, 3, 0.2, temperature, GeneralResources.GenerateColorVariance(vColor.HSV(0, 0, 0.06), vColor.HSV(0, 0, 0.1)), "Coal");
        }
        static public CoalSolidBlock Initalize()
        {
            return CoalSolidBlock.InitalizeWithTemperature(298.15);
        }
        private CoalSolidBlock(double density, double specificHeatCapacity, double thermalConductivity, double temperature, vColor color, string name) : base(density, specificHeatCapacity, thermalConductivity, temperature, color, name)
        {
            _fire = new FireComponent(400);
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
                bool randomChance = GeneralResources.GetRandomBool(0.2);
                if (randomChance)
                {
                    _burning = true;
                    _fire.Ignite(this);
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
