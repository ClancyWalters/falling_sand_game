using System.Collections.Generic;

namespace CustomProgram
{
    class TrunkFactory //creates the trunk which a tree grows from
    {
        static readonly private List<string> _treeTypes = new List<string>()
        {
            "birch", "maple", "oak"
        };
        public TrunkFactory() { }

        public List<string> TreeTypes { get => _treeTypes; }

        public TrunkBlock CreateTrunk(string treeType)
        {
            return treeType switch
            {
                "birch" => BirchTrunkBlock.Initalize(),
                "maple" => MapleTrunkBlock.Initalize(),
                _ => OakTrunkBlock.Initalize(),
            };
        }
    }
}
