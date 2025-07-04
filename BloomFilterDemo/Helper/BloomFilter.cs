using System.Collections;

namespace Helper
{
  

    public class BloomFilter
    {
        private readonly int _bitArraySize;
        private readonly BitArray _bitArray;
        private readonly IMurmurHash _hash;

        public BloomFilter(int bitArraySize, IMurmurHash hash)
        {
            _bitArraySize = bitArraySize;
            _bitArray = new BitArray(bitArraySize);
            _hash = hash;
        }

        public void Add(string item)
        {
            var hashes = _hash.ComputeHashes(item);
            foreach (var idx in hashes)
            {
                _bitArray.Set(idx, true);
            }
        }

        public bool MightContain(string item)
        {
            var hashes = _hash.ComputeHashes(item);
            foreach (var idx in hashes)
            {
                if (!_bitArray[idx])
                    return false;
            }
            return true;
        }

        public BitArray GetBitArray() => _bitArray;
    }
} 