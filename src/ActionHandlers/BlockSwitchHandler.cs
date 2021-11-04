namespace CustomProgram
{
    /// <summary>
    /// Replaces switches two blocks locations
    /// </summary>
    class BlockSwitchHandler : ActionHandler
    {
        readonly private GridCoordinate _location;
        readonly private RelativeCoordinate _replacementLocation;
        public BlockSwitchHandler(GridCoordinate location, RelativeCoordinate replacementLocation)
        {
            _location = location;
            _replacementLocation = replacementLocation;
        }
        internal override void ExecuteAction(GridHandlerAPI gridAPI)
        {
            //get the blocks that can move
            Block originBlock = gridAPI.GetBlock(_location);
            Block replacementBlock = gridAPI.GetBlock(_location, _replacementLocation);
            //switch
            gridAPI.SetBlock(_location, replacementBlock);
            gridAPI.SetBlock(_location, _replacementLocation, originBlock);
            (originBlock as StateBlock).HasUpdated = true;
            (replacementBlock as StateBlock).HasUpdated = true;
        }
    }
}
