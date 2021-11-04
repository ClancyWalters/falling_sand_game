using System.Collections.Generic;

namespace CustomProgram
{
    public interface IModel
    {
        /// <summary>
        /// Creates the grid's map
        /// </summary>
        void CreateMap();
        /// <summary>
        /// Passes a list of vColor objects for display
        /// </summary>
        List<List<vColor>> Display();
        /// <summary>
        /// Causes the model to update
        /// </summary>
        void Tick();
        /// <summary>
        /// Causes the model to change the current display type
        /// </summary>
        void ChangeDisplayType();
        /// <summary>
        /// Returns the block corresponding to a grid position
        /// </summary>
        Block GetBlock(GridCoordinate blockLocation);
        /// <summary>
        /// Checks if an AbsoluteCoordinate corresponds to a position on the grid
        /// </summary>
        bool CheckMouseOnGrid(AbsoluteCoordinate absoluteCoordinate);
        /// <summary>
        /// Checks if an pixel position corresponds to a position on the grid
        /// </summary>
        bool CheckMouseOnGrid(int x, int y);
        /// <summary>
        /// Adds an ActionHandler to the grid's action list
        /// </summary>
        void AddAction(ActionHandler action);
        /// <summary>
        /// Processes User Input without updating the model
        /// </summary>
        void ProcessUserInput();
        /// <summary>
        /// The width of the grid in GridCoordinates
        /// </summary>
        int Width { get; }
        /// <summary>
        /// The height of the grid in GridCoordinates
        /// </summary>
        int Height { get; }
        /// <summary>
        /// The scale of the grid
        /// </summary>
        int Scale { get; }
        /// <summary>
        /// Passes pixel position of the top left corner of the grid
        /// </summary>
        AbsoluteCoordinate Origin { get; }
        /// <summary>
        /// The midpoint for representation of temperature as color
        /// </summary>
        int TemperatureMidpoint { get; set; }
    }
}
