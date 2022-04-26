using System.Collections.Generic;
using Henacat_sharp.util;

namespace Henacat_sharp.servletimpl
{
    class WebApplication
    {
        private static string WEBAPPS_DIR = System.IO.Directory.GetCurrentDirectory() + "\\" + "webapps";
        private static Map<string, WebApplication> webAppCollection = new HashMap<string, WebApplication>();
        public string directory;
        public ClassLoader classLoader;
        private Map<string, ServletInfo> servletCollection = new HashMap<string, ServletInfo>();
        private SessionManager sessionManager;


        private WebApplication( string dir )
        {
            this.directory = dir;
            this.classLoader = new ClassLoader(WEBAPPS_DIR + "\\" + dir);
        }

        public static WebApplication createInstance( string dir )
        {
            WebApplication newApp = new WebApplication(dir);
            webAppCollection.Add(dir, newApp);

            return newApp;
        }

        public void addServlet( string urlPattern, string servletClassName )
        {
            this.servletCollection.Add(urlPattern,
                                       new ServletInfo(this, urlPattern,
                                                       servletClassName));
        }

        public ServletInfo searchServlet( string path )
        {
            return servletCollection.get(path);
        }

        public static WebApplication searchWebApplication( string dir )
        {
            return webAppCollection.get(dir);
        }

        public SessionManager getSessionManager()
        {
            if ( this.sessionManager == null )
            {
                this.sessionManager = new SessionManager();
            }
            return this.sessionManager;
        }
    }
}
