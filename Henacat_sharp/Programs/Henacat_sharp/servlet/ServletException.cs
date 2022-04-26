using System;

namespace Henacat_sharp.servlet
{
    class ServletException : Exception
    {
        public ServletException( string message ) : base(message) { }
    }
}
