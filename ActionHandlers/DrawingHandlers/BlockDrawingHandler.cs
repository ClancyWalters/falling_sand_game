using System.Collections.Generic;

namespace CustomProgram
{
    /// <summary>
    /// Replaces all gas blocks within a radius with clones of a block
    /// </summary>
    class BlockDrawingHandler : DrawingHandler
    {
        readonly private ICloneable _placementBlock;
        public BlockDrawingHandler(AbsoluteCoordinate mousePosition, List<RelativeCoordinate> coordinates, ICloneable block) : base(mousePosition, coordinates)
        {
            _placementBlock = block;
        }

        internal override void ExecuteAction(GridHandlerAPI gridAPI)
        {
            GridCoordinate mousePosOnGrid = MousePosition.GetGridCoordinate(gridAPI.Scale, gridAPI.Origin);
            foreach (RelativeCoordinate c in IncludedPoints)
            {
                if (gridAPI.GetBlockCheck(mousePosOnGrid, c))
                {
                    if (gridAPI.GetBlock(mousePosOnGrid, c) is GasStateBlock)
                    {
                        gridAPI.SetBlock(mousePosOnGrid, c, _placementBlock.Clone());
                    }
                }
            }
        }
    }
}