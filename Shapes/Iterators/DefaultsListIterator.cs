using System;
using System.Collections.Generic;
using System.Linq;

namespace Shapes.Iterators
{
    public sealed class DefaultsListIterator<T> where T : IEquatable<T>
    {
        private readonly List<T> _items;
        private T _currentItem;

        public DefaultsListIterator(List<T> items)
        {
            if (items.Count == 0)
            {
                throw new ArgumentException($"List could not be empty");
            }

            _items = items;
            _currentItem = _items.First();
        }

        public T GetCurrent()
        {
            return _currentItem;
        }

        public void MoveNext()
        {
            int nextIndex = _items.IndexOf(_currentItem) + 1;

            _currentItem = nextIndex == _items.Count
                ? _items[0]
                : _items[nextIndex];
        }
    }
}
