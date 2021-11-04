using LocalResouces;

namespace CustomProgram
{
    class BirchTrunkBlock : TrunkBlock
    {
        private BirchTrunkBlock(double temperature, int growthLifetime, double branchChance, vColor color, string name) : base(temperature, growthLifetime, branchChance, color, name) { }
        static public BirchTrunkBlock InitalizeAll(double temperature, int growthLifetime, double branchChance)
        {
            vColor color;
            if (growthLifetime % 2 == 0)
            {
                color = GeneralResources.GenerateColorVariance(vColor.HSV(0, 0, .25), vColor.HSV(0, 0, .35));
            }
            else
            {
                color = GeneralResources.GenerateColorVariance(vColor.HSV(0, 0, .60), vColor.HSV(0, 0, .70));
            }
            return new BirchTrunkBlock(temperature, growthLifetime, branchChance, color, "Birch Trunk");
        }
        static public BirchTrunkBlock Initalize()
        {
            return InitalizeAll(290, 15, 0.05);
        }
        protected override TrunkBlock GetTrunk()
        {
            return InitalizeAll(Temperature, GrowthLifetime - 1, BranchChance + 0.05);
        }

        protected override BranchBlock GetBranch(RelativeCoordinate branchCoord)
        {
            return BrichBranchBlock.InitalizeWithTemperatureAndLifetime(Temperature, (int)(GrowthLifetime * 0.3), branchCoord.GetMirrorCoordinate());
        }
    }
}
