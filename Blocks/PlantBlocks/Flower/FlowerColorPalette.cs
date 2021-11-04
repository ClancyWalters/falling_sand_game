using LocalResouces;
using System;
using System.Collections.Generic;

namespace CustomProgram
{
    class FlowerColorPalette
    {
        public FlowerColorPalette() { }

        private static readonly List<string> _colorPalettes = new List<string>()
        {
            "red", "blue", "purple", "pink", "green", "yellow"
        };
        private static readonly Random _rngGenerator = new Random();
        //ask for a color from color pallete
        //ask for color pallete
        public vColor GetColor(string pallete)
        {

            return pallete switch
            {
                "red" => GeneralResources.GenerateColorVariance(vColor.HSV(0, 1, .38), vColor.HSV(0, .63, .71)),
                "blue" => GeneralResources.GenerateColorVariance(vColor.HSV(179, 1, .38), vColor.HSV(179, .63, .71)),
                "purple" => GeneralResources.GenerateColorVariance(vColor.HSV(258, 1, .38), vColor.HSV(258, .63, .71)),
                "pink" => GeneralResources.GenerateColorVariance(vColor.HSV(307, 1, .38), vColor.HSV(307, .63, .71)),
                "green" => GeneralResources.GenerateColorVariance(vColor.HSV(117, .98, .61), vColor.HSV(117, .56, .83)),
                "yellow" => GeneralResources.GenerateColorVariance(vColor.HSV(52, .98, .61), vColor.HSV(52, .56, .83)),
                _ => GeneralResources.GenerateColorVariance(vColor.HSV(52, .98, .61), vColor.HSV(52, .56, .83)),
            };
        }
        public string GetPallete()
        {
            return _colorPalettes[_rngGenerator.Next(0, _colorPalettes.Count)];
        }


    }
}
