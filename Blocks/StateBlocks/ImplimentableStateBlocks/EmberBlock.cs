using LocalResouces;
using System;
using System.Collections.Generic;

namespace CustomProgram
{
    class EmberBlock : StateBlock, IBurningInterface, IActable //embers are not a specific state (do not contain functionality specific to any state) and hence inherit the stateblock directly
    {
        static readonly private Random _randomNumberGenerator = new Random();
        private int _lifetime;

        readonly private bool _burning = true;

        static readonly private List<RelativeCoordinate> _checkEmberPlacement = new List<RelativeCoordinate>()
        {
            RelativeCoordinate.Up, RelativeCoordinate.UpLeft, RelativeCoordinate.UpRight
        };
        static public EmberBlock InitalizeWithTemperatureAndLifetime(double temperature, int lifetime)
        {

            return new EmberBlock(0.1, 2, 0.2, temperature, GeneralResources.GenerateColorVariance(vColor.HSV(29, 1, 1), vColor.HSV(42, 1, 1)), lifetime, "Ember");
        }
        static public EmberBlock InitalizeWithTemperature(double temperature)
        {
            return EmberBlock.InitalizeWithTemperatureAndLifetime(temperature, _randomNumberGenerator.Next(5, 45));
        }
        static public EmberBlock Initalize()
        {
            return EmberBlock.InitalizeWithTemperature(1373.15);
        }
        private EmberBlock(double density, double specificHeatCapacity, double thermalConductivity, double temperature, vColor color, int lifetime, string name) : base(density, specificHeatCapacity, thermalConductivity, temperature, color, name)
        {
            _lifetime = lifetime;
        }

        public ActionHandler ActionQuery(GridBlockAPI gridAPI, GridCoordinate coordinate)
        {
            if (_lifetime < 1 || Temperature < 400) //check if it should get rid of itself
            {
                return new BlockChangeHandler(coordinate, SmokeGasBlock.InitalizeWithTemperature(Temperature));
            }

            _lifetime--;

            //Dictionary<RelativeCoordinate, Block> dict = gridAPI.GetBlockDictionary(_checkEmberPlacement, coordinate);
            if (GeneralResources.GetRandomBool(0.001))
            {
                if (GeneralResources.GetRandomBool(0.1))
                {
                    if (GeneralResources.GetRandomBool(0.8))
                    {
                        if (gridAPI.GetBlock(RelativeCoordinate.UpRight, coordinate) is GasStateBlock)
                        {
                            return new BlockChangeHandler(RelativeCoordinate.UpRight.GetGridCoordinate(coordinate), EmberBlock.InitalizeWithTemperatureAndLifetime(Temperature, _lifetime / 2));
                        }
                        if (gridAPI.GetBlock(RelativeCoordinate.UpLeft, coordinate) is GasStateBlock)
                        {
                            return new BlockChangeHandler(RelativeCoordinate.UpLeft.GetGridCoordinate(coordinate), EmberBlock.InitalizeWithTemperatureAndLifetime(Temperature, _lifetime / 2));
                        }
                    }
                    else
                    {
                        if (gridAPI.GetBlock(RelativeCoordinate.UpLeft, coordinate) is GasStateBlock)
                        {
                            return new BlockChangeHandler(RelativeCoordinate.UpLeft.GetGridCoordinate(coordinate), EmberBlock.InitalizeWithTemperatureAndLifetime(Temperature, _lifetime / 2));
                        }
                        if (gridAPI.GetBlock(RelativeCoordinate.UpRight, coordinate) is GasStateBlock)
                        {
                            return new BlockChangeHandler(RelativeCoordinate.UpRight.GetGridCoordinate(coordinate), EmberBlock.InitalizeWithTemperatureAndLifetime(Temperature, _lifetime / 2));
                        }
                    }
                    if (gridAPI.GetBlock(RelativeCoordinate.Up, coordinate) is GasStateBlock)
                    {
                        return new BlockChangeHandler(RelativeCoordinate.Up.GetGridCoordinate(coordinate), EmberBlock.InitalizeWithTemperatureAndLifetime(Temperature, _lifetime));
                    }
                }
                else
                {
                    if (GeneralResources.GetRandomBool(0.3))
                    {
                        if (gridAPI.GetBlock(RelativeCoordinate.UpRight, coordinate) is GasStateBlock)
                        {
                            return new BlockChangeHandler(RelativeCoordinate.UpRight.GetGridCoordinate(coordinate), SmokeGasBlock.InitalizeWithTemperature(Temperature));
                        }
                        if (gridAPI.GetBlock(RelativeCoordinate.UpLeft, coordinate) is GasStateBlock)
                        {
                            return new BlockChangeHandler(RelativeCoordinate.UpLeft.GetGridCoordinate(coordinate), SmokeGasBlock.InitalizeWithTemperature(Temperature));
                        }
                    }
                    else
                    {
                        if (gridAPI.GetBlock(RelativeCoordinate.UpLeft, coordinate) is GasStateBlock)
                        {
                            return new BlockChangeHandler(RelativeCoordinate.UpLeft.GetGridCoordinate(coordinate), SmokeGasBlock.InitalizeWithTemperature(Temperature));
                        }
                        if (gridAPI.GetBlock(RelativeCoordinate.UpRight, coordinate) is GasStateBlock)
                        {
                            return new BlockChangeHandler(RelativeCoordinate.UpRight.GetGridCoordinate(coordinate), SmokeGasBlock.InitalizeWithTemperature(Temperature));
                        }
                    }
                    if (gridAPI.GetBlock(RelativeCoordinate.Up, coordinate) is GasStateBlock)
                    {
                        return new BlockChangeHandler(RelativeCoordinate.Up.GetGridCoordinate(coordinate), SmokeGasBlock.InitalizeWithTemperature(Temperature));
                    }
                }
            }
            return null;
        }

        public override Block Clone()
        {
            return InitalizeWithTemperatureAndLifetime(Temperature, _lifetime);
        }

        public bool Burning { get => _burning; }
    }

}
