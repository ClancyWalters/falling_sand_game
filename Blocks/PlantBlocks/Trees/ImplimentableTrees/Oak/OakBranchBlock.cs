using LocalResouces;

namespace CustomProgram
{
    class OakBranchBlock : BranchBlock
    {
        static public OakBranchBlock InitalizeWithTemperatureAndLifetime(double temperature, int growthLifetime, RelativeCoordinate parentLocation)
        {
            return new OakBranchBlock(temperature, growthLifetime, parentLocation, 0.5, GeneralResources.GenerateColorVariance(vColor.HSV(33, .94, .35), vColor.HSV(33, .94, .45)), "Oak Branch");
        }
        private OakBranchBlock(double temperature, int growthLifetime, RelativeCoordinate parentLocation, double branchGenerationChance, vColor color, string name) : base(temperature, growthLifetime, parentLocation, branchGenerationChance, color, name) { }


        protected override BranchBlock GetBranch(RelativeCoordinate parentLocation)
        {
            return InitalizeWithTemperatureAndLifetime(Temperature, GrowthLifetime - 1, parentLocation);
        }
        protected override LeafBlock GetLeaf(RelativeCoordinate parentLocation)
        {
            return OakLeafBlock.Initalize(Temperature, 4, parentLocation.GetMirrorCoordinate());
        }

    }
}
