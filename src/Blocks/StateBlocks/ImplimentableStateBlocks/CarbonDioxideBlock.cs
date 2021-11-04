using LocalResouces;
using System.Collections.Generic;

namespace CustomProgram
{
    class CarbonDioxideGasBlock : GasStateBlock, IFreezeable
    {

        static readonly private List<RelativeCoordinate> _plantBlockCheck = new List<RelativeCoordinate>() //asks for all the blocks around itself to check oxygen
        {
            RelativeCoordinate.Down, RelativeCoordinate.DownLeft, RelativeCoordinate.DownRight, RelativeCoordinate.Left, RelativeCoordinate.Right, RelativeCoordinate.Up, RelativeCoordinate.UpLeft, RelativeCoordinate.UpRight
        };
        static readonly private double _freezingTemperature = 195.15;
        static public CarbonDioxideGasBlock InitalizeWithTemperature(double temperature)
        {
            return new CarbonDioxideGasBlock(2, .89, 0.01, temperature, vColor.RGB(252, 252, 252), "Carbon Dioxide Gas");
        }
        static public CarbonDioxideGasBlock Initalize()
        {
            return CarbonDioxideGasBlock.InitalizeWithTemperature(298.15);
        }
        private CarbonDioxideGasBlock(double density, double specificHeatCapacity, double thermalConductivity, double temperature, vColor color, string name) : base(density, specificHeatCapacity, thermalConductivity, temperature, color, name) { }

        protected override ActionHandler PreDefaultQuery(GridBlockAPI gridAPI, GridCoordinate coordinate)
        {
            //Dictionary<RelativeCoordinate, Block> dict = gridAPI.GetBlockDictionary(_plantBlockCheck, coordinate, 9);
            foreach (RelativeCoordinate r in _plantBlockCheck)
            {
                if (gridAPI.GetBlock(r, coordinate) is PlantBlock)
                {
                    return new BlockChangeHandler(coordinate, AirGasBlock.InitalizeWithTemperature(Temperature));
                }
            }
            if (QueryFreeze())
            {
                return new BlockChangeHandler(coordinate, CarbonDioxideSolidBlock.InitalizeWithTemperature(Temperature));
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
    class CarbonDioxideSolidBlock : SolidStateBlock, IBoilable
    {
        static readonly private double _boilingTemperature = 195.15;
        static public CarbonDioxideSolidBlock InitalizeWithTemperature(double temperature)
        {
            return new CarbonDioxideSolidBlock(1562, .89, 0.01, temperature, GeneralResources.GenerateColorVariance(vColor.HSV(0, 0, .73), vColor.HSV(0, 0, .6)), "Solid Carbon Dioxide");
        }
        private CarbonDioxideSolidBlock(double density, double specificHeatCapacity, double thermalConductivity, double temperature, vColor color, string name) : base(density, specificHeatCapacity, thermalConductivity, temperature, color, name) { }

        protected override ActionHandler PreDefaultQuery(GridBlockAPI gridAPI, GridCoordinate coordinate)
        {
            if (QueryBoil())
            {
                return new BlockChangeHandler(coordinate, CarbonDioxideGasBlock.InitalizeWithTemperature(Temperature));
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
    }
}
