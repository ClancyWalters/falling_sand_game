using FastNoiseLiteLib;
using LocalResouces;
using System;
using System.Collections.Generic;

namespace CustomProgram
{
    class AcidLiquidBlock : LiquidStateBlock, IBurningInterface
    {
        readonly private FastNoiseLite _noise;
        readonly private float _startingLocation;
        static readonly private Random _rngGenerator = new Random();

        private uint currentTicks;

        readonly private FireComponent _fire;
        private bool _burning;
        static readonly private List<RelativeCoordinate> _acidChecks = new List<RelativeCoordinate>() //asks for all the blocks around itself to check oxygen
        {
            RelativeCoordinate.Down, RelativeCoordinate.DownLeft, RelativeCoordinate.DownRight, RelativeCoordinate.Left, RelativeCoordinate.Right
        };
        static public AcidLiquidBlock InitalizeWithTemperature(double temperature)
        {
            return new AcidLiquidBlock(200, 1.76, 0.2, temperature, vColor.RGB(0, 0, 255), "Acid"); //initialization color is overwritten after 1 tick anyway
        }
        static public AcidLiquidBlock Initalize()
        {
            return AcidLiquidBlock.InitalizeWithTemperature(298.15);
        }
        private AcidLiquidBlock(double density, double specificHeatCapacity, double thermalConductivity, double temperature, vColor color, string name) : base(density, specificHeatCapacity, thermalConductivity, temperature, color, name)
        {
            //implements a library that has noise types including Open Simplex noise
            //FROM: https://github.com/Auburn/FastNoise
            //_noise = new FastNoiseLite(_rngGenerator.Next(1, 2000));
            _noise = new FastNoiseLite();
            _noise.SetNoiseType(FastNoiseLite.NoiseType.OpenSimplex2);
            _startingLocation = _rngGenerator.Next(1, 15);
            //_startingLocation = 1;//_rngGenerator.Next(1, 50);

            _fire = new FireComponent(5);
            _burning = false;

            currentTicks = 0;
        }
        protected override ActionHandler PreDefaultQuery(GridBlockAPI gridAPI, GridCoordinate coordinate)
        {
            currentTicks++;

            if (_fire.CanIgnite(gridAPI, coordinate))
            {
                _burning = true;
                _fire.Ignite(this);
            }
            else if (_burning)
            {
                return _fire.CheckHasOxygen(gridAPI, coordinate, Temperature);
            }
            if (!_burning)
            {
                float noiseValue = _noise.GetNoise(_startingLocation, (coordinate.X + coordinate.Y) / 10, currentTicks / 100); //one of the dimentions is time

                double mappedNoise = GeneralResources.Map(noiseValue, -1, 1, 0, 360);
                _vColor = vColor.HSV(mappedNoise, .9, .9);
            }
            if (GeneralResources.GetRandomBool(0.1)) //chance for acid to eat
            {
                //Dictionary<RelativeCoordinate, Block> dict = gridAPI.GetBlockDictionary(_acidChecks, coordinate);
                List<RelativeCoordinate> tempList = new List<RelativeCoordinate>(_acidChecks); //pass by value
                GeneralResources.Shuffle<RelativeCoordinate>(tempList);
                foreach (RelativeCoordinate r in tempList)
                {
                    if (!(gridAPI.GetBlock(r, coordinate) is AcidLiquidBlock) && (gridAPI.GetBlock(r, coordinate) is SolidStateBlock || gridAPI.GetBlock(r, coordinate) is LiquidStateBlock || gridAPI.GetBlock(r, coordinate) is PlantBlock))
                    {
                        if (GeneralResources.GetRandomBool(0.5))
                        {
                            return new TemperatureConsistantBlockChangeHandler(r.GetGridCoordinate(coordinate), AirGasBlock.Initalize());
                        }
                        else
                        {
                            return new TemperatureConsistantBlockChangeHandler(r.GetGridCoordinate(coordinate), AcidLiquidBlock.Initalize());
                        }

                    }
                }
            }
            return null;
        }
        protected override ActionHandler PostDefaultQuery(GridBlockAPI gridAPI, GridCoordinate coordinate)
        {
            if (_burning)
            {
                return _fire.GenerateParticles(gridAPI, coordinate, Temperature);
            }
            return null;
        }

        public override Block Clone()
        {
            return InitalizeWithTemperature(Temperature);
        }

        public FireComponent Fire { get => _fire; }
        public bool Burning { get => _burning; }
    }
}
