using System.Collections.Generic;

namespace CustomProgram
{
    class GridBlockAPI //Defines how a block can request from the grid;
    {
        private readonly Grid _grid;
        /// <summary>
        /// Creates the GridBlockAPI
        /// </summary>
        public GridBlockAPI(Grid grid)
        {
            _grid = grid;
        }
        /// <summary>
        /// Gets a block given a GridCoordinate translated by a RelativeCoordinate
        /// </summary>
        /// <returns>Returns a Block</returns>
        public Block GetBlock(RelativeCoordinate coordintate, GridCoordinate blockLocation)
        {
            return _grid.GetBlock(coordintate.GetGridCoordinate(blockLocation));
        }
        /// <summary>
        /// Creates a dictionary with the key being a relative coordinate and the value being a block. Avoid using due to performance issues.
        /// </summary>
        public Dictionary<RelativeCoordinate, Block> GetBlockDictionary(List<RelativeCoordinate> coordinateList, GridCoordinate blockLocation)
        {
            Dictionary<RelativeCoordinate, Block> dict = new Dictionary<RelativeCoordinate, Block>();
            foreach (RelativeCoordinate c in coordinateList)
            {
                GridCoordinate cGrid = c.GetGridCoordinate(blockLocation);
                if (_grid.GetBlockCheck(cGrid))
                {
                    dict.Add(c, _grid.GetBlock(cGrid));
                }
            }
            return dict;
        }
        /// <summary>
        /// Creates a dictionary with the key being a relative coordinate and the value being a block. 
        /// Avoid using due to performance issues. 
        /// Mitigation of performance issues via defined dictionary size is ineffective.
        /// </summary>
        public Dictionary<RelativeCoordinate, Block> GetBlockDictionary(List<RelativeCoordinate> coordinateList, GridCoordinate blockLocation, int dictionarySize)
        {
            Dictionary<RelativeCoordinate, Block> dict = new Dictionary<RelativeCoordinate, Block>(dictionarySize);
            foreach (RelativeCoordinate c in coordinateList)
            {
                GridCoordinate cGrid = c.GetGridCoordinate(blockLocation);
                if (_grid.GetBlockCheck(cGrid))
                {
                    dict.Add(c, _grid.GetBlock(cGrid));
                }
            }
            return dict;
        }
        /// <summary>
        /// Adds an ActionHandler to the grids action list
        /// </summary>
        public void AddAction(ActionHandler action) //actions in this list are handled at the end of the tick. 
        {
            _grid.AddAction(action);
        }
    }
}
