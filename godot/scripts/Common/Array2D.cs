using System.Collections.Generic;
using Godot;

namespace PuzzleFight.Common
{
    public class Array2D<T>
    {
        private List<T> _data;
        private int _width;
        private int _height;
        private T _default;

        public int Width => _width;

        public int Height => _height;

        public T this[int x, int y]
        {
            get => At(x, y);
            set => Set(x, y, value);
        }

        public T this[Vector2I v]
        {
            get => At(v.X, v.Y);
            set => Set(v.X, v.Y, value);
        }

        public Array2D(int width, int height, T emptyElement = default(T))
        {
            int size = width * height;
            _data = new List<T>(size);
            _default = emptyElement;
            for (int i = 0; i < size; i++)
            {
                _data.Add(_default);
            }
            _width = width;
            _height = height;
        }

        // Use this for access outside of bounds of the array
        // This prevents us from having to do a bounds check in the matching 
        // algorithms
        public T CheckedAt(int x, int y)
        {
            if (x < 0 || x >= _width || y < 0 || y >= _height)
            {
                return _default;
            }
            else return _data[x + y * _width];
        }

        public T At(int x, int y)
        {
            if (x < 0 || x >= _width || y < 0 || y >= _height)
            {
                throw new System.Exception("Out of bounds with " + x.ToString() + "/" + y.ToString());
            }
            return _data[x + y * _width];
        }

        public void CheckedSet(int x, int y, T item)
        {
            if (x < 0 || x >= _width || y < 0 || y >= _height)
            {
                return;
            }
            else _data[x + y * _width] = item;
        }

        public void Set(int x, int y, T item)
        {
#if UNITY_EDITOR
            if (x < 0 || x >= _width || y < 0 || y >= _height)
            {
                throw new System.Exception("Out of bounds with " + x.ToString() + "/" + y.ToString());
            }
#endif
            _data[x + y * _width] = item;
        }

        public void Swap(int x0, int y0, int x1, int y1)
        {
            var temp = At(x0, y0);
            Set(x0, y0, At(x1, y1));
            Set(x1, y1, temp);
        }

        public void Swap(Vector2I v0, Vector2I v1)
        {
            var temp = At(v0.X, v0.Y);
            Set(v0.X, v0.Y, At(v1.X, v1.Y));
            Set(v1.X, v1.Y, temp);
        }

        public void Fill(T val)
        {
            for (int i = 0; i < _width * _height; ++i)
            {
                _data[i] = val;
            }
        }

        public void ForEach(System.Action<T> action)
        {
            _data.ForEach(action);
        }

        public Array2D<T> DeepCopy()
        {
            var result = new Array2D<T>(_width, _height, _default);
            result._data = new List<T>(_data);
            return result;
        }
    }
}
