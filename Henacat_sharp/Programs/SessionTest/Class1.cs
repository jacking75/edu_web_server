using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Henacat_sharp.servlet.http;
using System.IO;

namespace SessionTest
{
    public class SessionTest : HttpServlet
    {
        protected override void doGet( HttpServletRequest request, HttpServletResponse response )
        {
            response.setContentType("text/plain");
            StreamWriter output = response.getWriter();

            HttpSession session = request.getSession(true);
            int? counter = (int?) session.getAttribute("Counter");
            if ( counter == null )
            {
                output.WriteLine("No session");
                session.setAttribute("Counter", 1);
            }
            else
            {
                output.WriteLine("Counter.." + counter);
                session.setAttribute("Counter", counter + 1);
            }
        }
    }
}
