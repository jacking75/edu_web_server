using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Henacat_sharp.servlet.http;
using System.IO;

namespace ShowBBS
{
    public class ShowBBS : HttpServlet
    {
        // HTMLで意味を持つ文字をエスケープするユーティリティメソッド
        private String escapeHtml( String src )
        {
            return src.Replace("&", "&amp;").Replace("<", "&lt;")
                    .Replace(">", "&gt;").Replace("\"", "&quot;")
                    .Replace("'", "&#39;");
        }


        protected override void doGet( HttpServletRequest request, HttpServletResponse response )
        {
            response.setContentType("text/html; charset=UTF-8");
            StreamWriter output = response.getWriter();
            output.WriteLine("<html>");
            output.WriteLine("<head>");
            output.WriteLine("<title>テスト掲示板</title>");
            output.WriteLine("<head>");
            output.WriteLine("<body>");
            output.WriteLine("<h1>テスト掲示板</h1>");
            output.WriteLine("<form action='/testbbs/PostBBS' method='post'>");
            output.WriteLine("タイトル：<input type='text' name='title' size='60'>"
                    + "<br/>");
            output.WriteLine("ハンドル名：<input type='text' name='handle'><br/>");
            output.WriteLine("<textarea name='message' rows='4' cols='60'>"
                    + "</textarea><br/>");
            output.WriteLine("<input type='submit'/>");
            output.WriteLine("</form>");
            output.WriteLine("<hr/>");

            foreach ( Message.Message message in Message.Message.messageList )
            {
                output.WriteLine("<p>『" + escapeHtml(message.title) + "』&nbsp;&nbsp;"
                        + escapeHtml(message.handle) + " さん&nbsp;&nbsp;"
                        + escapeHtml(message.date.ToString()) + "</p>");
                output.WriteLine("<p>");
                output.WriteLine(escapeHtml(message.message).Replace("\r\n", "<br/>"));
                output.WriteLine("</p><hr/>");
            }

            output.WriteLine("</body>");
            output.WriteLine("</html>");
        }
    }
}
