
using System.IO;

namespace Xperimen.Helper
{
    public class StreamByteConverter
    {
        public byte[] GetImageBytes(Stream data)
        {
            byte[] ImageBytes;
            using (var memoryStream = new MemoryStream())
            {
                data.CopyTo(memoryStream);
                ImageBytes = memoryStream.ToArray();
            }
            return ImageBytes;
        }

        public Stream BytesToStream(byte[] bytes)
        {
            Stream stream = new MemoryStream(bytes);
            return stream;
        }
    }
}
