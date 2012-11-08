#region License GNU GPL
// ZipHelper.cs
// 
// Copyright (C) 2012 - BehaviorIsManaged
// 
// This program is free software; you can redistribute it and/or modify it 
// under the terms of the GNU General Public License as published by the Free Software Foundation;
// either version 2 of the License, or (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; 
// without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. 
// See the GNU General Public License for more details. 
// You should have received a copy of the GNU General Public License along with this program; 
// if not, write to the Free Software Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA 02111-1307 USA
#endregion
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