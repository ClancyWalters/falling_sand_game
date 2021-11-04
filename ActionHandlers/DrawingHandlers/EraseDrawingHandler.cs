using System.Collections.Generic;

namespace CustomProgram
{
    /// <summary>
    /// Replaces all blocks within the radius with air
    /// </summary>
    class EraseDrawingHandler : DrawingHandler
    {
        static readonly private AirGasBlock _block = AirGasBlock.Initalize();
        public EraseDrawingHandler(AbsoluteCoordinate mousePosition, List<RelativeCoordinate> coordinates) : base(mousePosition, coordinates)
        {

        }

        internal override void ExecuteAction(GridHandlerAPI gridAPI)
        {
            GridCoordinate mousePosOnGrid = MousePosition.GetGridCoordinate(gridAPI.Scale, gridAPI.Origin);
            foreach (RelativeCoordinate c in IncludedPoints)
            {
                if (gridAPI.GetBlockCheck(mousePosOnGrid, c))
                {
                    if (!(gridAPI.GetBlock(mousePosOnGrid, c) is BorderBlock))
                    {
                        gridAPI.SetBlock(mousePosOnGrid, c, _block.Clone());
                    }
                }
            }
        }
    }
}