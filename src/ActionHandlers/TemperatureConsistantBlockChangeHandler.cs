using System.Collections.Generic;
namespace CustomProgram
{
    class TemperatureConsistantBlockChangeHandler : ActionHandler
    {
        readonly private GridCoordinate _coordinate;
        readonly private StateBlock _newBlock;

        static readonly private List<RelativeCoordinate> _temperatureCheckedBlocks = new List<RelativeCoordinate>()
        {
            RelativeCoordinate.Down, RelativeCoordinate.Left, RelativeCoordinate.Right, RelativeCoordinate.Up
        };
        public TemperatureConsistantBlockChangeHandler(GridCoordinate coordinate, StateBlock newBlock)
        {
            _coordinate = coordinate;
            _newBlock = newBlock;
        }

        internal override void ExecuteAction(GridHandlerAPI gridAPI)
        {
            //Dictionary<RelativeCoordinate, Block> dict = gridAPI.GetBlockDictionary(_temperatureCheckedBlocks, _coordinate);
            double totalTemperature = 0;
            int numberOfTemperatureBlocks = 0;
            foreach (RelativeCoordinate r in _temperatureCheckedBlocks)
            {
                if (gridAPI.GetBlock(r, _coordinate) is StateBlock locationStateBlock)
                {
                    totalTemperature += locationStateBlock.Temperature;
                    numberOfTemperatureBlocks++;
                }
            }
            if (numberOfTemperatureBlocks == 0) //handles the case if there are no stateblocks around the called block
            {
                gridAPI.SetBlock(_coordinate, _newBlock);
            }
            else
            {
                double averageTemperature = totalTemperature / numberOfTemperatureBlocks;
                _newBlock.Temperature = averageTemperature;
                gridAPI.SetBlock(_coordinate, _newBlock);
            }

        }
    }
}
