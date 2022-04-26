using System.IO;

namespace Henacat_sharp.servlet.http
{
    public abstract class HttpServletResponse
    {
        public const int SC_OK = 200;
        public const int SC_FOUND = 302;

        abstract public void setContentType( string contentType );
        abstract public void setCharacterEncoding( string charset );
        abstract public StreamWriter getWriter();
        abstract public void sendRedirect( string location );
        abstract public void setStatus( int sc );
        abstract public void addCookie( Cookie cookie );
    }
}
