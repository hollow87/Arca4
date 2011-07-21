/*
 * 
 * A simple wrapper to allow .net interface with the popular
 * compression utility zlib1.dll.
 * 
 * Coded by oobe.
 * 
*/

using System;
using System.Linq;
using System.Runtime.InteropServices;

namespace Compression
{
    class ZipLib
    {
        [DllImport("zlib1.dll", CallingConvention = CallingConvention.Cdecl)]
        static extern int compress(byte[] destBuffer, ref uint destLen, byte[] sourceBuffer, uint sourceLen);

        [DllImport("zlib1.dll", CallingConvention = CallingConvention.Cdecl)]
        static extern int uncompress(byte[] destBuffer, ref uint destLen, byte[] sourceBuffer, uint sourceLen);

        public static byte[] Compress(byte[] data)
        {
            uint _dLen = (uint)Math.Round((double)(data.Length * 1.1) + 12);
            byte[] _d = new byte[_dLen];

            if (compress(_d, ref _dLen, data, (uint)data.Length) != 0)
                return null;

            return _d.Take((int)_dLen).ToArray();
        }
        public static byte[] Decompress(byte[] data)
        {
            uint _dLen = 8192;
            byte[] _d = new byte[_dLen];

            if (uncompress(_d, ref _dLen, data, (uint)data.Length) != 0)
                return null;

            return _d.Take((int)_dLen).ToArray();
        }

        public static byte[] Decompress(byte[] data, uint final_size)
        {
            uint _dLen = final_size;
            byte[] _d = new byte[_dLen];

            if (uncompress(_d, ref _dLen, data, (uint)data.Length) != 0)
                return null;

            return _d;
        }
    }
}
