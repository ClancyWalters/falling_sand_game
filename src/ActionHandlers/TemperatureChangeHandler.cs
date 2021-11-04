using System.Collections.Generic;
namespace CustomProgram
{
    class TemperatureChangeHandler : ActionHandler
    {
        readonly private List<CoordinateTemperaturePair> _coordinateTemperaturePairList;
        readonly private GridCoordinate _origin;
        public TemperatureChangeHandler(List<CoordinateTemperaturePair> coordinateTemperatureList, GridCoordinate originPosition) //changes every block by a amount determined in its corresponding value
        {
            _coordinateTemperaturePairList = coordinateTemperatureList;
            _origin = originPosition;
        }
        internal override void ExecuteAction(GridHandlerAPI gridAPI)
        {
            foreach (CoordinateTemperaturePair p in _coordinateTemperaturePairList)
            {
                Block block = gridAPI.GetBlock(_origin, p.Coordinate);
                (block as ITemperature).ChangeTemperature(p.Temperature);
            }
        }
    }
}
