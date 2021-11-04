using LocalResouces;
using System;
using System.Collections.Generic;

namespace CustomProgram
{
    class FireworkSolidBlock : SolidStateBlock
    {
        private int _lifetime;
        private int _lifetimeProtection = 10;
        readonly private static Random _rngGenerator = new Random();
        static public FireworkSolidBlock InitalizeWithTemperatureAndLifetime(double temperature, int lifetime)
        {
            return new FireworkSolidBlock(1100, 1.76, 0.2, temperature, lifetime, GeneralResources.GenerateColorVariance(vColor.HSV(0, .8, 1), vColor.HSV(0, .6, 1)), "Fireworks");
        }
        static public FireworkSolidBlock InitalizeWithTemperature(double temperature)
        {
            return InitalizeWithTemperatureAndLifetime(temperature, _rngGenerator.Next(50, 80));
        }
        static public FireworkSolidBlock Initalize()
        {
            return FireworkSolidBlock.InitalizeWithTemperature(298.15);
        }
        private FireworkSolidBlock(double density, double specificHeatCapacity, double thermalConductivity, double temperature, int lifetime, vColor color, string name) : base(density, specificHeatCapacity, thermalConductivity, temperature, color, name)
        {
            _lifetime = lifetime;
        }

        protected override ActionHandler MovementQuery(GridBlockAPI gridAPI, GridCoordinate coordinate)
        {
            _lifetime--;
            _lifetimeProtection--;
            if (_lifetime > 0)
            {
                Block block = gridAPI.GetBlock(RelativeCoordinate.Up, coordinate);
                if (block is StateBlock)
                {
                    if (!(block is SolidStateBlock))
                    {
                        List<Block> blockList = new List<Block>()
                    {
                        SmokeGasBlock.InitalizeWithTemperature(500)
                    };
                        return new TrailingBlockChangeHandler(coordinate, RelativeCoordinate.Up, 0.7, blockList);
                    }
                }
            }
            return null;
        }
        protected override ActionHandler PostDefaultQuery(GridBlockAPI gridAPI, GridCoordinate coordinate)
        {
            if (_lifetimeProtection <= 0)
            {
                return new ExplosionHandler(15, 1500, coordinate);
            }
            return null;
        }
        public override Block Clone()
        {
            return InitalizeWithTemperature(Temperature);
        }
    }
}
