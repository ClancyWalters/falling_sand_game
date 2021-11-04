using LocalResouces;
using System.Collections.Generic;
namespace CustomProgram
{
    class CloneBlock : Block, IActable, ICloneable
    {
        private bool _hasUpdated;

        readonly private ICloneable _replacementBlock;

        static readonly private List<RelativeCoordinate> _cloneReplaceable = new List<RelativeCoordinate>() //asks for all the blocks around itself to check oxygen
        {
            RelativeCoordinate.Down, RelativeCoordinate.DownLeft, RelativeCoordinate.DownRight, RelativeCoordinate.Left, RelativeCoordinate.Right, RelativeCoordinate.Up, RelativeCoordinate.UpLeft, RelativeCoordinate.UpRight
        };
        static public CloneBlock Initalize(ICloneable replacementBlock) //this only exists to make it consistant with other blocks
        {
            return new CloneBlock(replacementBlock, "Clone Block");
        }
        private CloneBlock(ICloneable replacementBlock, string name) : base(vColor.RGB(0, 0, 0), name)
        {
            _replacementBlock = replacementBlock;
        }

        public ActionHandler ActionQuery(GridBlockAPI gridAPI, GridCoordinate coordinate)
        {
            //Dictionary<RelativeCoordinate, Block> dict = gridAPI.GetBlockDictionary(_cloneReplaceable, coordinate);
            List<RelativeCoordinate> outputList = new List<RelativeCoordinate>();
            foreach (RelativeCoordinate r in _cloneReplaceable)
            {
                if (gridAPI.GetBlock(r, coordinate) is StateBlock)
                {
                    if (GeneralResources.GetRandomBool(0.3))
                    {
                        outputList.Add(r);
                    }
                }
            }
            if (outputList.Count != 0)
            {
                return new CloneHandler(coordinate, _replacementBlock, outputList);
            }
            return null;
        }

        public Block Clone()
        {
            return Initalize(_replacementBlock);
        }

        public bool HasUpdated { get => _hasUpdated; set => _hasUpdated = value; }
    }
}