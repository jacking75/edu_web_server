namespace Henacat_sharp.servlet.http
{
    public class HttpServlet
    {
        protected virtual void doGet( HttpServletRequest req, HttpServletResponse resp )
        {

        }

        protected virtual void doPost( HttpServletRequest req, HttpServletResponse resp )
        {
        }

        public void service( HttpServletRequest req,
                            HttpServletResponse resp )
        {
            if ( req.getMethod().Equals("GET") )
            {
                doGet(req, resp);
            }
            else if ( req.getMethod().Equals("POST") )
            {
                doPost(req, resp);
            }
        }
    }
}
