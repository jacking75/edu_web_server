using System.Collections.Generic;
using System.Text;
using Henacat_sharp.servlet.http;
using Henacat_sharp.util;

namespace Henacat_sharp.servletimpl
{
    class HttpServletRequestImpl : HttpServletRequest
    {
        private string method;
        private string characterEncoding = "iso-8859-1";
        // リクエストがmultipart/form-dataの時はbyteParameterMapを、
        // それ以外の場合はparameterMapを使用する。使わない方はnull。
        private Map<string, string[]> parameterMap;
        private Map<string, byte[][]> byteParameterMap;
        private Cookie[] cookies;
        private HttpSessionImpl session;
        private List<Part> partList;
        private HttpServletResponseImpl response;
        private WebApplication webApp;
        private const string SESSION_COOKIE_ID = "JSESSIONID";

        public string getMethod()
        {
            return this.method;
        }


        public string getParameter( string name )
        {
            string[] values = getParameterValues(name);
            if ( values == null )
            {
                return null;
            }
            return values[0];
        }

        public string[] getParameterValues( string name )
        {
            string[] decoded;
            if ( this.parameterMap != null )
            {
                string[] values = this.parameterMap.get(name);
                if ( values == null )
                {
                    return null;
                }
                decoded = new string[values.Length];

                for ( int i = 0; i < values.Length; i++ )
                {
                    decoded[i] = MyURLDecoder.decode(values[i],
                                            this.characterEncoding);
                }
            }
            else
            {
                byte[][] data = this.byteParameterMap.get(name);
                if ( data == null )
                {
                    return null;
                }
                decoded = new string[data.Length];

                for ( int i = 0; i < data.Length; i++ )
                {

                    decoded[i] = Encoding.GetEncoding(this.characterEncoding).GetString(data[0]);
                }

            }
            return decoded;
        }

        public void setCharacterEncoding( string env )
        {
            Encoding.GetEncoding(env);
            this.characterEncoding = env;
        }

        public Cookie[] getCookies()
        {
            return this.cookies;
        }

        private static Cookie[] parseCookies( string cookieString )
        {
            if ( cookieString == null )
            {
                return null;
            }
            string[] cookiePairArray = cookieString.Split(';');
            Cookie[] ret = new Cookie[cookiePairArray.Length];
            int cookieCount = 0;

            foreach ( string cookiePair in cookiePairArray )
            {
                string[] pair = cookiePair.Split(new char[] { '=' }, 2);

                ret[cookieCount] = new Cookie(pair[0], pair[1]);
                cookieCount++;
            }

            return ret;
        }

        public HttpSession getSession()
        {
            return getSession(true);
        }

        public HttpSession getSession( bool create )
        {
            if ( !create )
            {
                return this.session;
            }
            if ( this.session == null )
            {
                SessionManager manager = this.webApp.getSessionManager();
                this.session = manager.createSession();
                addSessionCookie();
            }
            return this.session;
        }

        private HttpSessionImpl getSessionInternal()
        {
            if ( this.cookies == null )
            {
                return null;
            }
            Cookie cookie = null;
            foreach ( Cookie tempCookie in this.cookies )
            {
                if ( tempCookie.getName().Equals(SESSION_COOKIE_ID) )
                {
                    cookie = tempCookie;
                }
            }

            SessionManager manager = this.webApp.getSessionManager();
            HttpSessionImpl ret = null;
            if ( cookie != null )
            {
                ret = manager.getSession(cookie.getValue());
            }
            return ret;
        }

        private void addSessionCookie()
        {
            Cookie cookie = new Cookie(SESSION_COOKIE_ID,
                                       this.session.getId());
            cookie.setPath("/" + webApp.directory + "/");
            cookie.setHttpOnly(true);
            this.response.addCookie(cookie);
        }

        public Part getPart( string name )
        {
            foreach ( Part part in this.partList )
            {
                if ( part.getName().Equals(name) )
                {
                    return part;
                }
            }
            return null;
        }

        public ICollection<Part> getParts()
        {
            return this.partList;
        }

        private HttpServletRequestImpl( string method, Map<string, string> requestHeader,
                       HttpServletResponseImpl resp,
                       WebApplication webApp )
        {
            this.method = method;
            this.cookies = parseCookies(requestHeader.get("COOKIE"));
            this.response = resp;
            this.webApp = webApp;
            this.session = getSessionInternal();
            if ( this.session != null )
            {
                addSessionCookie();
            }
        }

        public HttpServletRequestImpl( string method, Map<string, string> requestHeader,
                               Map<string, string[]> parameterMap,
                               HttpServletResponseImpl resp,
                               WebApplication webApp ) : this(method, requestHeader, resp, webApp)
        {
            this.parameterMap = parameterMap;
        }

        public HttpServletRequestImpl( Map<string, string> requestHeader,
                       Map<string, byte[][]> byteParameterMap,
                       List<Part> partList,
                       HttpServletResponseImpl resp,
                       WebApplication webApp ) : this("POST", requestHeader, resp, webApp)
        {
            this.partList = partList;
            this.byteParameterMap = byteParameterMap;
        }
    }
}
