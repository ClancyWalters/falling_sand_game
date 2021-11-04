namespace CustomProgram
{
    public class EraserBrush : Brush
    {
        public EraserBrush(int width, int density) : base(width, density) { }
        /// <summary>
        /// Adds an EraseDrawingHandler to the model action list
        /// </summary>
        public override void Draw(IModel model, AbsoluteCoordinate coord)
        {
            model.AddAction(new EraseDrawingHandler(coord, CashedBrush));
        }

    }
}
