using LocalResouces;
using System;
using System.Collections.Generic;

namespace CustomProgram
{
    class TrailingBlockChangeHandler : ActionHandler
    {
        readonly private GridCoordinate _location;
        readonly private RelativeCoordinate _replacementLocation;
        readonly private double _trailProbability;
        readonly private List<Block> _trailBlocks;
        static readonly private Random _rngGenerator = new Random();
        public TrailingBlockChangeHandler(GridCoordinate location, RelativeCoordinate replacementLocation, double trailProbability, List<Block> trailBlocks)
        {
            _location = location;
            _replacementLocation = replacementLocation;
            _trailProbability = trailProbability;
            _trailBlocks = trailBlocks;
        }

        internal override void ExecuteAction(GridHandlerAPI gridAPI)
        {
            //get the blocks that can move
            Block originBlock = gridAPI.GetBlock(_location);
            Block replacementBlock = gridAPI.GetBlock(_location, _replacementLocation);
            //switch
            gridAPI.SetBlock(_location, _replacementLocation, originBlock);

            if (GeneralResources.GetRandomBool(_trailProbability))
            {
                replacementBlock = _trailBlocks[_rngGenerator.Next(0, _trailBlocks.Count)];
            }
            gridAPI.SetBlock(_location, replacementBlock);
            (originBlock as StateBlock).HasUpdated = true;
            (replacementBlock as StateBlock).HasUpdated = true;
        }
    }
}
