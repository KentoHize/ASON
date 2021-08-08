using System;
using System.Collections;
using System.Collections.Generic;

namespace ArinaStandardObjectNotation
{
    public class ArObject : IList<object>
    {
        public object this[int index] { get => ((IList<object>)Values)[index]; set => ((IList<object>)Values)[index] = value; }

        public ArType Type { get; set; }
        
        public List<object> Values { get; set; }

        public int Count => ((ICollection<object>)Values).Count;

        public bool IsReadOnly => ((ICollection<object>)Values).IsReadOnly;

        public void Add(object item)
        {
            ((ICollection<object>)Values).Add(item);
        }

        public void Clear()
        {
            ((ICollection<object>)Values).Clear();
        }

        public bool Contains(object item)
        {
            return ((ICollection<object>)Values).Contains(item);
        }

        public void CopyTo(object[] array, int arrayIndex)
        {
            ((ICollection<object>)Values).CopyTo(array, arrayIndex);
        }

        public IEnumerator<object> GetEnumerator()
        {
            return ((IEnumerable<object>)Values).GetEnumerator();
        }

        public int IndexOf(object item)
        {
            return ((IList<object>)Values).IndexOf(item);
        }

        public void Insert(int index, object item)
        {
            ((IList<object>)Values).Insert(index, item);
        }

        public bool Remove(object item)
        {
            return ((ICollection<object>)Values).Remove(item);
        }

        public void RemoveAt(int index)
        {
            ((IList<object>)Values).RemoveAt(index);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)Values).GetEnumerator();
        }
    }
}
