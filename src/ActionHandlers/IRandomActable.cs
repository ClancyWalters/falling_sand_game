namespace CustomProgram
{
    interface IRandomActable
    {
        /// <summary>
        /// Executes an action
        /// </summary>
        ActionHandler RandomActionQuery(GridBlockAPI gridAPI, GridCoordinate coordinate);
    }
}
