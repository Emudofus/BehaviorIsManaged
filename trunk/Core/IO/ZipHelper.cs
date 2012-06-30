using System.IO;
using System.IO.Compression;
using zlib;

namespace BiM.Core.IO
{
    public class ZipHelper
    {
        public static byte[] Compress(byte[] data)
        {
            var input = new MemoryStream(data);
            var output = new MemoryStream();

            Compress(input, output);

            return output.ToArray();
        }

        public static void Compress(Stream input, Stream output)
        {
            using (var compressStream = new GZipStream(output, CompressionMode.Compress))
            {
                input.CopyTo(compressStream);
            }
        }

        public static byte[] Uncompress(byte[] data)
        {
            var input = new MemoryStream(data);
            var output = new MemoryStream();

            Uncompress(input, output);

            return output.ToArray();
        }

        public static void Uncompress(Stream input, Stream output)
        {
            using (var zinput = new GZipStream(input, CompressionMode.Decompress, true))
            {
                zinput.CopyTo(output);
            }
        }

        public static void Deflate(Stream input, Stream output)
        {
            var zoutput = new ZOutputStream(output);
            var inputReader = new BinaryReader(input);

            zoutput.Write(inputReader.ReadBytes((int)input.Length), 0, (int)input.Length);
            zoutput.Flush();
        }
    }
}