using Shapes.Decorators;
using System;
using System.Collections.Generic;
using System.IO;

namespace Shapes.Strategies
{
    public sealed class SaveShapesToTextFileStrategy : SaveShapesStrategy
    {
        public override void Save(string fileName, List<ShapeDecorator> shapes)
        {
            if (string.IsNullOrEmpty(fileName))
            {
                throw new ArgumentNullException("File name could be null or empty");
            }
            using StreamWriter outFile = new(fileName + ".txt");
            List<string> shapesInfo = GetShapesInfo(shapes);
            shapesInfo.ForEach(x => outFile.WriteLine(x));
        }
    }
}
