using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Alaska.Foundation.Core.Collections
{
    public class Buffer<T>
    {
        private int? _defaultSize = null;
        private List<T> _innerList = new List<T>();

        public Buffer()
        { }

        public Buffer(int size)
        {
            _defaultSize = size;
        }

        public void AddRange(IEnumerable<T> elements)
        {
            lock (this)
            {
                _innerList.AddRange(elements);
            }
        }

        public void Add(T element)
        {
            lock (this)
            {
                _innerList.Add(element);
            }
        }

        public IEnumerable<T> DequeueIfFull()
        {
            if (!_defaultSize.HasValue)
                throw new InvalidOperationException("No default buffer size specified");

            return DequeueIfFull(_defaultSize.Value);
        }

        public IEnumerable<T> DequeueIfFull(int size)
        {
            lock (this)
            {
                if (_innerList.Count < size)
                    return new List<T>();

                var elements = _innerList.ToList();
                _innerList.Clear();
                return elements;
            }
        }

        public IEnumerable<T> DequeueAll()
        {
            lock (this)
            {
                var elements = _innerList.ToList();
                _innerList.Clear();
                return elements;
            }
        }
    }
}
