using System.Collections.Generic;

namespace CustomProgram
{
    /// <summary>
    /// Derived from the ActionHandler and handles actions from brushes
    /// </summary>
    abstract class DrawingHandler : ActionHandler
    {
        private readonly AbsoluteCoordinate _mousePosition;
        private readonly List<RelativeCoordinate> _includedPoints;
        public DrawingHandler(AbsoluteCoordinate mousepos, List<RelativeCoordinate> includedPoints)
        {
            _mousePosition = mousepos;
            _includedPoints = includedPoints;
        }
        public AbsoluteCoordinate MousePosition { get => _mousePosition; }
        public List<RelativeCoordinate> IncludedPoints { get => _includedPoints; }
        internal override abstract void ExecuteAction(GridHandlerAPI gridAPI);


    }
}
