namespace Helper;
using System.Text;

public class MurmurHash<T> : IMurmurHash{
    private readonly int _bitArraySize;

    public MurmurHash(int bitArraySize) {
        _bitArraySize = bitArraySize;
    }

    public int[] ComputeHashes(string input) {
        var data = Encoding.UTF8.GetBytes(input);
        byte[] baseHash = MurmurHash3_x64_128(data, 0);
        int[] hashes = new int[4];
        for (int i = 0; i < 4; i++) {
            hashes[i] = Math.Abs(BitConverter.ToInt32(baseHash, i * 4) % _bitArraySize);
        }
        return hashes;
    }

  
    private static byte[] MurmurHash3_x64_128(byte[] data, ulong seed)
    {
        const ulong c1 = 0x87c37b91114253d5UL;
        const ulong c2 = 0x4cf5ad432745937fUL;
        int length = data.Length;
        int nblocks = length / 16;
        ulong h1 = seed;
        ulong h2 = seed;
        // Body
        for (int i = 0; i < nblocks; i++)
        {
            int i16 = i * 16;
            ulong k1 = BitConverter.ToUInt64(data, i16);
            ulong k2 = BitConverter.ToUInt64(data, i16 + 8);
            k1 *= c1; k1 = Rotl64(k1, 31); k1 *= c2; h1 ^= k1;
            h1 = Rotl64(h1, 27); h1 += h2; h1 = h1 * 5 + 0x52dce729;
            k2 *= c2; k2 = Rotl64(k2, 33); k2 *= c1; h2 ^= k2;
            h2 = Rotl64(h2, 31); h2 += h1; h2 = h2 * 5 + 0x38495ab5;
        }
        // Tail
        ulong k1_tail = 0;
        ulong k2_tail = 0;
        int tailStart = nblocks * 16;
        int tailLen = length & 15;
        if (tailLen > 0)
        {
            for (int i = 0; i < tailLen; i++)
            {
                if (i < 8)
                    k1_tail |= ((ulong)data[tailStart + i]) << (8 * i);
                else
                    k2_tail |= ((ulong)data[tailStart + i]) << (8 * (i - 8));
            }
            if (k1_tail != 0)
            {
                k1_tail *= c1; k1_tail = Rotl64(k1_tail, 31); k1_tail *= c2; h1 ^= k1_tail;
            }
            if (k2_tail != 0)
            {
                k2_tail *= c2; k2_tail = Rotl64(k2_tail, 33); k2_tail *= c1; h2 ^= k2_tail;
            }
        }
        // Finalization
        h1 ^= (ulong)length;
        h2 ^= (ulong)length;
        h1 += h2;
        h2 += h1;
        h1 = FMix(h1);
        h2 = FMix(h2);
        h1 += h2;
        h2 += h1;
        byte[] hash = new byte[16];
        Array.Copy(BitConverter.GetBytes(h1), 0, hash, 0, 8);
        Array.Copy(BitConverter.GetBytes(h2), 0, hash, 8, 8);
        return hash;
    }
    private static ulong Rotl64(ulong x, byte r) => (x << r) | (x >> (64 - r));
    private static ulong FMix(ulong k)
    {
        k ^= k >> 33;
        k *= 0xff51afd7ed558ccdUL;
        k ^= k >> 33;
        k *= 0xc4ceb9fe1a85ec53UL;
        k ^= k >> 33;
        return k;
    }

}