using System.Linq;
using System.IO;
using ICSharpCode.SharpZipLib.Zip.Compression.Streams;

namespace Compression
{
    class ZipLib
    {
        public static byte[] Compress(byte[] data)
        {
            byte[] result = null;

            using (MemoryStream ms = new MemoryStream())
                using (Stream s = new DeflaterOutputStream(ms))
                {
                    s.Write(data, 0, data.Length);
                    s.Close();
                    result = ms.ToArray();
                }

            return result;
        }

        public static byte[] Decompress(byte[] data)
        {
            byte[] r = null;

            using (Stream s = new InflaterInputStream(new MemoryStream(data)))
            {
                byte[] b = new byte[8192];
                int count = s.Read(b, 0, b.Length);
                r = b.Take(count).ToArray();
            }

            return r;
        }

        public static byte[] Decompress(byte[] data, int final_size)
        {
            byte[] r = null;

            using (Stream s = new InflaterInputStream(new MemoryStream(data)))
                s.Read(r, 0, final_size);

            return r;
        }
    }
}
