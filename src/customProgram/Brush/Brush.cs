using LocalResouces;
using System.Collections.Generic;

namespace CustomProgram
{

    public abstract class Brush
    {
        private int _cashedBrushWidth;
        private float _density;
        private List<RelativeCoordinate> _cashedBrush;

        public Brush(int width, int density)
        {
            _cashedBrushWidth = width;
            _cashedBrush = new List<RelativeCoordinate>(ProjectResource.GenerateCoordinateCircle(width));
            _density = density;
        }

        protected List<RelativeCoordinate> CashedBrush { get => _cashedBrush; }

        /// <summary>
        /// Changes the size radius of the list of relative coordinates used
        /// for drawing
        /// </summary>
        public void AlterBrush(int size, float density)
        {
            if ((size >= 0 && size < 20 && size != _cashedBrushWidth) || (density >= 0 && density <= 1 && density != _density))
            {
                _cashedBrush = ProjectResource.RemoveCoordinates(ProjectResource.GenerateCoordinateCircle(size), _density);
                _cashedBrushWidth = size;
                _density = density;
            }
        }
        /// <summary>
        /// Adds a DrawingHandler to the model's action list.
        /// </summary>
        public abstract void Draw(IModel model, AbsoluteCoordinate coord);
    }
}
