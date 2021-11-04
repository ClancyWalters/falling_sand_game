namespace CustomProgram
{
    /// <summary>
    /// Base block for any block that grows
    /// </summary>
    abstract class PlantBlock : Block, IActable, IBurningInterface
    {
        readonly private FireComponent _fire;
        private bool _burning;
        private int _growthLifetime;
        private bool _hasUpdated;
        private bool _finishedGrowing;

        readonly private TemperatureComponent _temperatureComponent;
        public PlantBlock(double specificHeatCapacity, double thermalConductivity, double temperature, int growthLifetime, vColor color, string name) : base(color, name)
        {
            _growthLifetime = growthLifetime;
            _temperatureComponent = new TemperatureComponent(specificHeatCapacity, thermalConductivity, temperature);
            _fire = new FireComponent(3);
            _burning = false;
            _finishedGrowing = false;
        }

        public ActionHandler ActionQuery(GridBlockAPI gridAPI, GridCoordinate coordinate)
        {
            if (_fire.CanIgnite(gridAPI, coordinate))
            {
                _burning = true;
                _fire.Ignite(this);
            }
            if (_burning)
            {
                ActionHandler extinguish = _fire.CheckHasOxygen(gridAPI, coordinate, Temperature);
                if (extinguish != null)
                {
                    return extinguish;
                }
                return _fire.GenerateParticles(gridAPI, coordinate, Temperature);
            }
            if (!FinishedGrowing)
            {
                return GrowthQuery(gridAPI, coordinate);
            }
            ActionHandler action = PostDefaultQuery(gridAPI, coordinate);
            if (action != null)
            {
                return action;
            }
            if (_burning)
            {
                return _fire.GenerateParticles(gridAPI, coordinate, Temperature);
            }
            return null;
        }

        /// <summary>
        /// Overriden for Adding functionality to the Action Query
        /// </summary>
        protected virtual ActionHandler GrowthQuery(GridBlockAPI gridAPI, GridCoordinate coordinate)
        {
            return null;
        }
        /// <summary>
        /// Overriden for Adding functionality to the Action Query
        /// </summary>
        protected virtual ActionHandler PostDefaultQuery(GridBlockAPI gridAPI, GridCoordinate coordinate)
        {
            return null;
        }
        /// <summary>
        /// Performs temperature calculations
        /// </summary>
        public void TemperatureQuery(GridBlockAPI gridAPI, GridCoordinate blockLocation)
        {
            _temperatureComponent.TemperatureQuery(gridAPI, blockLocation);
        }
        /// <summary>
        /// Passes block color for temperature view
        /// </summary>
        public vColor DrawTemperature(int Midpoint)
        {
            return _temperatureComponent.DrawTemperature(Midpoint);
        }
        /// <summary>
        /// Changes block temperature
        /// </summary>
        public void ChangeTemperature(double temperature)
        {
            _temperatureComponent.ChangeTemperature(temperature);
        }
        /// <summary>
        /// Changes growth lifetime. Negative values reduce lifetime.
        /// </summary>
        public virtual void ChangeGrowthLifetime(int changeAmount) //possibly not needed
        {
            _growthLifetime += changeAmount;
        }
        public FireComponent Fire { get => _fire; }
        public bool Burning { get => _burning; }
        public bool HasUpdated { get => _hasUpdated; set => _hasUpdated = value; }
        public double Temperature { get => _temperatureComponent.Temperature; set => _temperatureComponent.Temperature = value; }
        public double ThermalConductivity { get => _temperatureComponent.ThermalConductivity; }
        public double SpecificHeatCapacity { get => _temperatureComponent.SpecificHeatCapacity; }
        protected int GrowthLifetime { get => _growthLifetime; }
        protected bool FinishedGrowing { get => _finishedGrowing; set => _finishedGrowing = value; }
    }
}
