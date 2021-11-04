using LocalResouces;

namespace CustomProgram
{
    class MapleBranchBlock : BranchBlock
    {
        static public MapleBranchBlock InitalizeWithTemperatureAndLifetime(double temperature, int growthLifetime, RelativeCoordinate parentLocation)
        {
            return new MapleBranchBlock(temperature, growthLifetime, parentLocation, 0.5, GeneralResources.GenerateColorVariance(vColor.HSV(33, .86, .25), vColor.HSV(33, .98, .42)), "Maple Branch");
        }
        private MapleBranchBlock(double temperature, int growthLifetime, RelativeCoordinate parentLocation, double branchGenerationChance, vColor color, string name) : base(temperature, growthLifetime, parentLocation, branchGenerationChance, color, name) { }


        protected override BranchBlock GetBranch(RelativeCoordinate parentLocation)
        {
            return InitalizeWithTemperatureAndLifetime(Temperature, GrowthLifetime - 1, parentLocation);
        }

        protected override LeafBlock GetLeaf(RelativeCoordinate parentLocation)
        {
            return MapleLeafBlock.Initalize(Temperature, 5, parentLocation.GetMirrorCoordinate());
        }

    }
}
