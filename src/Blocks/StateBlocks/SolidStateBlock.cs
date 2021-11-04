using System;
namespace CustomProgram
{
    abstract class SolidStateBlock : StateBlock, IActable
    {
        readonly private static Random _randomNumberGenerator = new Random();
        public SolidStateBlock(double density, double specificHeatCapacity, double thermalConductivity, double temperature, vColor color, string name) : base(density, specificHeatCapacity, thermalConductivity, temperature, color, name) { }
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
        protected virtual ActionHandler MovementQuery(GridBlockAPI gridAPI, GridCoordinate coordinate)
        {
            //Dictionary<RelativeCoordinate, Block> blockDictionary = gridAPI.GetBlockDictionary(_movementRelavantBlocks, coordinate, _movementRelavantBlocks.Count);
            if (CheckMovable(gridAPI.GetBlock(RelativeCoordinate.Down, coordinate)))
            {
                return new BlockSwitchHandler(coordinate, RelativeCoordinate.Down);
            }
            //gridAPI.GetBlock(RelativeCoordinate.Down, coordinate)
            //gridAPI.GetBlock(RelativeCoordinate.Left, coordinate)
            //gridAPI.GetBlock(RelativeCoordinate.Right, coordinate)
            //gridAPI.GetBlock(RelativeCoordinate.DownLeft, coordinate)
            //gridAPI.GetBlock(RelativeCoordinate.DownRight, coordinate)

            //simulates gravity
            int randomNumber = _randomNumberGenerator.Next(1, 3);
            switch (randomNumber)
            {
                case 1:
                    if (CheckMovable(gridAPI.GetBlock(RelativeCoordinate.Left, coordinate)) && CheckMovable(gridAPI.GetBlock(RelativeCoordinate.DownLeft, coordinate)))
                    {
                        return new GravityHandler(coordinate, RelativeCoordinate.DownLeft);
                    }
                    if (CheckMovable(gridAPI.GetBlock(RelativeCoordinate.Right, coordinate)) && CheckMovable(gridAPI.GetBlock(RelativeCoordinate.DownRight, coordinate)))
                    {
                        return new GravityHandler(coordinate, RelativeCoordinate.DownRight);
                    }
                    break;
                case 2:
                    if (CheckMovable(gridAPI.GetBlock(RelativeCoordinate.Right, coordinate)) && CheckMovable(gridAPI.GetBlock(RelativeCoordinate.DownRight, coordinate)))
                    {
                        return new GravityHandler(coordinate, RelativeCoordinate.DownRight);
                    }
                    if (CheckMovable(gridAPI.GetBlock(RelativeCoordinate.Left, coordinate)) && CheckMovable(gridAPI.GetBlock(RelativeCoordinate.DownLeft, coordinate)))
                    {
                        return new GravityHandler(coordinate, RelativeCoordinate.DownLeft);
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
            if (block is StateBlock && !(block is SolidStateBlock))
            {
                if (!(block as StateBlock).HasUpdated || block is GasStateBlock)
                {
                    if ((block as StateBlock).Density < Density)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
    }
}