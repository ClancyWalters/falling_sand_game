using System;
using System.Collections.Generic;

namespace CustomProgram
{
    public class Grid : IModel
    {
        static readonly private Random _randomNumberGenerator = new Random();
        private List<List<Block>> _grid;
        private List<List<GridCoordinate>> _cashedGridCoordinates;
        readonly private int _scale;
        readonly private int _gridWidth;
        readonly private int _gridHeight;
        readonly private AbsoluteCoordinate _origin;
        readonly private List<ActionHandler> _actionQue = new List<ActionHandler>();
        private int _displayModeIndex;
        readonly private GridBlockAPI _gridBlockAPI;
        readonly private GridHandlerAPI _gridHandlerAPI;
        readonly private int _pixelWidth;
        readonly private int _pixelHeight;
        private int _temperatureMidpoint;



        static private List<string> _displayMode = new List<string>()
        {
            "block",
            "temperature"
        };
        /// <summary>
        /// Creates a grid object
        /// </summary>
        public Grid(AbsoluteCoordinate origin, int scale, int pixelWidth, int pixelHeight)
        {
            _origin = origin;
            _scale = scale;
            _pixelWidth = pixelWidth;
            _gridWidth = (pixelWidth - (pixelWidth % scale)) / scale;
            _pixelHeight = pixelHeight;
            _gridHeight = (pixelHeight - (pixelHeight % scale)) / scale;
            _displayModeIndex = 0;
            CreateMap();
            _gridBlockAPI = new GridBlockAPI(this);
            _gridHandlerAPI = new GridHandlerAPI(this);
            _temperatureMidpoint = 500;
        }
        /// <summary>
        /// Creates a new grid of blocks
        /// </summary>
        public void CreateMap()
        {
            List<List<Block>> blockMap = new List<List<Block>>(); //creates the 2d array
            List<List<GridCoordinate>> coordinateGrid = new List<List<GridCoordinate>>();
            for (int i = 0; i < _gridHeight; i++)
            {
                List<Block> blockList = new List<Block>();
                List<GridCoordinate> coordinateList = new List<GridCoordinate>();
                for (int j = 0; j < _gridWidth; j++)
                {
                    blockList.Add(AirGasBlock.Initalize());
                    coordinateList.Add(new GridCoordinate(j, i));
                }
                blockMap.Add(blockList);
                coordinateGrid.Add(coordinateList);
            }

            for (int i = 0; i < blockMap.Count; i++) //sets the outer edge to border blocks
            {
                for (int j = 0; j < blockMap[i].Count; j++)
                {
                    if (i == 0 || j == 0 || i == blockMap.Count - 1 || j == blockMap[i].Count - 1)
                    {
                        blockMap[i][j] = new BorderBlock();
                    }
                }
            }

            _grid = blockMap;
            _cashedGridCoordinates = coordinateGrid;
        }
        /// <summary>
        /// Replaces the block at a given GridCoordinate
        /// </summary>
        internal void ReplaceBlock(GridCoordinate coord, Block block) //set block
        {
            _grid[coord.Y][coord.X] = block;
        }
        /// <summary>
        /// Checks if a GridCoordinate is within the grid
        /// </summary>
        /// <returns>Returns true if the block is within the grid. Otherwise returns false.</returns>
        public bool GetBlockCheck(GridCoordinate blockLocation) //get block from a coordinate
        {
            if (blockLocation.X > _gridWidth - 1 || blockLocation.X < 0 || blockLocation.Y > _gridHeight - 1 || blockLocation.Y < 0)
            {
                return false;
            }
            return true;
        }
        /// <summary>
        /// Gets a block given a GridCoordinate
        /// </summary>
        /// <returns>Returns a reference to a block stored in the grid</returns>
        public Block GetBlock(GridCoordinate blockLocation) //get block from a coordinate
        {
            return _grid[blockLocation.Y][blockLocation.X];
        }
        /// <summary>
        /// Retrieves the vColor object for each block
        /// </summary>
        /// <returns>Returns a grid of vColor objects for drawing</returns>
        public List<List<vColor>> Display()
        {
            return (_displayMode[_displayModeIndex]) switch
            {
                "temperature" => DisplayTemperature(),
                _ => DisplayBlocks(),
            };
        }
        private List<List<vColor>> DisplayBlocks()
        {
            List<List<vColor>> blockColors = new List<List<vColor>>();
            foreach (List<Block> l in _grid) // row
            {
                List<vColor> colorCols = new List<vColor>();
                foreach (Block b in l) //col
                {
                    colorCols.Add(b.Draw());
                }
                blockColors.Add(colorCols);
            }
            //Debug.WriteLine(blockColors.Count);
            return blockColors;
        }
        private List<List<vColor>> DisplayTemperature()
        {
            List<List<vColor>> blockColors = new List<List<vColor>>();
            foreach (List<Block> l in _grid) // row
            {
                List<vColor> colorCols = new List<vColor>();
                foreach (Block b in l) //col
                {
                    if (b is ITemperature tempB)
                    {
                        colorCols.Add(tempB.DrawTemperature(_temperatureMidpoint));
                    }
                    else
                    {
                        colorCols.Add(b.Draw());
                    }
                }
                blockColors.Add(colorCols);
            }
            return blockColors;
        }
        /// <summary>
        /// Processes User Input without updating the grid
        /// </summary>
        public void ProcessUserInput() //deals with processing each time step
        {
            ProcessActions(); //catches user input
        }
        /// <summary>
        /// Updates the grid and processes all actions
        /// </summary>
        public void Tick() //deals with processing each time step
        {
            ProcessUserInput(); //User input processing
            GetActions(); //updates the game world
            UpdateTemperature(); //gets temp changes
            ProcessActions(); //executes temp changes
        }
        private void UpdateTemperature()
        {
            for (int i = 0; i < _grid.Count; i++)
            {
                if (i % 2 == 0)
                {
                    for (int j = 0; j < _grid[i].Count; j += 2)
                    {
                        if (j % 2 == 0)
                        {
                            GetTemperatureQuery(j, i);
                        }
                    }
                }
                else
                {
                    for (int j = 1; j < _grid[i].Count; j += 2)
                    {
                        if (j % 2 != 0)
                        {
                            GetTemperatureQuery(j, i);
                        }
                    }
                }
            }
        }

        private void GetTemperatureQuery(int x, int y) //minimises casting
        {
            if (_grid[y][x] is ITemperature temp)
            {
                temp.TemperatureQuery(_gridBlockAPI, _cashedGridCoordinates[y][x]);
            }
        }
        private void GetActions() //gets the actions
        {
            foreach (List<Block> l in _grid)
            {
                foreach (Block b in l)
                {
                    if (b is IActable actableB)
                    {
                        actableB.HasUpdated = false;
                    }
                }
            }

            for (int i = _grid.Count - 1; i > -1; i--) //loops through array bottom to top
            //for (int i = 0; i < _grid.Count; i++) //loops through array top to bottom
            {
                if (i % 2 == 0) //snakes through array (minimises bias to either side)
                {
                    for (int j = _grid[i].Count - 1; j > -1; j--)
                    {
                        ActionCode(i, j);
                    }
                }
                else
                {
                    for (int j = 0; j < _grid[i].Count; j++)
                    {
                        ActionCode(i, j);
                    }
                }
            }
        }
        private void ActionCode(int i, int j) //exists to improve readability and avoid code duplication
        {
            if (_grid[i][j] is IActable actable)
            {
                if (!actable.HasUpdated)
                {
                    ActionHandler action = actable.ActionQuery(_gridBlockAPI, _cashedGridCoordinates[i][j]);
                    if (action != null)
                    {
                        action.ExecuteAction(_gridHandlerAPI);
                    }
                }
            }
            if (_grid[i][j] is IRandomActable randomActable)
            {
                if (_randomNumberGenerator.Next(0, 60) == 0)
                {
                    ActionHandler action = randomActable.RandomActionQuery(_gridBlockAPI, _cashedGridCoordinates[i][j]);
                    if (action != null)
                    {
                        action.ExecuteAction(_gridHandlerAPI);
                    }
                }
            }
        }
        private void ProcessActions() //executes the actions
        {
            /*
            foreach (ActionHandler action in _actionQue)
            {
                action.ExecuteAction(_gridHandlerAPI);
            }
            */
            for (int i = 0; i < _actionQue.Count; i++)
            {
               _actionQue[i].ExecuteAction(_gridHandlerAPI);
            }
            
            _actionQue.Clear();
        }
        /// <summary>
        /// Adds an action to the grid's action list
        /// </summary>
        public void AddAction(ActionHandler action) //so that the game can add actions (brushes)
        {
            _actionQue.Add(action);
        }
        /// <summary>
        /// Changes the currently selected display type. Current types are: Block color and Temperature color.
        /// </summary>
        public void ChangeDisplayType()
        {
            _displayModeIndex++;
            if (_displayModeIndex >= _displayMode.Count)
            {
                _displayModeIndex = 0;
            }
        }
        /// <summary>
        /// Checks if a pixel position is on the grid
        /// </summary>
        /// <returns>Returns true if the AbsoluteCoordinate is on the grid else returns false</returns>
        public bool CheckMouseOnGrid(AbsoluteCoordinate mousePosAbs)
        {
            return (mousePosAbs.X >= Origin.X && mousePosAbs.Y >= Origin.Y && mousePosAbs.X < _pixelWidth + Origin.X && mousePosAbs.Y < _pixelHeight + Origin.Y);
        }
        /// <summary>
        /// Checks if a pixel position is on the grid
        /// </summary>
        /// <returns>Returns true if the pixel position is on the grid else returns false</returns>
        public bool CheckMouseOnGrid(int x, int y)
        {
            return (x >= Origin.X && y >= Origin.Y && x < _pixelWidth + Origin.X && y < _pixelHeight + Origin.Y);
        }
        /// <summary>
        /// The width of the grid in GridCoordinates
        /// </summary>
        public int Width { get => _gridWidth; }
        /// <summary>
        /// The height of the grid in GridCoordinates
        /// </summary>
        public int Height { get => _gridHeight; }
        /// <summary>
        /// The scale of each block in the grid
        /// </summary>
        public int Scale { get => _scale; }
        /// <summary>
        /// The pixel position of the top left pixel in the grid
        /// </summary>
        public AbsoluteCoordinate Origin { get => _origin; }
        /// <summary>
        /// The midpoint for the displaying temperature values
        /// </summary>
        public int TemperatureMidpoint { get => _temperatureMidpoint; set { if (value > 0 && value < 2000) { _temperatureMidpoint = value; } } }
    }
}
