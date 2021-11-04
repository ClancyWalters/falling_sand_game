namespace CustomProgram
{
    class ImplimentableBorderBlock : Block, ICloneable
    {
        static public ImplimentableBorderBlock Initalize() //this only exists to make it consistant with other blocks
        {
            return new ImplimentableBorderBlock();
        }
        private ImplimentableBorderBlock() : base(vColor.RGB(0, 0, 0), "Implimentable Border Block") { }

        public Block Clone()
        {
            return new ImplimentableBorderBlock();
        }
    }
}