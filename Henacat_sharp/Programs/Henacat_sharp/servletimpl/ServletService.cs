using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Henacat_sharp.servlet.http;
using Henacat_sharp.util;


namespace Henacat_sharp.servletimpl
{
    class ServletService
    {
        private static HttpServlet createServlet( ServletInfo info )
        {
            Type clazz
                = info.webApp.classLoader.loadClass(info.servletClassName);
            return (HttpServlet) Activator.CreateInstance(clazz);
        }

        private static Map<string, string[]> stringToMap( string str )
        {
            Map<string, string[]> parameterMap = new HashMap<string, string[]>();
            if ( !string.IsNullOrEmpty(str) )
            {
                string[] paramArray = str.Split('&');
                foreach ( string param in paramArray )
                {
                    string[] keyValue = param.Split('=');
                    if ( parameterMap.ContainsKey(keyValue[0]) )
                    {
                        string[] array = parameterMap[keyValue[0]];
                        string[] newArray = new string[array.Length + 1];
                        Array.Copy(array, 0, newArray, 0, array.Length);
                        newArray[array.Length] = keyValue[1];
                        parameterMap.Add(keyValue[0], newArray);
                    }
                    else
                    {
                        parameterMap.Add(keyValue[0], new string[] { keyValue[1] });
                    }
                }
            }
            return parameterMap;
        }

        private static string readToSize( Stream input, int size )
        {
            int ch;
            StringBuilder sb = new StringBuilder();
            int readSize = 0;

            while ( readSize < size && (ch = input.ReadByte()) != -1 )
            {
                sb.Append((char) ch);
                readSize++;
            }
            return sb.ToString();
        }

        public static void doService( string method, string query, ServletInfo info,
                                 Map<string, string> requestHeader,
                                 Stream input, Stream output )
        {
            if ( info.servlet == null )
            {
                info.servlet = createServlet(info);
            }

            MemoryStream outputBuffer = new MemoryStream();
            HttpServletResponseImpl resp
                    = new HttpServletResponseImpl(outputBuffer);

            HttpServletRequest req;

            if ( method == "GET" )
            {
                Map<string, string[]> map;
                map = stringToMap(query);
                req = new HttpServletRequestImpl("GET", requestHeader, map, resp, info.webApp);
            }
            else if ( method == "POST" )
            {
                string contentType = requestHeader.get("CONTENT-TYPE");
                int contentLength
                    = Int32.Parse(requestHeader.get("CONTENT-LENGTH"));
                if ( contentType.ToUpper().StartsWith("MULTIPART/FORM-DATA") )
                {
                    req = MultiPartParser.parse(requestHeader, input,
                                            contentType, contentLength,
                                            resp, info.webApp);
                }
                else
                {
                    Map<string, string[]> map;
                    string line = readToSize(input, contentLength);
                    map = stringToMap(line);
                    req = new HttpServletRequestImpl("POST", requestHeader, map,
                                                     resp, info.webApp);
                }
            }
            else
            {
                throw new Exception("BAD METHOD:" + method);
            }

            info.servlet.service(req, resp);

            if ( resp.status == HttpServletResponse.SC_OK )
            {
                ResponseHeaderGenerator hg
                    = new ResponseHeaderGeneratorImpl(resp.cookies);
                SendResponse.sendOkResponseHeader(output, resp.contentType, hg);

                if ( resp.printWriter != null )
                {
                    resp.printWriter.Flush();
                }
                byte[] outputBytes = outputBuffer.ToArray();
                foreach ( byte b in outputBytes )
                {
                    output.WriteByte(b);
                }
            }
            else if ( resp.status == HttpServletResponse.SC_FOUND )
            {
                string redirectLocation;
                if ( resp.redirectLocation.StartsWith("/") )
                {
                    string host = requestHeader.get("HOST");
                    redirectLocation = "http://"
                                                + ((host != null) ? host : Constants.SERVER_NAME)
                                                + resp.redirectLocation;
                }
                else
                {
                    redirectLocation = resp.redirectLocation;
                }
                SendResponse.sendFoundResponse(output, redirectLocation);
            }
        }
    }
}
