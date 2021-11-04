namespace CustomProgram
{
    /// <summary>
    /// Performs actions that effect the grid
    /// </summary>
    public abstract class ActionHandler
    {
        /// <summary>
        /// Executes actions effecting grid objects
        /// </summary>
        internal abstract void ExecuteAction(GridHandlerAPI gridHandlerAPI);
    }
}
