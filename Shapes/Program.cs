using SFML.Graphics;
using SFML.Window;
using Shapes.Applications;
using Shapes.Types;
using System;
using System.IO;

namespace Shapes
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                StreamReader inputFileStream = DefaultFiles.Input;
                StreamWriter outputFileStream = DefaultFiles.Output;
                RenderWindow window = new(new VideoMode(1000, 1000), "Shapes");

                Application application = new(window, inputFileStream, outputFileStream);

                application.ReadDefaultShapes();
                application.ProcessWindow();
                application.PrintShapesInfo();

                inputFileStream.Close();
                outputFileStream.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error message: {e.Message}");
            }
        }
    }
}
