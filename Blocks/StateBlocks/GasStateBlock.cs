using System;
namespace CustomProgram
{
    abstract class GasStateBlock : StateBlock, IActable
    {
        static readonly private Random _randomNumberGenerator = new Random();
        public GasStateBlock(double density, double specificHeatCapacity, double thermalConductivity, double temperature, vColor color, string name) : base(density, specificHeatCapacity, thermalConductivity, temperature, color, name) { }
        public virtual ActionHandler ActionQuery(GridBlockAPI gridAPI, GridCoordinate coordinate)
        {
            ActionHandler action;
            action = PreDefaultQuery(gridAPI, coordinate);
            if (action != null)
            {
                return action;
            }
            action = MovementQuery(gridAPI, coordinate);
            if (action != null)
            {
                return action;
            }
            action = PostDefaultQuery(gridAPI, coordinate);
            if (action != null)
            {
                return action;
            }
            return null;
        }
        private ActionHandler MovementQuery(GridBlockAPI gridAPI, GridCoordinate coordinate)
        {
            //Dictionary<RelativeCoordinate, Block> blockDictionary = gridAPI.GetBlockDictionary(_movementRelavantBlocks, coordinate, _movementRelavantBlocks.Count);

            if (CheckMovable(gridAPI.GetBlock(RelativeCoordinate.Down, coordinate)))
            {
                return new BlockSwitchHandler(coordinate, RelativeCoordinate.Down);
            }

            int randomNumber = _randomNumberGenerator.Next(1, 3);
            //simulates liquid like properties (will become flat)
            switch (randomNumber)
            {
                case 1:
                    if (CheckMovable(gridAPI.GetBlock(RelativeCoordinate.Left, coordinate)))
                    {
                        return new BlockSwitchHandler(coordinate, RelativeCoordinate.Left);
                    }
                    break;
                case 2:
                    if (CheckMovable(gridAPI.GetBlock(RelativeCoordinate.Right, coordinate)))
                    {
                        return new BlockSwitchHandler(coordinate, RelativeCoordinate.Right);
                    }
                    break;
            }
            return null;
        }
        //implemented a non-virtual interface pattern to allow children to add functionallity to the action query
        protected virtual ActionHandler PreDefaultQuery(GridBlockAPI gridAPI, GridCoordinate coordinate)
        {
            return null;
        }
        protected virtual ActionHandler PostDefaultQuery(GridBlockAPI gridAPI, GridCoordinate coordinate)
        {
            return null;
        }
        private bool CheckMovable(Block block) //only move into a gas state block
        {
            if (block is StateBlock)
            {
                if (!(block as StateBlock).HasUpdated)
                {
                    if ((block as StateBlock).Density < Density)
                    {
                        return true;
                    }
                    else if ((block as StateBlock).Density == Density && block is GasStateBlock && ((block as GasStateBlock).Temperature) - Temperature > 1)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
    }
}