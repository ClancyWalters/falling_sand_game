using LocalResouces;
using System;

namespace CustomProgram
{
    class OilLiquidBlock : LiquidStateBlock, IBurningInterface
    {
        readonly private FireComponent _fire;
        private bool _burning;
        readonly private static Random _random = new Random();
        static public OilLiquidBlock InitalizeWithTemperature(double temperature)
        {
            return new OilLiquidBlock(200, 0.5, 0.1, temperature, GeneralResources.GenerateColorVariance(vColor.HSV(_random.Next(0, 361), .5, .05), vColor.HSV(_random.Next(0, 361), .5, .07)), "Oil");
        }
        static public OilLiquidBlock Initalize()
        {
            return OilLiquidBlock.InitalizeWithTemperature(298.15);
        }
        private OilLiquidBlock(double density, double specificHeatCapacity, double thermalConductivity, double temperature, vColor color, string name) : base(density, specificHeatCapacity, thermalConductivity, temperature, color, name)
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
        public bool Burning { get => _burning; }
    }
}
