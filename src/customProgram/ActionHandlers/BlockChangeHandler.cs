namespace CustomProgram
{
    /// <summary>
    /// Replaces a block at a location
    /// </summary>
    class BlockChangeHandler : ActionHandler
    {
        readonly private GridCoordinate _coordinate;
        readonly private Block _newBlock;
        public BlockChangeHandler(GridCoordinate coordinate, Block newBlock)
        {
            _coordinate = coordinate;
            _newBlock = newBlock;
        }
        internal override void ExecuteAction(GridHandlerAPI gridAPI)
        {
            gridAPI.SetBlock(_coordinate, _newBlock);
        }
    }
}
