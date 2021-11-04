using LocalResouces;

namespace CustomProgram
{
    class OakTrunkBlock : TrunkBlock
    {
        private OakTrunkBlock(double temperature, int growthLifetime, double branchChance, vColor color, string name) : base(temperature, growthLifetime, branchChance, color, name) { }
        static public OakTrunkBlock InitalizeAll(double temperature, int growthLifetime, double branchChance)
        {
            return new OakTrunkBlock(temperature, growthLifetime, branchChance, GeneralResources.GenerateColorVariance(vColor.HSV(33, .94, .19), vColor.HSV(33, .94, .25)), "Oak Trunk");
        }
        static public OakTrunkBlock Initalize()
        {
            return InitalizeAll(290, 10, 0.2);
        }
        protected override TrunkBlock GetTrunk()
        {
            return InitalizeAll(Temperature, GrowthLifetime - 1, BranchChance + 0.1);
        }
        protected override BranchBlock GetBranch(RelativeCoordinate branchCoord)
        {
            return OakBranchBlock.InitalizeWithTemperatureAndLifetime(Temperature, (int)(GrowthLifetime * 0.5), branchCoord.GetMirrorCoordinate());
        }
    }
}
