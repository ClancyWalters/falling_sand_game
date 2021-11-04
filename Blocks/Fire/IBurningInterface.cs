namespace CustomProgram
{
    interface IBurningInterface : ITemperature //it doesnt make sense for a block that is burning to not have a temperature
    {
        bool Burning { get; }
    }
}
