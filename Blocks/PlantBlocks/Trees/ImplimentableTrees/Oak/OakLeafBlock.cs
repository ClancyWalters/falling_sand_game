using LocalResouces;
namespace CustomProgram
{
    class OakLeafBlock : LeafBlock
    {
        static public OakLeafBlock Initalize(double temperature, int growthLifetime, RelativeCoordinate parentLocation)
        {
            vColor color;
            if (GeneralResources.GetRandomBool(0.01))
            {
                color = vColor.RGB(200, 0, 0);
            }
            else
            {
                color = GeneralResources.GenerateColorVariance(vColor.HSV(114, .88, .41), vColor.HSV(114, .88, .54));
            }
            return new OakLeafBlock(temperature, growthLifetime, parentLocation, color, "Oak Leaf");
        }
        private OakLeafBlock(double temperature, int growthLifetime, RelativeCoordinate parentLocation, vColor color, string name) : base(temperature, growthLifetime, parentLocation, color, name) { }
        protected override LeafBlock GetLeaf(RelativeCoordinate parentLocation)
        {
            return Initalize(Temperature, GrowthLifetime - 1, parentLocation.GetMirrorCoordinate());
        }
    }
}
