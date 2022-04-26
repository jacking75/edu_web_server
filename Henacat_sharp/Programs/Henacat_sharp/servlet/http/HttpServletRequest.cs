using System.Collections.Generic;

namespace Henacat_sharp.servlet.http
{
    public interface HttpServletRequest
    {
        string getMethod();
        string getParameter( string name );
        string[] getParameterValues( string name );
        void setCharacterEncoding( string env );
        Cookie[] getCookies();
        HttpSession getSession();
        HttpSession getSession( bool create );
        Part getPart( string name );
        ICollection<Part> getParts();
    }
}
