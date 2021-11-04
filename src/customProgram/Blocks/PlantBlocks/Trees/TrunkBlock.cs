using LocalResouces;

namespace CustomProgram
{
    abstract class TrunkBlock : PlantBlock
    {

        readonly private double _branchChance;

        protected double BranchChance { get => _branchChance; }

        protected TrunkBlock(double temperature, int growthLifetime, double branchChance, vColor color, string name) : base(1.76, 0.2, temperature, growthLifetime, color, name)
        {
            _branchChance = GeneralResources.KeepWithinRange(branchChance, 0, 1);
        }

        protected override ActionHandler PostDefaultQuery(GridBlockAPI gridAPI, GridCoordinate coordinate)
        {
            Block down = gridAPI.GetBlock(RelativeCoordinate.Down, coordinate);
            if (!(down is GrassSolidBlock || down is DirtSolidBlock || down is MudSolidBlock || down is TrunkBlock))
            {
                return new BlockChangeHandler(coordinate, WoodSolidBlock.InitalizeWithTemperature(Temperature));
            }
            return null;
        }
        protected override ActionHandler GrowthQuery(GridBlockAPI gridAPI, GridCoordinate coordinate)
        {
            if (GrowthLifetime > 0)
            {
                Block up = gridAPI.GetBlock(RelativeCoordinate.Up, coordinate);
                if (GeneralResources.GetRandomBool(1 - _branchChance)) //chooses to create branch or trunk
                {
                    if (up is AirGasBlock || up is LeafBlock)
                    {
                        FinishedGrowing = true;

                        Block returnBlock = GetTrunk();
                        (returnBlock as IActable).HasUpdated = true;
                        return new BlockChangeHandler(RelativeCoordinate.Up.GetGridCoordinate(coordinate), returnBlock);
                    }
                }
                else
                {
                    if (GeneralResources.GetRandomBool(0.5))
                    {
                        if (gridAPI.GetBlock(RelativeCoordinate.UpRight, coordinate) is GasStateBlock)
                        {
                            return new BlockChangeHandler(RelativeCoordinate.UpRight.GetGridCoordinate(coordinate), GetBranch(RelativeCoordinate.UpRight));
                        }
                    }
                    else
                    {
                        if (gridAPI.GetBlock(RelativeCoordinate.UpLeft, coordinate) is GasStateBlock)
                        {
                            return new BlockChangeHandler(RelativeCoordinate.UpLeft.GetGridCoordinate(coordinate), GetBranch(RelativeCoordinate.UpLeft));
                        }
                    }
                }
            }
            else
            {
                FinishedGrowing = true;
            }
            return null;
        }
        protected abstract TrunkBlock GetTrunk();
        protected abstract BranchBlock GetBranch(RelativeCoordinate coordinate);
    }
}
