using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Henacat_sharp.servlet.http;

namespace Henacat_sharp.servletimpl
{
    class HttpServletResponseImpl : HttpServletResponse
    {
        public string contentType = "application/octet-stream";
        private string characterEncoding = "iso-8859-1";
        private Stream outputStream;
        public StreamWriter printWriter;
        public int status;
        public string redirectLocation;
        public List<Cookie> cookies = new List<Cookie>();

        public override void setContentType( string contentType )
        {
            string[] delimiter = { "; " };
            this.contentType = contentType;
            string[] temp = contentType.Split(delimiter, StringSplitOptions.None);
            if ( temp.Length > 1 )
            {
                string[] keyValue = temp[1].Split('=');
                if ( keyValue.Length == 2 && keyValue[0].Equals("charset") )
                {
                    setCharacterEncoding(keyValue[1]);
                }
            }
        }


        public override void setCharacterEncoding( string charset )
        {
            this.characterEncoding = charset;
        }


        public override StreamWriter getWriter()
        {
            this.printWriter = new StreamWriter(outputStream, Encoding.GetEncoding(this.characterEncoding));
            return this.printWriter;
        }


        public override void sendRedirect( string location )
        {
            this.redirectLocation = location;
            setStatus(SC_FOUND);
        }


        public override void setStatus( int sc )
        {
            this.status = sc;
        }


        public override void addCookie( Cookie cookie )
        {
            this.cookies.Add(cookie);
        }

        public HttpServletResponseImpl( Stream output )
        {
            this.outputStream = output;
            this.status = SC_OK;
        }
    }
}
