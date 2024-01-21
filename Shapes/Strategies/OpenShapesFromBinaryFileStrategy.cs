using Shapes.Types;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Shapes.Strategies
{
    public sealed class OpenShapesFromBinaryFileStrategy : OpenShapesStrategy
    {
        protected override List<KeyValuePair<ShapeType, List<string>>> GetShapesInfo(string fileName)
        {
            List<KeyValuePair<ShapeType, List<string>>> shapesInfo = new();

            using FileStream stream = new(fileName + ".dat", FileMode.Open);
            using BinaryReader inFile = new(stream);

            while (inFile.BaseStream.Position < inFile.BaseStream.Length)
            {
                List<string> shapeParams = inFile
                    .ReadString()
                    .Split(" ")
                    .Where(s => !string.IsNullOrEmpty(s))
                    .ToList();
                if (shapeParams.Count == 0 || !Enum.TryParse(shapeParams[0], true, out ShapeType shapeType))
                {
                    continue;
                }

                shapeParams.RemoveAt(0);
                shapesInfo.Add(new(shapeType, shapeParams));
            }

            return shapesInfo;
        }
    }
}
