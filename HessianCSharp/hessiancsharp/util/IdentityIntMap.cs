using System;
using System.Collections.Generic;
using System.Text;

namespace hessiancsharp.util
{
    /// <summary>
    /// The IntMap provides a simple hashmap from keys to integers.  The API is
    /// an abbreviation of the HashMap collection API.
    /// <para />The convenience of IntMap is avoiding all the silly wrapping of integers.
    /// </summary>
    public class IdentityIntMap
    {
        //Encoding of a null entry.  Since NULL is equal to Integer.MIN_VALUE,it's impossible to distinguish between the two.
        public const int NULL = unchecked((int)0xdeadbeef); // Integer.MIN_VALUE + 1;

        private object[] _keys;
        private int[] _values;

        private int _size;
        private int _prime;

        /// <summary>
        /// Create a new IntMap.  Default size is 16.
        /// </summary>
        public IdentityIntMap(int capacity)
        {
            _keys = new object[capacity];
            _values = new int[capacity];

            _prime = GetBiggestPrime(_keys.Length);
            _size = 0;
        }

        /// <summary>
        /// Clear the hashmap.
        /// </summary>
        public void Clear()
        {
            Array.Clear(_keys, 0, _keys.Length);
            Array.Clear(_values, 0, _values.Length);
            _size = 0;
        }

        /// <summary>
        /// Returns the current number of entries in the map.
        /// </summary>
        public int Size()
        {
            return _size;
        }

        /// <summary>
        /// Puts a new value in the property table with the appropriate flags
        /// </summary>
        public int Get(object key)
        {
            int prime = _prime;
            int hash = HashCode(key) % prime;
            // int hash = key.hashCode() & mask;

            Object[] keys = _keys;

            while (true)
            {
                Object mapKey = keys[hash];

                if (mapKey == null)
                    return NULL;
                else if (mapKey == key)
                    return _values[hash];

                hash = (hash + 1) % prime;
            }
        }

        /// <summary>
        /// Puts a new value in the property table with the appropriate flags
        /// </summary>
        public int Put(Object key, int value, bool isReplace)
        {
            int prime = _prime;
            int hash = Math.Abs(HashCode(key) % prime);
            // int hash = key.hashCode() % prime;

            Object[] keys = _keys;

            while (true)
            {
                Object testKey = keys[hash];

                if (testKey == null)
                {
                    keys[hash] = key;
                    _values[hash] = value;

                    _size++;

                    if (keys.Length <= 4 * _size)
                        Resize(4 * keys.Length);

                    return value;
                }
                else if (key != testKey)
                {
                    hash = (hash + 1) % prime;

                    continue;
                }
                else if (isReplace)
                {
                    int old = _values[hash];

                    _values[hash] = value;

                    return old;
                }
                else
                {
                    return _values[hash];
                }
            }
        }

        /// <summary>
        /// Removes a value in the property table.
        /// </summary>
        public void Remove(Object key)
        {
            if (Put(key, NULL, true) != NULL)
            {
                _size--;
            }
        }

        /// <summary>
        /// Expands the property table
        /// </summary>
        private void Resize(int newSize)
        {
            Object[] keys = _keys;
            int[] values = _values;

            _keys = new Object[newSize];
            _values = new int[newSize];
            _size = 0;

            _prime = GetBiggestPrime(_keys.Length);

            for (int i = keys.Length - 1; i >= 0; i--)
            {
                Object key = keys[i];
                int value = values[i];

                if (key != null && value != NULL)
                {
                    Put(key, value, true);
                }
            }
        }

        protected int HashCode(object value)
        {
            return value.GetHashCode();
        }

        public override string ToString()
        {
            var sbuf = new StringBuilder();

            sbuf.Append("IntMap[");
            bool isFirst = true;

            for (int i = 0; i <= _keys.Length; i++)
            {
                if (_keys[i] != null)
                {
                    if (!isFirst)
                        sbuf.Append(", ");

                    isFirst = false;
                    sbuf.Append(_keys[i]);
                    sbuf.Append(":");
                    sbuf.Append(_values[i]);
                }
            }
            sbuf.Append("]");

            return sbuf.ToString();
        }

        public static readonly int[] PRIMES =
        {
            1,       /* 1<< 0 = 1 */
            2,       /* 1<< 1 = 2 */
            3,       /* 1<< 2 = 4 */
            7,       /* 1<< 3 = 8 */
            13,      /* 1<< 4 = 16 */
            31,      /* 1<< 5 = 32 */
            61,      /* 1<< 6 = 64 */
            127,     /* 1<< 7 = 128 */
            251,     /* 1<< 8 = 256 */
            509,     /* 1<< 9 = 512 */
            1021,    /* 1<<10 = 1024 */
            2039,    /* 1<<11 = 2048 */
            4093,    /* 1<<12 = 4096 */
            8191,    /* 1<<13 = 8192 */
            16381,   /* 1<<14 = 16384 */
            32749,   /* 1<<15 = 32768 */
            65521,   /* 1<<16 = 65536 */
            131071,  /* 1<<17 = 131072 */
            262139,  /* 1<<18 = 262144 */
            524287,  /* 1<<19 = 524288 */
            1048573, /* 1<<20 = 1048576 */
            2097143, /* 1<<21 = 2097152 */
            4194301, /* 1<<22 = 4194304 */
            8388593, /* 1<<23 = 8388608 */
            16777213, /* 1<<24 = 16777216 */
            33554393, /* 1<<25 = 33554432 */
            67108859, /* 1<<26 = 67108864 */
            134217689, /* 1<<27 = 134217728 */
            268435399, /* 1<<28 = 268435456 */
        };

        public static int GetBiggestPrime(int value)
        {
            for (int i = PRIMES.Length - 1; i >= 0; i--)
            {
                if (PRIMES[i] <= value)
                    return PRIMES[i];
            }

            return 2;
        }
    }
}
