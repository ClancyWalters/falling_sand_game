using LocalResouces;
using System.Collections.Generic;

namespace CustomProgram
{
    abstract class BranchBlock : PlantBlock
    {

        static readonly private List<RelativeCoordinate> _leafOptions = new List<RelativeCoordinate>()
        {
            RelativeCoordinate.Down, RelativeCoordinate.DownLeft, RelativeCoordinate.DownRight, RelativeCoordinate.Left, RelativeCoordinate.Right, RelativeCoordinate.Up, RelativeCoordinate.UpLeft, RelativeCoordinate.UpRight
        };
        readonly private RelativeCoordinate _parentLocation;
        readonly private double _branchGenerationChance;
        protected BranchBlock(double temperature, int growthLifetime, RelativeCoordinate parentLocation, double branchGenerationChance, vColor color, string name) : base(1.76, 0.2, temperature, growthLifetime, color, name)
        {
            _branchGenerationChance = GeneralResources.KeepWithinRange(branchGenerationChance, 0, 1);
            _parentLocation = parentLocation;
        }
        protected override ActionHandler GrowthQuery(GridBlockAPI gridAPI, GridCoordinate coordinate)
        {

            if (GrowthLifetime > 0)
            {
                if (GeneralResources.GetRandomBool(_branchGenerationChance))
                {
                    RelativeCoordinate mirrorCoord = _parentLocation.GetMirrorCoordinate();
                    Block potentialGrowthBlock = gridAPI.GetBlock(mirrorCoord, coordinate);
                    if (potentialGrowthBlock is AirGasBlock)
                    {

                        return new BlockChangeHandler(mirrorCoord.GetGridCoordinate(coordinate), GetBranch(_parentLocation));
                    }
                }
                else
                {
                    //Dictionary<RelativeCoordinate, Block> dict = gridAPI.GetBlockDictionary(_leafOptions, coordinate);
                    foreach (RelativeCoordinate r in _leafOptions)
                    {
                        if (gridAPI.GetBlock(r, coordinate) is GasStateBlock)
                        {
                            return new BlockChangeHandler(r.GetGridCoordinate(coordinate), GetLeaf(r));
                        }
                    }
                }
            }
            return null;
        }

        protected override ActionHandler PostDefaultQuery(GridBlockAPI gridAPI, GridCoordinate coordinate)
        {
            Block parentBlock = gridAPI.GetBlock(_parentLocation, coordinate);
            if (!(parentBlock is OakBranchBlock || parentBlock is OakTrunkBlock))
            {
                return new BlockChangeHandler(coordinate, WoodSolidBlock.InitalizeWithTemperature(Temperature));
            }
            return null;
        }

        protected abstract BranchBlock GetBranch(RelativeCoordinate parentLocation);
        protected abstract LeafBlock GetLeaf(RelativeCoordinate parentLocation);
    }
}
