using System.Collections.Generic;

namespace WinToolkit.Controls.Imaging
{
    internal sealed class Pixel
    {
        private readonly int _pixelsWidth;
        private readonly int _pixelsHeight;
        private readonly byte[] _pixels;

        public Pixel(byte[] pixels, int index, int pixelsWidth, int pixelsHeight)
        {
            _pixels = pixels;
            Index = index;
            _pixelsWidth = pixelsWidth;
            _pixelsHeight = pixelsHeight;
        }

        public int Index { get; private set; }

        public bool IsWhite
        {
            get
            {

                byte color = 240;
                int pixelsDataIndex = Index * 4;
                for (int i = pixelsDataIndex; i < pixelsDataIndex + 4; i++)
                    if (_pixels[i] < color)
                        return false;

                return true;
            }
        }

        public void ChangeColor(byte newColor)
        {
            int pixelsDataIndex = Index * 4;
            for (int i = pixelsDataIndex; i < pixelsDataIndex + 4; i++)
                _pixels[i] = newColor;
        }

        public Pixel[] GetAdjacents()
        {
            var result = new List<Pixel>();

            int row = Row;
            int col = Col;

            if (row > 0)
                AddPixel(GetMatrixIndex(row - 1, col), result);

            if (row < _pixelsHeight - 1)
                AddPixel(GetMatrixIndex(row + 1, col), result);


            if (col > 0)
                AddPixel(GetMatrixIndex(row, col - 1), result);


            if (col < _pixelsWidth - 1)
                AddPixel(GetMatrixIndex(row, col + 1), result);


            if (row > 0 && col > 0)
                AddPixel(GetMatrixIndex(row - 1, col - 1), result);


            if (row < _pixelsHeight - 1 && col > 0)
                AddPixel(GetMatrixIndex(row + 1, col - 1), result);


            if (row > 0 && col < _pixelsWidth - 1)
                AddPixel(GetMatrixIndex(row - 1, col + 1), result);


            if (row < _pixelsHeight - 1 && col < _pixelsWidth - 1)
                AddPixel(GetMatrixIndex(row + 1, col + 1), result);


            return result.ToArray();
        }

        public int Row => Index / _pixelsWidth;
        public int Col => Index % _pixelsWidth;
        private int GetMatrixIndex(int row, int col) => row * _pixelsWidth + col;

        private void AddPixel(int index, List<Pixel> adjacents)
        {
            var pixel = new Pixel(_pixels, index, _pixelsWidth, _pixelsHeight);
            adjacents.Add(pixel);
        }
    }
}
