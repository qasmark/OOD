using Shapes.Decorators;
using System;
using System.Collections.Generic;
using System.IO;

namespace Shapes.Strategies
{
    public sealed class SaveShapesToBinaryFileStrategy : SaveShapesStrategy
    {
        public override void Save(string fileName, List<ShapeDecorator> shapes)
        {
            if (string.IsNullOrEmpty(fileName))
            {
                throw new ArgumentNullException("File name could be null or empty");
            }
            using FileStream stream = new(fileName + ".dat", FileMode.Create);
            using BinaryWriter outFile = new(stream);

            List<string> shapesInfo = GetShapesInfo(shapes);
            shapesInfo.ForEach(x => outFile.Write(x));
        }
    }
}
