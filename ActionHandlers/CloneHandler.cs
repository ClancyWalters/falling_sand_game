using System.Collections.Generic;
namespace CustomProgram
{
    class CloneHandler : ActionHandler
    {
        readonly private GridCoordinate _coordinate;
        readonly private ICloneable _newBlock;
        readonly private List<RelativeCoordinate> _replacementList;
        public CloneHandler(GridCoordinate coordinate, ICloneable newBlock, List<RelativeCoordinate> replacementList)
        {
            _coordinate = coordinate;
            _newBlock = newBlock;
            _replacementList = replacementList;
        }

        internal override void ExecuteAction(GridHandlerAPI gridAPI)
        {
            foreach (RelativeCoordinate r in _replacementList)
            {
                gridAPI.SetBlock(r.GetGridCoordinate(_coordinate), _newBlock.Clone());
            }
        }
    }
}
