using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vibely_App.Business.Helpers
{
    public class MemoryStreamFileAbstraction : TagLib.File.IFileAbstraction
    {
        public string Name { get; }
        public Stream ReadStream { get; }
        public Stream WriteStream => throw new NotSupportedException(); // We only need to read

        public MemoryStreamFileAbstraction(string name, Stream stream)
        {
            Name = name;
            ReadStream = stream;
        }

        public void CloseStream(Stream stream)
        {
            stream?.Dispose(); // Dispose the stream when TagLib# is done
        }
    }

}
