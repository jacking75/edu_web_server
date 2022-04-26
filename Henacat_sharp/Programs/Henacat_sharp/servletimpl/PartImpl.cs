using System.Collections.Generic;
using System.Linq;
using System.IO;
using Henacat_sharp.servlet.http;
using Henacat_sharp.util;

namespace Henacat_sharp.servletimpl
{
    class PartImpl : Part
    {
        private string contentType;
        private Map<string, string> headerMap;
        private byte[] data;
        private string name;

        public string getContentType()
        {
            return contentType;
        }

        public string getHeader( string name )
        {
            return headerMap.get(name);
        }

        public ICollection<string> getHeaderNames()
        {
            return headerMap.Keys.ToList();
        }

        public Stream getInputStream()
        {
            return new MemoryStream(data);
        }

        public string getName()
        {
            return name;
        }

        public long getSize()
        {
            return data.Length;
        }

        public void write( string fileName )
        {
            using ( StreamWriter fos = new StreamWriter(fileName) )
            {
                fos.Write(data);
            }
        }


        public PartImpl( string contentType, Map<string, string> headerMap,
                 byte[] data, string name )
        {
            this.contentType = contentType;
            this.headerMap = headerMap;
            this.data = data;
            this.name = name;
        }
    }
}
