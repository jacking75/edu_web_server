using System.Collections.Generic;

namespace Henacat_sharp.servlet.http
{
    public interface HttpSession
    {
        string getId();
        object getAttribute( string name );
        IEnumerable<string> getAttributeNames();
        void removeAttribute( string name );
        void setAttribute( string name, object value );
    }
}
