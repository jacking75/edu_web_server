using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Henacat_sharp.servlet.http;
using Message;

namespace PostBBS
{
    public class PostBBS : HttpServlet
    {
        protected override void doPost( HttpServletRequest request, HttpServletResponse response )
        {
            request.setCharacterEncoding("UTF-8");
            Message.Message newMessage = new Message.Message(request.getParameter("title"),
                                             request.getParameter("handle"),
                                             request.getParameter("message"));
            newMessage.date = DateTime.Now;
            Message.Message.messageList.Insert(0, newMessage);
            response.sendRedirect("/testbbs/ShowBBS");
        }
    }
}
