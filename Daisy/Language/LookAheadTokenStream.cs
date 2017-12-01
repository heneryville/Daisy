namespace Ancestry.Daisy.Language
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Collections;

    public class LookAheadStream<T> : IEnumerable<T> , IEnumerator<T>
    {
        private LinkedList<T> stored = new LinkedList<T>(); //The first value of the array is the current
        private IEnumerator<T> enumerator;

        private bool hasCurrent = false;
        private T current;

        public LookAheadStream(IEnumerable<T> stream)
        {
            enumerator = stream.GetEnumerator();
        }

        public struct Peek
        {
            public bool HasCharactersUpTo { get; set; }

            public T Value { get; set; }
        }

        public Peek LookAhead(int lookTo)
        {
            if (lookTo < 0) throw new ArgumentOutOfRangeException("lookTo");
            if (lookTo == 0) return new Peek() { HasCharactersUpTo = hasCurrent, Value = current };
            if(stored.Count >= lookTo)
            {
                return new Peek(){
                    HasCharactersUpTo = true, 
                    Value  = stored.ElementAt(lookTo-1)
                };
            }
            var needToFastForward = lookTo - stored.Count;
            for(;needToFastForward > 0; --needToFastForward)
            {
                if(!enumerator.MoveNext())
                {
                    return new Peek() {
                            HasCharactersUpTo = false,
                            Value = default(T)
                        };
                }
                stored.AddLast(enumerator.Current);
            }
            return new Peek() {
                    HasCharactersUpTo = true,
                    Value = stored.LastOrDefault()
                };
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.Collections.Generic.IEnumerator`1"/> that can be used to iterate through the collection.
        /// </returns>
        /// <filterpriority>1</filterpriority>
        public IEnumerator<T> GetEnumerator()
        {
            return this;
        }

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Collections.IEnumerator"/> object that can be used to iterate through the collection.
        /// </returns>
        /// <filterpriority>2</filterpriority>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// <filterpriority>2</filterpriority>
        public void Dispose()
        {
            enumerator.Dispose();
        }

        /// <summary>
        /// Advances the enumerator to the next element of the collection.
        /// </summary>
        /// <returns>
        /// true if the enumerator was successfully advanced to the next element; false if the enumerator has passed the end of the collection.
        /// </returns>
        /// <exception cref="T:System.InvalidOperationException">The collection was modified after the enumerator was created. </exception><filterpriority>2</filterpriority>
        public bool MoveNext()
        {
            if(stored.Any())
            {
                current = stored.First.Value;
                stored.RemoveFirst();
                return true;
            }
            else
            {
                var hasNext = enumerator.MoveNext();
                if (hasNext) current = enumerator.Current;
                return hasNext;
            }

        }

        /// <summary>
        /// Sets the enumerator to its initial position, which is before the first element in the collection.
        /// </summary>
        /// <exception cref="T:System.InvalidOperationException">The collection was modified after the enumerator was created. </exception><filterpriority>2</filterpriority>
        public void Reset()
        {
            stored.Clear();
            enumerator.Reset();
        }

        /// <summary>
        /// Gets the element in the collection at the current position of the enumerator.
        /// </summary>
        /// <returns>
        /// The element in the collection at the current position of the enumerator.
        /// </returns>
        public T Current
        {
            get
            {
                return current;
            }
        }

        /// <summary>
        /// Gets the current element in the collection.
        /// </summary>
        /// <returns>
        /// The current element in the collection.
        /// </returns>
        /// <filterpriority>2</filterpriority>
        object IEnumerator.Current
        {
            get
            {
                return Current;
            }
        }

    }
}
