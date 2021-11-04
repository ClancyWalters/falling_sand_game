using LocalResouces;
using System.Collections.Generic;
namespace CustomProgram
{
    class GunpowderSolidBlock : SolidStateBlock
    {
        static public GunpowderSolidBlock InitalizeWithTemperature(double temperature)
        {
            return new GunpowderSolidBlock(1000, 1, 0.05, temperature, GeneralResources.GenerateColorVariance(vColor.HSV(0, 0, .10), vColor.HSV(0, 0, .15)), "Gunpowder");
        }
        static public GunpowderSolidBlock Initalize()
        {
            return InitalizeWithTemperature(297.15);
        }
        private GunpowderSolidBlock(double density, double specificHeatCapacity, double thermalConductivity, double temperature, vColor color, string name) : base(density, specificHeatCapacity, thermalConductivity, temperature, color, name) { }
        public override Block Clone()
        {
            return InitalizeWithTemperature(Temperature);
        }
        static readonly private List<RelativeCoordinate> _fireChecks = new List<RelativeCoordinate>() //asks for all the blocks around itself to check oxygen
        {
            RelativeCoordinate.Down, RelativeCoordinate.DownLeft, RelativeCoordinate.DownRight, RelativeCoordinate.Left, RelativeCoordinate.Right, RelativeCoordinate.Up, RelativeCoordinate.UpLeft, RelativeCoordinate.UpRight
        };
        protected override ActionHandler PreDefaultQuery(GridBlockAPI gridAPI, GridCoordinate coordinate)
        {
            //Dictionary<RelativeCoordinate, Block> dict = gridAPI.GetBlockDictionary(_fireChecks, coordinate);
            foreach (RelativeCoordinate r in _fireChecks)
            {
                if (gridAPI.GetBlock(r, coordinate) is IBurningInterface)
                {
                    if ((gridAPI.GetBlock(r, coordinate) as IBurningInterface).Burning)
                    {
                        return new ExplosionHandler(7, 1500, coordinate);
                    }
                }
            }
            return null;
        }


    }
}
