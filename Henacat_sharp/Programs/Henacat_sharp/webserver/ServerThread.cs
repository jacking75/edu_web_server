using System;
using System.Collections.Generic;
using System.Net.Sockets;
using Henacat_sharp.util;
using Henacat_sharp.servletimpl;
using System.IO;

namespace Henacat_sharp.webserver
{
    class ServerThread
    {
        private const string DOCUMENT_ROOT = "htdocs";
        private const string ERROR_DOCUMENT = "error_document";
        private Socket socket;

        public void Run()
        {
            Stream output = null;
            Stream input = null;

            try
            {
                input = new NetworkStream(socket);

                string line;
                string requestLine = null;
                string method = null;
                Map<string, string> requestHeader = new HashMap<string, string>();
                while ( (line = Util.readLine(input)) != null )
                {
                    if ( line == "" )
                    {
                        break;
                    }
                    if ( line.StartsWith("GET") )
                    {
                        method = "GET";
                        requestLine = line;
                    }
                    else if ( line.StartsWith("POST") )
                    {
                        method = "POST";
                        requestLine = line;
                    }
                    else
                    {
                        Util.parseHeader(requestHeader, line);
                    }
                }

                if ( requestLine == null )
                    return;


                string reqUri = MyURLDecoder.decode(requestLine.Split(' ')[1],
                                                   "UTF-8");
                string[] pathAndQuery = reqUri.Split('?');
                string path = pathAndQuery[0];
                string query = null;
                if ( pathAndQuery.Length > 1 )
                {
                    query = pathAndQuery[1];
                }
                output = new NetworkStream(socket);

                string appDir = path.Substring(1).Split('/')[0];
                WebApplication webApp = WebApplication.searchWebApplication(appDir);
                if ( webApp != null )
                {
                    ServletInfo servletInfo
                        = webApp.searchServlet(path.Substring(appDir.Length + 1));
                    if ( servletInfo != null )
                    {
                        ServletService.doService(method, query, servletInfo,
                                             requestHeader, input, output);
                        return;
                    }
                }
                string ext = null;
                string[] tmp = reqUri.Split('.');
                ext = tmp[tmp.Length - 1];

                if ( path.EndsWith("/") )
                {
                    path += "index.html";
                    ext = "html";
                }

                string realPath;
                try
                {
                    realPath = Path.GetFullPath(DOCUMENT_ROOT + path);
                }
                catch
                {
                    SendResponse.sendNotFoundResponse(output, ERROR_DOCUMENT);
                    return;
                }

                if ( !realPath.StartsWith(Directory.GetCurrentDirectory() + "\\" + DOCUMENT_ROOT) )
                {
                    SendResponse.sendNotFoundResponse(output, ERROR_DOCUMENT);
                    return;
                }
                else if ( Directory.Exists(realPath) )
                {
                    string host = requestHeader.get("HOST");
                    string location = "http://"
                        + ((host != null) ? host : Constants.SERVER_NAME)
                        + path + "/";
                    SendResponse.sendMovePermanentlyResponse(output, location);
                    return;
                }
                try
                {
                    using ( FileStream fis = new FileStream(realPath, FileMode.Open) )
                    {
                        SendResponse.sendOkResponse(output, fis, ext);
                        fis.Close();
                    }
                }
                catch
                {
                    SendResponse.sendNotFoundResponse(output, ERROR_DOCUMENT);
                    return;
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
                    if ( output != null )
                    {
                        output.Close();
                    }
                    if ( input != null )
                    {
                        input.Close();
                    }
                    socket.Close();
                }
                catch ( Exception ex )
                {
                    Console.Write(ex);
                }
            }
        }
        public ServerThread( Socket socket )
        {
            this.socket = socket;
        }
    }
}
