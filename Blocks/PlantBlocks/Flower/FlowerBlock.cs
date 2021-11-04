using System.Collections.Generic;

namespace CustomProgram
{

    class FlowerBlock : PlantBlock
    {

        static readonly private List<RelativeCoordinate> _flowerOptions = new List<RelativeCoordinate>()
        {
            RelativeCoordinate.Down, RelativeCoordinate.DownLeft, RelativeCoordinate.DownRight, RelativeCoordinate.Left, RelativeCoordinate.Right, RelativeCoordinate.Up, RelativeCoordinate.UpLeft, RelativeCoordinate.UpRight
        };
        readonly private static FlowerColorPalette _flowerColorPalette = new FlowerColorPalette();
        readonly private string _palette;
        readonly private RelativeCoordinate _parentCoordiante;
        static public FlowerBlock InitalizeWithTemperatureAndLifetime(double temperature, int growthLifetime)
        {
            string palette = _flowerColorPalette.GetPallete();
            return FlowerBlock.InitalizeWithTempLifetimeAndPalette(temperature, growthLifetime, RelativeCoordinate.Down, palette);
        }
        static protected FlowerBlock InitalizeWithTempLifetimeAndPalette(double temperature, int growthLifetime, RelativeCoordinate parentCoordinate, string palette)
        {
            vColor color = _flowerColorPalette.GetColor(palette);
            return new FlowerBlock(0.75, 0.01, temperature, growthLifetime, color, parentCoordinate, palette, "Flower Block");
        }
        private FlowerBlock(double specificHeatCapacity, double thermalConductivity, double temperature, int growthLifetime, vColor color, RelativeCoordinate parentCoordinate, string colorPalette, string name)
            : base(specificHeatCapacity, thermalConductivity, temperature, growthLifetime, color, name)
        {
            _parentCoordiante = parentCoordinate;
            _palette = colorPalette;
        }

        protected override ActionHandler GrowthQuery(GridBlockAPI gridAPI, GridCoordinate coordinate)
        {
            if (GrowthLifetime > 0)
            {
                //Dictionary<RelativeCoordinate, Block> dict = gridAPI.GetBlockDictionary(_flowerOptions, coordinate);
                foreach (RelativeCoordinate r in _flowerOptions)
                {
                    if (gridAPI.GetBlock(r, coordinate) is AirGasBlock)
                    {
                        ChangeGrowthLifetime(-1);
                        return new BlockChangeHandler(r.GetGridCoordinate(coordinate), InitalizeWithTempLifetimeAndPalette(Temperature, GrowthLifetime - 1, r.GetMirrorCoordinate(), _palette));
                    }
                }
            }
            else
            {
                FinishedGrowing = true;
            }
            return null;
        }

        protected override ActionHandler PostDefaultQuery(GridBlockAPI gridAPI, GridCoordinate coordinate)
        {
            Block parentBlock = gridAPI.GetBlock(_parentCoordiante, coordinate);
            if (parentBlock is FlowerBlock || parentBlock is FlowerStemBlock)
            {
                return null;
            }
            else
            {
                return new TemperatureConsistantBlockChangeHandler(coordinate, AirGasBlock.Initalize());
            }
        }
    }
}
