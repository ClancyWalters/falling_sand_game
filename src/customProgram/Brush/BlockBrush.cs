using System.Collections.Generic;

namespace CustomProgram
{
    public class BlockBrush : Brush
    {
        private int _selectedBlock;
        private bool _cloneBlockPlacement;
        private static List<ICloneable> _blockList = new List<ICloneable>()
        {
                ImplimentableBorderBlock.Initalize(),
                TemperaturePermiableBarrierBlock.InitalizeWithTemperature(290),
                ConstantTemperatureBlock.InitalizeWithTemperature(10),
                ConstantTemperatureBlock.InitalizeWithTemperature(273),
                ConstantTemperatureBlock.InitalizeWithTemperature(400),
                ConstantTemperatureBlock.InitalizeWithTemperature(3000),
                StoneSolidBlock.Initalize(),
                DirtSolidBlock.Initalize(),
                SeedSolidBlock.Initalize(),
                SandSolidBlock.Initalize(),
                WaterLiquidBlock.Initalize(),
                OilLiquidBlock.Initalize(),
                WoodSolidBlock.Initalize(),
                CoalSolidBlock.Initalize(),
                GunpowderSolidBlock.Initalize(),
                EmberBlock.Initalize(),
                LavaLiquidBlock.InitalizeWithTemperature(1500),
                AcidLiquidBlock.Initalize(),
                NitrogenGasBlock.Initalize(),
                HydrogenGasBlock.Initalize(),
                CarbonDioxideGasBlock.Initalize(),
                MeteorSolidBlock.Initalize(),
                FireworkSolidBlock.Initalize()
        };
        /// <summary>
        /// Creates a BlockBrush
        /// </summary>
        public BlockBrush(int width, int density) : base(width, density)
        {
            _selectedBlock = 0;
            _cloneBlockPlacement = false;
        }
        /// <summary>
        /// Increases the saved index corresponding to the _blockList
        /// </summary>
        public void IncrementSelectedBlock()
        {
            _selectedBlock++;
            if (_selectedBlock >= _blockList.Count)
            {
                _selectedBlock = 0;
            }

        }
        /// <summary>
        /// Decreases the saved index corresponding to the _blockList
        /// </summary>
        public void DecrementSelectedBlock()
        {
            _selectedBlock--;
            if (_selectedBlock < 0)
            {
                _selectedBlock = _blockList.Count - 1;
            }
        }
        /// <summary>
        /// Adds a BlockDrawingHandler to the model action list
        /// </summary>
        public override void Draw(IModel model, AbsoluteCoordinate coord)
        {
            if (_cloneBlockPlacement)
            {
                model.AddAction(new BlockDrawingHandler(coord, CashedBrush, CloneBlock.Initalize(_blockList[_selectedBlock])));
            }
            else
            {
                //Debug.WriteLine("A block action has been called");
                model.AddAction(new BlockDrawingHandler(coord, CashedBrush, _blockList[_selectedBlock]));
            }
        }
        /// <summary>
        /// Returns the currently selected block from the block list
        /// </summary>
        public ICloneable CurrentlySelectedBlock { get => _blockList[_selectedBlock]; }
        /// <summary>
        /// Toggles if the BlockDrawingHandler should add blocks directly or as clone blocks
        /// </summary>
        public bool CloneBlockPlacement { get => _cloneBlockPlacement; set => _cloneBlockPlacement = value; }
    }
}
