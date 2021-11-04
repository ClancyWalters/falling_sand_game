namespace CustomProgram
{
    public interface ICloneable
    {
        /// <summary>
        /// Initalizes a new instance with the same values
        /// </summary>
        /// <returns>Returns a copy of the object</returns>
        Block Clone();
    }
}
