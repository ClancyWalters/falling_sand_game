namespace CustomProgram
{
    /// <summary>
    /// Base block that all blocks inherit from
    /// </summary>
    public abstract class Block
    {
        readonly private string _name;
        protected vColor _vColor;
        internal Block(vColor color, string name)
        {
            _vColor = color;
            _name = name;
        }
        /// <summary>
        /// Gets the color the block should be drawn as
        /// </summary>
        /// <returns>
        /// Returns the color as a vColor object
        /// </returns>
        internal virtual vColor Draw() //blocks must be able to be drawn
        {
            return _vColor;
        }
        /// <summary>
        /// Return's the blocks color as a vColor object
        /// </summary>
        public vColor VColor { get => _vColor; set => _vColor = value; }
        /// <summary>
        /// Returns the name of a block as a string
        /// </summary>
        public string Name { get => _name; }
    }
}
