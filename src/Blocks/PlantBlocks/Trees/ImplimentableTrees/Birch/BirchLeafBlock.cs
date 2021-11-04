using LocalResouces;
namespace CustomProgram
{
    class BirchLeafBlock : LeafBlock
    {
        static public LeafBlock Initalize(double temperature, int growthLifetime, RelativeCoordinate parentLocation)
        {
            return new BirchLeafBlock(temperature, growthLifetime, parentLocation, GeneralResources.GenerateColorVariance(vColor.HSV(112, .47, .63), vColor.HSV(112, .35, .72)), "Birch Leaf");
        }
        private BirchLeafBlock(double temperature, int growthLifetime, RelativeCoordinate parentLocation, vColor color, string name) : base(temperature, growthLifetime, parentLocation, color, name) { }
        protected override LeafBlock GetLeaf(RelativeCoordinate parentLocation)
        {
            return Initalize(Temperature, GrowthLifetime - 1, parentLocation.GetMirrorCoordinate());
        }
    }
}
