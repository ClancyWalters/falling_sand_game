using LocalResouces;
namespace CustomProgram
{
    class MapleLeafBlock : LeafBlock
    {
        static public MapleLeafBlock Initalize(double temperature, int growthLifetime, RelativeCoordinate parentLocation)
        {
            return new MapleLeafBlock(temperature, growthLifetime, parentLocation, GeneralResources.GenerateColorVariance(vColor.HSV(0, .63, .93), vColor.HSV(41, .63, .93)), "Maple Leaf");
        }
        private MapleLeafBlock(double temperature, int growthLifetime, RelativeCoordinate parentLocation, vColor color, string name) : base(temperature, growthLifetime, parentLocation, color, name) { }
        protected override LeafBlock GetLeaf(RelativeCoordinate parentLocation)
        {
            return Initalize(Temperature, GrowthLifetime - 1, parentLocation.GetMirrorCoordinate());
        }
    }
}
