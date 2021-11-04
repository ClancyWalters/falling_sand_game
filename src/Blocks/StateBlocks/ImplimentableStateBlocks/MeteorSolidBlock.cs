using LocalResouces;
using System.Collections.Generic;

namespace CustomProgram
{
    class MeteorSolidBlock : SolidStateBlock
    {
        static public MeteorSolidBlock InitalizeWithTemperature(double temperature)
        {
            return new MeteorSolidBlock(1100, 1.76, 0.2, temperature, GeneralResources.GenerateColorVariance(vColor.HSV(0, .0, .8), vColor.HSV(0, .0, .15)), "Meteor");
        }
        static public MeteorSolidBlock Initalize()
        {
            return MeteorSolidBlock.InitalizeWithTemperature(298.15);
        }
        private MeteorSolidBlock(double density, double specificHeatCapacity, double thermalConductivity, double temperature, vColor color, string name) : base(density, specificHeatCapacity, thermalConductivity, temperature, color, name) { }

        protected override ActionHandler MovementQuery(GridBlockAPI gridAPI, GridCoordinate coordinate)
        {
            //move down
            Block block = gridAPI.GetBlock(RelativeCoordinate.Down, coordinate);
            if (block is StateBlock)
            {
                if ((block as StateBlock).Density < Density && !(block is SolidStateBlock))
                {
                    List<Block> blockList = new List<Block>()
                    {
                        SmokeGasBlock.InitalizeWithTemperature(500),
                        EmberBlock.InitalizeWithTemperatureAndLifetime(500, 10)
                    };
                    return new TrailingBlockChangeHandler(coordinate, RelativeCoordinate.Down, 0.7, blockList);
                }
            }
            return null;
        }
        protected override ActionHandler PostDefaultQuery(GridBlockAPI gridAPI, GridCoordinate coordinate)
        {
            return new ExplosionHandler(15, 2000, coordinate);
            //explode
        }
        public override Block Clone()
        {
            return InitalizeWithTemperature(Temperature);
        }
    }
}
