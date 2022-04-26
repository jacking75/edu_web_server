using System;
using System.Collections.Generic;
using System.IO;
using Henacat_sharp.servlet.http;
using Henacat_sharp.util;
using System.Globalization;

namespace Henacat_sharp.servletimpl
{
    class ResponseHeaderGeneratorImpl : ResponseHeaderGenerator
    {
        private List<Cookie> cookies;

        private static string getCookieDateString( DateTime cal )
        {
            string timestr = cal.ToString("ddd, dd MMM yyyy HH:mm:ss", CultureInfo.CreateSpecificCulture("en-US"));
            return timestr + " GMT";
        }

        public void generate( Stream output )
        {
            foreach ( Cookie cookie in cookies )
            {
                string header;
                header = "Set-Cookie: "
                    + cookie.getName() + "=" + cookie.getValue();

                if ( cookie.getDomain() != null )
                {
                    header += "; Domain=" + cookie.getDomain();
                }
                if ( cookie.getMaxAge() > 0 )
                {
                    DateTime cal = DateTime.Now.ToUniversalTime();
                    cal.AddSeconds(cookie.getMaxAge());
                    header += "; Expires=" + getCookieDateString(cal);
                }
                else if ( cookie.getMaxAge() == 0 )
                {
                    DateTime cal = new DateTime(1970, 1, 1, 0, 0, 10).ToUniversalTime();
                    header += "; Expires=" + getCookieDateString(cal);
                }
                if ( cookie.getPath() != null )
                {
                    header += "; Path=" + cookie.getPath();
                }
                if ( cookie.getSecure() )
                {
                    header += "; Secure";
                }
                if ( cookie.isHttpOnly() )
                {
                    header += "; HttpOnly";
                }
                Util.writeLine(output, header);
            }
        }

        public ResponseHeaderGeneratorImpl( List<Cookie> cookies )
        {
            this.cookies = cookies;
        }
    }
}
