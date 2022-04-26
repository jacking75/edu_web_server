using System.Collections.Generic;
using System.IO;

namespace Henacat_sharp.servlet.http
{
    public interface Part
    {
        string getContentType();
        string getHeader( string name );
        ICollection<string> getHeaderNames();
        Stream getInputStream();
        string getName();
        long getSize();
        void write( string fileName );
    }
}
