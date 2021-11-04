using LocalResouces;
namespace CustomProgram
{
    class AshSolidBlock : SolidStateBlock //low density solid
    {
        static public AshSolidBlock InitalizeWithTemperature(double temperature)
        {
            return new AshSolidBlock(10, 3, 0.2, temperature, GeneralResources.GenerateColorVariance(vColor.HSV(0, 0, .17), vColor.HSV(0, 0, .20)), "Ash");
        }

        public override Block Clone()
        {
            return InitalizeWithTemperature(Temperature);
        }

        private AshSolidBlock(double density, double specificHeatCapacity, double thermalConductivity, double temperature, vColor color, string name) : base(density, specificHeatCapacity, thermalConductivity, temperature, color, name) { }
    }
}
