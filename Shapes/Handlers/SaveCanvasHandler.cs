using Shapes.Compounds;
using Shapes.Decorators;
using Shapes.Strategies;
using System.Collections.Generic;

namespace Shapes.Handlers
{
    public sealed class SaveCanvasHandler
    {
        private readonly CanvasHandler _canvasHandler;
        private readonly SaveShapesToTextFileStrategy _saveTextStrategy;
        private readonly SaveShapesToBinaryFileStrategy _saveBinaryStrategy;
        private SaveShapesStrategy _saveStrategy;

        private readonly OpenShapesFromTextFileStrategy _openTextStrategy;
        private readonly OpenShapesFromBinaryFileStrategy _openBinaryStrategy;
        private OpenShapesStrategy _openStrategy;

        public SaveCanvasHandler(CanvasHandler canvasHandler)
        {
            _canvasHandler = canvasHandler;
            
            _saveTextStrategy = new SaveShapesToTextFileStrategy();
            _saveBinaryStrategy = new SaveShapesToBinaryFileStrategy();
            SetSaveTextStrategy();

            _openTextStrategy = new OpenShapesFromTextFileStrategy();
            _openBinaryStrategy = new OpenShapesFromBinaryFileStrategy();
            SetOpenTextStrategy();
        }

        public void SetSaveTextStrategy()
        {
            _saveStrategy = _saveTextStrategy;
        }

        public void SetSaveBinaryStrategy()
        {
            _saveStrategy = _saveBinaryStrategy;
        }

        public void SetOpenTextStrategy()
        {
            _openStrategy = _openTextStrategy;
        }

        public void SetOpenBinaryStrategy()
        {
            _openStrategy = _openBinaryStrategy;
        }

        public void Save(string fileName)
        {
            _canvasHandler.UnselectAll();

            _saveStrategy.Save(fileName, _canvasHandler.Shapes);
        }

        public void Open(string fileName)
        {
            _canvasHandler.Clear();

            List<ShapeDecorator> shapes = _openStrategy.Open(fileName);
            shapes.ForEach(x => _canvasHandler.AddShape(x));
        }
    }
}
