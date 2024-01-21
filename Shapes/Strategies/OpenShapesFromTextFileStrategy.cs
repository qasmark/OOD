using Shapes.Types;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Shapes.Strategies
{
    public sealed class OpenShapesFromTextFileStrategy : OpenShapesStrategy
    {
        protected override List<KeyValuePair<ShapeType, List<string>>> GetShapesInfo(string fileName)
        {
            List<KeyValuePair<ShapeType, List<string>>> shapesInfo = new();

            using StreamReader inFile = new(fileName + ".txt");
            while (!inFile.EndOfStream)
            {
                List<string> shapeParams = inFile
                    .ReadLine()
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
