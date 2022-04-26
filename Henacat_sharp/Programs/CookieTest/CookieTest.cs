using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Henacat_sharp.servlet.http;
using System.IO;

namespace CookieTest
{
    public class CookieTest : HttpServlet
    {
        protected override void doGet( HttpServletRequest request, HttpServletResponse response )
        {
            response.setContentType("text/plain");
            StreamWriter output = response.getWriter();
            String counterStr = null;

            Cookie[] cookies = request.getCookies();
            if ( cookies == null )
            {
                output.WriteLine("cookies == null");
            }
            else
            {
                output.WriteLine("cookies.length.." + cookies.Length);
                for ( int i = 0; i < cookies.Length; i++ )
                {
                    output.WriteLine("cookies[" + i + "].."
                        + cookies[i].getName() + "/" + cookies[i].getValue());
                    if ( cookies[i].getName().Equals("COUNTER") )
                    {
                        counterStr = cookies[i].getValue();
                    }
                }
            }
            int counter;
            if ( counterStr == null )
            {
                counter = 1;
            }
            else
            {
                counter = Int32.Parse(counterStr) + 1;
            }
            Cookie newCookie = new Cookie("COUNTER", "" + counter);
            response.addCookie(newCookie);
        }
    }
}
