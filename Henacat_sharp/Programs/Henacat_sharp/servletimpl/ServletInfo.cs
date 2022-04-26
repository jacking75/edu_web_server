using Henacat_sharp.servlet.http;

namespace Henacat_sharp.servletimpl
{
    class ServletInfo
    {
        public WebApplication webApp;
        public string urlPattern;
        public string servletClassName;
        public HttpServlet servlet;

        public ServletInfo( WebApplication webApp, string urlPattern,
                           string servletClassName )
        {
            this.webApp = webApp;
            this.urlPattern = urlPattern;
            this.servletClassName = servletClassName;
        }
    }
}
