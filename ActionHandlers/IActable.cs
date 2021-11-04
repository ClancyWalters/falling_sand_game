namespace CustomProgram
{
    interface IActable
    {
        /// <summary>
        /// Executes an action
        /// </summary>
        ActionHandler ActionQuery(GridBlockAPI gridAPI, GridCoordinate coordinate);
        /// <summary>
        /// Returns if a block has updated this turn
        /// </summary>
        bool HasUpdated { get; set; }
    }
}
