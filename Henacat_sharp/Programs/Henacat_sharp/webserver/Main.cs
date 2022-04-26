using System;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using Henacat_sharp.servletimpl;

namespace Henacat_sharp.webserver
{
    public class Main
    {
        public static void main()
        {
            WebApplication app = WebApplication.createInstance("testbbs");
            app.addServlet("/ShowBBS", "ShowBBS");
            app.addServlet("/PostBBS", "PostBBS");
            app = WebApplication.createInstance("cookietest");
            app.addServlet("/CookieTest", "CookieTest");
            app = WebApplication.createInstance("sessiontest");
            app.addServlet("/SessionTest", "SessionTest");
            app = WebApplication.createInstance("ajaxtest");
            app.addServlet("/GetAddress", "GetAddress");
            app = WebApplication.createInstance("uploadtest");
            app.addServlet("/UploadTest", "UploadTest");


            TcpListener listener = null;
            try
            {
                listener = new TcpListener(IPAddress.Parse("127.0.0.1"), 8001);
                listener.Start();
                while ( true )
                {
                    Socket socket = listener.AcceptSocket();
                    ServerThread serverThread = new ServerThread(socket);
                    Thread thread = new Thread(serverThread.Run);
                    thread.Start();
                }
            }
            catch ( Exception ex )
            {
                Console.Write(ex);
            }
            finally
            {
                try
                {
                    if ( listener != null )
                    {
                        listener.Stop();
                    }
                }
                catch ( Exception ex )
                {
                    Console.Write(ex);
                }
            }
        }

    }
}
