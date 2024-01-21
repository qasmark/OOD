using SFML.Graphics;
using System.IO;

namespace Shapes.Types
{
    public static class DefaultFiles
    {
        private static readonly Font _font = new("RobotoUI.ttf");
        private static readonly StreamReader _input = new("input.txt");
        private static readonly StreamWriter _output = new("output.txt");

        public static StreamReader Input => _input;
        public static StreamWriter Output => _output;
        public static Font Font => _font;
        public static string SaveFileName = "export";
        public static string OpenFileName = "export";
    }
}
