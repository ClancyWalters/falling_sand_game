namespace CustomProgram
{
    class GravityHandler : ActionHandler
    {
        readonly private GridCoordinate _location;
        readonly private GridCoordinate _replacementLocation;
        readonly private GridCoordinate _moveThroughBlock;
        public GravityHandler(GridCoordinate location, RelativeCoordinate replacementLocation)
        {
            _location = location;
            _replacementLocation = replacementLocation.GetGridCoordinate(location);
            _moveThroughBlock = new GridCoordinate(_replacementLocation.X, _location.Y);
        }

        internal override void ExecuteAction(GridHandlerAPI gridAPI)
        {
            //gets the three blocks that can move
            Block gravityBlock = gridAPI.GetBlock(_location);
            Block sideBlock = gridAPI.GetBlock(_moveThroughBlock);
            Block downSideBlock = gridAPI.GetBlock(_replacementLocation);
            //moves the blocks
            gridAPI.SetBlock(_replacementLocation, gravityBlock);
            gridAPI.SetBlock(_moveThroughBlock, downSideBlock);
            gridAPI.SetBlock(_location, sideBlock);
            //all gravity effected blocks are stateblocks and can be treated as such
            (gravityBlock as StateBlock).HasUpdated = true;
            (downSideBlock as StateBlock).HasUpdated = true;
            (sideBlock as StateBlock).HasUpdated = true;
        }
    }
}
