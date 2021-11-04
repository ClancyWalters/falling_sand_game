using System;

namespace MonoCustomProgram
{
    public static class Program
    {
        [STAThread]
        static void Main()
        {
            using (var game = new MonoMain())
                game.Run();
        }
    }
}
