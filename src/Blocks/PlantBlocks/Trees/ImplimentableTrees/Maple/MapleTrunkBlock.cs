using LocalResouces;

namespace CustomProgram
{
    class MapleTrunkBlock : TrunkBlock
    {
        private MapleTrunkBlock(double temperature, int growthLifetime, double branchChance, vColor color, string name) : base(temperature, growthLifetime, branchChance, color, name) { }
        static public MapleTrunkBlock InitalizeAll(double temperature, int growthLifetime, double branchChance)
        {
            return new MapleTrunkBlock(temperature, growthLifetime, branchChance, GeneralResources.GenerateColorVariance(vColor.HSV(33, .86, .25), vColor.HSV(33, .98, .42)), "Maple Trunk");
        }
        static public MapleTrunkBlock Initalize()
        {
            return InitalizeAll(290, 8, 0.05);
        }
        protected override TrunkBlock GetTrunk()
        {
            return InitalizeAll(Temperature, GrowthLifetime - 1, BranchChance + 0.05);
        }

        protected override BranchBlock GetBranch(RelativeCoordinate branchCoord)
        {
            return MapleBranchBlock.InitalizeWithTemperatureAndLifetime(Temperature, (int)(GrowthLifetime * 0.6), branchCoord.GetMirrorCoordinate());
        }
    }
}
