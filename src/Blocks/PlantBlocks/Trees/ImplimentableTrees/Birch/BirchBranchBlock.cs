using LocalResouces;

namespace CustomProgram
{
    class BrichBranchBlock : BranchBlock
    {
        static public BrichBranchBlock InitalizeWithTemperatureAndLifetime(double temperature, int growthLifetime, RelativeCoordinate parentLocation)
        {
            return new BrichBranchBlock(temperature, growthLifetime, parentLocation, 0.5, GeneralResources.GenerateColorVariance(vColor.HSV(0, 0, .60), vColor.HSV(0, 0, .70)), "Birch Branch");
        }
        private BrichBranchBlock(double temperature, int growthLifetime, RelativeCoordinate parentLocation, double branchGenerationChance, vColor color, string name) : base(temperature, growthLifetime, parentLocation, branchGenerationChance, color, name) { }


        protected override BranchBlock GetBranch(RelativeCoordinate parentLocation)
        {
            return InitalizeWithTemperatureAndLifetime(Temperature, GrowthLifetime - 1, parentLocation);
        }

        protected override LeafBlock GetLeaf(RelativeCoordinate parentLocation)
        {
            return BirchLeafBlock.Initalize(Temperature, 2, parentLocation.GetMirrorCoordinate());
        }

    }
}
