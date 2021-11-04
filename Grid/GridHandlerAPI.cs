using System.Collections.Generic;
using System.Diagnostics;

namespace CustomProgram
{
    class GridHandlerAPI
    {
        readonly private Grid _grid;
        /// <summary>
        /// Creates a GridHandlerAPI
        /// </summary>
        internal GridHandlerAPI(Grid grid)
        {
            _grid = grid;
        }
        /// <summary>
        /// Gets a block that is at a GridCoordinate
        /// </summary>
        /// <returns>Retuns a block</returns>
        internal Block GetBlock(GridCoordinate location)
        {
            return _grid.GetBlock(location);
        }
        /// <summary>
        /// Gets a block that is at a GridCoordinate and translated by a RelativeCoordinate
        /// </summary>
        /// <returns>Retuns a block</returns>
        internal Block GetBlock(GridCoordinate origin, RelativeCoordinate translation)
        {
            return _grid.GetBlock(translation.GetGridCoordinate(origin));
        }
        /// <summary>
        /// Gets a block that is at a GridCoordinate and translated by a RelativeCoordinate
        /// </summary>
        /// <returns>Retuns a block</returns>
        internal Block GetBlock(RelativeCoordinate translation, GridCoordinate origin) //Just a compatability overload
        {
            return GetBlock(origin, translation);
        }
        /// <summary>
        /// Sets the given Block at the given GridCoordinate
        /// </summary>
        internal void SetBlock(GridCoordinate location, Block setToBlock)
        {
            _grid.ReplaceBlock(location, setToBlock);
        }
        /// <summary>
        /// Sets the given Block at the given GridCoordinate translated by a given RelativeCoordinate
        /// </summary>
        internal void SetBlock(GridCoordinate origin, RelativeCoordinate translation, Block setToBlock)
        {
            _grid.ReplaceBlock(translation.GetGridCoordinate(origin), setToBlock);
        }
        /// <summary>
        /// Checks if a given GridCoordinate is on the grid
        /// </summary>
        internal bool GetBlockCheck(GridCoordinate location)
        {
            return _grid.GetBlockCheck(location);
        }
        /// <summary>
        /// Checks if a given GridCoordinate is on the grid translated by a RelativeCoordinate
        /// </summary>
        internal bool GetBlockCheck(GridCoordinate origin, RelativeCoordinate translation)
        {
            return _grid.GetBlockCheck(translation.GetGridCoordinate(origin));
        }
        /// <summary>
        /// Creates a dictionary with the key being a relative coordinate and the value being a block. Avoid using due to performance issues.
        /// </summary>
        internal Dictionary<RelativeCoordinate, Block> GetBlockDictionary(List<RelativeCoordinate> coordinateList, GridCoordinate blockLocation)
        {
            Dictionary<RelativeCoordinate, Block> dict = new Dictionary<RelativeCoordinate, Block>();
            foreach (RelativeCoordinate c in coordinateList)
            {
                GridCoordinate cGrid = c.GetGridCoordinate(blockLocation);
                if (_grid.GetBlockCheck(cGrid))
                {
                    dict.Add(c, _grid.GetBlock(cGrid));
                }
                else
                {
                    Debug.WriteLine("Grid Coordinate:" + cGrid.X + " " + cGrid.Y);
                }
            }
            return dict;
        }
        /// <summary>
        /// Limits a list of RelativeCoordinates ensuring all are within the grid
        /// </summary>
        /// <returns>The limited list of RelativeCoordinates</returns>
        internal List<RelativeCoordinate> LimitCoordinates(List<RelativeCoordinate> inputList, GridCoordinate centerpoint)
        {
            List<RelativeCoordinate> returnList = new List<RelativeCoordinate>();
            foreach (RelativeCoordinate r in inputList)
            {
                GridCoordinate gridCoordinate = r.GetGridCoordinate(centerpoint);
                if (_grid.GetBlockCheck(gridCoordinate))
                {
                    returnList.Add(r);
                }
            }
            return returnList;
        }
        /// <summary>
        /// Returns the scale of the grid
        /// </summary>
        internal int Scale { get => _grid.Scale; }
        /// <summary>
        /// Returns the location of the top left pixel of the grid
        /// </summary>
        internal AbsoluteCoordinate Origin { get => _grid.Origin; }
    }
}
