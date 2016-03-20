using System.Collections.Generic;

namespace WinToolkit.Controls.Imaging
{
    internal sealed class ImageBackgroundProcessor
    {
        private readonly bool[][] _isVisited;
        private readonly int _pixelWidth;
        private readonly int _pixelHeight;

        public ImageBackgroundProcessor(int pixelWidth, int pixelHeight)
        {
            _isVisited = new bool[pixelHeight][];
            _pixelWidth = pixelWidth;
            _pixelHeight = pixelWidth;
        }

        public void Process(byte[] pixelsData)
        {
            var queue = new Queue<Pixel>();
            var firstPixel = new Pixel(pixelsData, 0, _pixelWidth, _pixelHeight);
            queue.Enqueue(firstPixel);
            MarkAsVisited(firstPixel);


            while (queue.Count > 0)
            {
                Pixel pixel = queue.Dequeue();
                pixel.ChangeColor(150);

                Pixel[] adjacents = pixel.GetAdjacents();
                foreach (Pixel adj in adjacents)
                {
                    if (adj.IsWhite && !IsVisited(adj))
                    {
                        MarkAsVisited(adj);
                        queue.Enqueue(adj);
                    }
                }
            }
        }

        private void MarkAsVisited(Pixel pixel)
        {
            int index = pixel.Index;
            int row = index / _pixelWidth;
            int col = index % _pixelWidth;

            if (_isVisited[row] == null)
                _isVisited[row] = new bool[_pixelWidth];

            _isVisited[row][col] = true;

        }

        private bool IsVisited(int row, int col)
        {
            if (_isVisited[row] == null)
                return false;

            return _isVisited[row][col];
        }

        private bool IsVisited(Pixel pixel) => IsVisited(pixel.Row, pixel.Col);
    }
}
