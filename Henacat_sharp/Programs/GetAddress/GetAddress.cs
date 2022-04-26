using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Henacat_sharp.servlet.http;
using System.IO;

namespace GetAddress
{
    public class GetAddress : HttpServlet
    {
        protected override void doGet( HttpServletRequest request, HttpServletResponse response )
        {
            response.setContentType("text/plain; charset=UTF-8");
            StreamWriter output = response.getWriter();

            String postalCode = request.getParameter("postalCode");
            String ret;
            if ( postalCode.Equals("162-0846") )
            {
                ret = "東京都新宿区市谷左内町";
            }
            else if ( postalCode.Equals("100-0014") )
            {
                ret = "東京都千代田区永田町";
            }
            else
            {
                ret = "不明";
            }
            response.setStatus(HttpServletResponse.SC_OK);
            output.WriteLine(ret);
        }
    }
}
