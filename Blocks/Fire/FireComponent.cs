using LocalResouces;
using System.Collections.Generic;

namespace CustomProgram
{

    class FireComponent
    {
        private int _lifetime;

        static readonly private List<RelativeCoordinate> _oxygenCheckBlocks = new List<RelativeCoordinate>() //asks for all the blocks around itself to check oxygen
        {
            RelativeCoordinate.Down,
            RelativeCoordinate.DownLeft,
            RelativeCoordinate.DownRight,
            RelativeCoordinate.Left,
            RelativeCoordinate.Right,
            RelativeCoordinate.Up,
            RelativeCoordinate.UpLeft,
            RelativeCoordinate.UpRight
        };
        static readonly private List<RelativeCoordinate> _particalCandidates = new List<RelativeCoordinate>() //asks for some of the blocks around itself to produce particles
        {
            RelativeCoordinate.Left,
            RelativeCoordinate.Right,
            RelativeCoordinate.Up,
            RelativeCoordinate.UpLeft,
            RelativeCoordinate.UpRight
        };
        public FireComponent(int lifetime)
        {
            _lifetime = lifetime;
        }
        /// <summary>
        /// Changes the color of the block and increases its temperature
        /// </summary>
        public void Ignite(ITemperature block)
        {
            block.ChangeTemperature(1350 - block.Temperature);
            (block as Block).VColor = GeneralResources.GenerateColorVariance(vColor.HSV(22, 1, 0.9), vColor.HSV(4, 1, 0.9));
        }
        /// <summary>
        /// Checks if a block is eligible to ignite
        /// </summary>
        public bool CanIgnite(GridBlockAPI gridAPI, GridCoordinate coordinate)
        {
            //Dictionary<RelativeCoordinate, Block> dict = gridAPI.GetBlockDictionary(_oxygenCheckBlocks, coordinate);
            bool hasFire = false;
            bool hasOxygen = false;
            foreach (RelativeCoordinate r in _oxygenCheckBlocks)
            {
                if (gridAPI.GetBlock(r, coordinate) is AirGasBlock)
                {
                    hasOxygen = true;
                }
                else if (gridAPI.GetBlock(r, coordinate) is IBurningInterface burningIntBlock)
                {
                    if (burningIntBlock.Burning)
                    {
                        hasFire = true;
                    }
                }
            }
            return hasOxygen && hasFire;
        }
        /// <summary>
        /// Checks if a block has oxygen. If it has no oxygen the block should 'extinguish'
        /// </summary>
        public ActionHandler CheckHasOxygen(GridBlockAPI gridAPI, GridCoordinate coordinate, double temperature)
        {
            //Dictionary<RelativeCoordinate, Block> dict = gridAPI.GetBlockDictionary(_oxygenCheckBlocks, coordinate);
            bool hasOxygen = false;
            foreach (RelativeCoordinate r in _oxygenCheckBlocks)
            {
                if (gridAPI.GetBlock(r, coordinate) is AirGasBlock)
                {
                    hasOxygen = true;
                }
            }
            if (!hasOxygen || _lifetime < 1) //if it has no oxygen or it is out of life (could be considered fuel) it turns into ash
            {
                bool generateAshChance = GeneralResources.GetRandomBool(0.1);
                if (generateAshChance)
                {
                    return new BlockChangeHandler(coordinate, AshSolidBlock.InitalizeWithTemperature(temperature));
                }
                return new BlockChangeHandler(coordinate, AirGasBlock.InitalizeWithTemperature(temperature));
            }
            _lifetime--;
            return null;
        }
        /// <summary>
        /// Generates smoke or embers around the burning block
        /// </summary>
        public ActionHandler GenerateParticles(GridBlockAPI gridAPI, GridCoordinate coordinate, double temperature)
        {
            //Dictionary<RelativeCoordinate, Block> dict = gridAPI.GetBlockDictionary(_particalCandidates, coordinate);
            int OxygenCount = 0;
            foreach (RelativeCoordinate r in _particalCandidates)
            {
                if (gridAPI.GetBlock(r, coordinate) is AirGasBlock)
                {
                    OxygenCount++;
                }
            }
            if (OxygenCount > 1) //this exists because its possible embers could snuff out flames
            {
                bool makeParticle = GeneralResources.GetRandomBool(0.1);
                if (makeParticle)
                {
                    bool firstParticleCreationOrder = GeneralResources.GetRandomBool(0.5);
                    if (firstParticleCreationOrder)
                    {
                        if (gridAPI.GetBlock(RelativeCoordinate.UpRight, coordinate) is GasStateBlock)
                        {
                            return new BlockChangeHandler(RelativeCoordinate.UpRight.GetGridCoordinate(coordinate), EmberBlock.InitalizeWithTemperature(temperature));
                        }
                        if (gridAPI.GetBlock(RelativeCoordinate.UpLeft, coordinate) is GasStateBlock)
                        {
                            return new BlockChangeHandler(RelativeCoordinate.UpLeft.GetGridCoordinate(coordinate), EmberBlock.InitalizeWithTemperature(temperature));
                        }
                    }
                    else
                    {
                        if (gridAPI.GetBlock(RelativeCoordinate.UpLeft, coordinate) is GasStateBlock)
                        {
                            return new BlockChangeHandler(RelativeCoordinate.UpLeft.GetGridCoordinate(coordinate), EmberBlock.InitalizeWithTemperature(temperature));
                        }
                        if (gridAPI.GetBlock(RelativeCoordinate.UpRight, coordinate) is GasStateBlock)
                        {
                            return new BlockChangeHandler(RelativeCoordinate.UpRight.GetGridCoordinate(coordinate), EmberBlock.InitalizeWithTemperature(temperature));
                        }
                    }
                    if (gridAPI.GetBlock(RelativeCoordinate.Up, coordinate) is GasStateBlock)
                    {
                        return new BlockChangeHandler(RelativeCoordinate.Up.GetGridCoordinate(coordinate), EmberBlock.InitalizeWithTemperature(temperature));
                    }
                    bool secondParticleCreationOrder = GeneralResources.GetRandomBool(0.5);
                    if (secondParticleCreationOrder)
                    {
                        if (gridAPI.GetBlock(RelativeCoordinate.Right, coordinate) is GasStateBlock)
                        {
                            return new BlockChangeHandler(RelativeCoordinate.Right.GetGridCoordinate(coordinate), EmberBlock.InitalizeWithTemperature(temperature));
                        }
                        if (gridAPI.GetBlock(RelativeCoordinate.Left, coordinate) is GasStateBlock)
                        {
                            return new BlockChangeHandler(RelativeCoordinate.Left.GetGridCoordinate(coordinate), EmberBlock.InitalizeWithTemperature(temperature));
                        }
                    }
                    else
                    {
                        if (gridAPI.GetBlock(RelativeCoordinate.Left, coordinate) is GasStateBlock)
                        {
                            return new BlockChangeHandler(RelativeCoordinate.Left.GetGridCoordinate(coordinate), EmberBlock.InitalizeWithTemperature(temperature));
                        }
                        if (gridAPI.GetBlock(RelativeCoordinate.Right, coordinate) is GasStateBlock)
                        {
                            return new BlockChangeHandler(RelativeCoordinate.Right.GetGridCoordinate(coordinate), EmberBlock.InitalizeWithTemperature(temperature));
                        }
                    }
                }
            }
            return null;
        }
    }
}
