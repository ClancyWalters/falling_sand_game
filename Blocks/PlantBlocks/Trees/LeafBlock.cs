using System.Collections.Generic;

namespace CustomProgram
{
    abstract class LeafBlock : PlantBlock
    {

        static readonly private List<RelativeCoordinate> _leafOptions = new List<RelativeCoordinate>()
        {
            RelativeCoordinate.Down, RelativeCoordinate.DownLeft, RelativeCoordinate.DownRight, RelativeCoordinate.Left, RelativeCoordinate.Right, RelativeCoordinate.Up, RelativeCoordinate.UpLeft, RelativeCoordinate.UpRight
        };
        readonly private RelativeCoordinate _parentLocation;
        public LeafBlock(double temperature, int growthLifetime, RelativeCoordinate parentLocation, vColor color, string name) : base(1.76, 0.2, temperature, growthLifetime, color, name)
        {
            _parentLocation = parentLocation;
        }
        protected override ActionHandler GrowthQuery(GridBlockAPI gridAPI, GridCoordinate coordinate)
        {
            Block parentBlock = gridAPI.GetBlock(_parentLocation, coordinate);
            if (parentBlock is BranchBlock || parentBlock is TrunkBlock || parentBlock is LeafBlock)
            {
                if (GrowthLifetime > 0)
                {
                    //Dictionary<RelativeCoordinate, Block> dict = gridAPI.GetBlockDictionary(_leafOptions, coordinate);
                    foreach (RelativeCoordinate r in _leafOptions)
                    {
                        if (gridAPI.GetBlock(r, coordinate) is AirGasBlock)
                        {
                            ChangeGrowthLifetime(-1);
                            return new BlockChangeHandler(r.GetGridCoordinate(coordinate), GetLeaf(r));
                        }
                    }
                }
            }
            else
            {
                return new TemperatureConsistantBlockChangeHandler(coordinate, AirGasBlock.Initalize());
            }
            return null;
        }
        protected abstract LeafBlock GetLeaf(RelativeCoordinate parentLocation);
    }
}
