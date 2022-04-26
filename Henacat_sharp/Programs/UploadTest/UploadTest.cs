using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Henacat_sharp.servlet.http;
using System.IO;

namespace UploadTest
{
    public class UploadTest : HttpServlet
    {
        protected override void doPost( HttpServletRequest request, HttpServletResponse response )
        {
            request.setCharacterEncoding("Shift_JIS");
            response.setContentType("text/plain; charset=Shift_JIS");
            StreamWriter output = response.getWriter();

            foreach ( Part part in request.getParts() )
            {
                output.WriteLine("name.." + part.getName());
                foreach ( String headerName in part.getHeaderNames() )
                {
                    output.WriteLine(headerName + "=" + part.getHeader(headerName));
                }
                output.WriteLine("Content-Type.." + part.getContentType());
                output.WriteLine("Name.." + part.getName() + "/size.." + part.getSize());
                StreamReader reader = new StreamReader(part.getInputStream(), Encoding.GetEncoding("Shift_JIS"));
                int ch;
                while ( (ch = reader.Read()) >= 0 )
                {
                    output.Write((char) (ch & 0xffff));
                }
                reader.Close();
                output.WriteLine("\n==================================");
            }
            output.WriteLine("text_name=" + request.getParameter("text_name"));
        }
    }
}
