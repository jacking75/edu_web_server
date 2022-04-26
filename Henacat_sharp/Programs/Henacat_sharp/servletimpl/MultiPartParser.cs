using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Henacat_sharp.util;
using Henacat_sharp.servlet.http;

namespace Henacat_sharp.servletimpl
{
    class MultiPartParser
    {
        public static HttpServletRequestImpl parse( Map<string, string> requestHeader,
         Stream input,
         string contentTypeStr, int contentLength,
         HttpServletResponseImpl resp,
         WebApplication webApp )
        {
            ContentType contentType = Util.parseContentType(contentTypeStr);
            string boundary = "--" + contentType.getAttribute("BOUNDARY");
            List<Part> partList = new List<Part>();
            int length = contentLength;
            length = readToBoundary(input, boundary, length, null);
            HashMap<string, byte[][]> parameterMap = new HashMap<string, byte[][]>();
            int cnt = 0;
            for ( ;;)
            {
                Map<string, string> headerMap = new HashMap<string, string>();
                string line;
                while ( (line = Util.readLine(input)) != null )
                {
                    length -= line.Length + 2; // 2はCR+LF分
                    if ( line == "" )
                    {
                        break;
                    }
                    Util.parseHeader(headerMap, line);
                }
                ContentType cd
                    = Util.parseContentType(headerMap.get("CONTENT-DISPOSITION"));
                string quotedName = cd.getAttribute("NAME");
                string name = quotedName.Substring(1, quotedName.Length - 1);
                string ct = headerMap.get("CONTENT-TYPE");
                byte[][] dataOut = new byte[1][];
                length = readToBoundary(input, "\r\n" + boundary, length, dataOut);
                PartImpl part = new PartImpl(ct, headerMap, dataOut[0], name);
                partList.Add(part);
                if ( ct == null )
                {
                    byte[][] array = parameterMap.get(name);
                    if ( array == null )
                    {
                        parameterMap.Add(name, new byte[][] { dataOut[0] });
                    }
                    else
                    {
                        byte[][] newArray = new byte[array.Length + 1][];
                        Array.Copy(array, 0, newArray, 0, array.Length);
                        newArray[array.Length] = dataOut[0];
                        if ( parameterMap.ContainsKey(name) )
                        {
                            // 送信されたパラメータ名が重複した場合はカウント値を追加する
                            // 例外回避のため
                            cnt++;
                            parameterMap.Add(name + cnt.ToString(), newArray);
                        }
                        else
                        {
                            parameterMap.Add(name, newArray);
                        }
                    }
                }
                if ( length == 0 )
                {
                    break;
                }
            }
            HttpServletRequestImpl req
                    = new HttpServletRequestImpl(requestHeader, parameterMap,
                            partList, resp, webApp);

            return req;
        }

        // inputから、boundaryの終了まで読み取り、boundaryの手前までの
        // バイト列をdataOut[0]に返す(dataOutがnullであれば返さない)。
        // Content-Lengthの残りを戻り値として返す。
        private static int readToBoundary( Stream input,
                string boundary, int length,
                byte[][] dataOut )
        {
            MemoryStream output = new MemoryStream();

            int ch;
            int bPos = 0;
            bool found = false;
            while ( (ch = input.ReadByte()) != -1 && length > 0 )
            {
                length--;
                if ( ch == boundary.ElementAt(bPos) )
                {
                    bPos++;
                    if ( bPos == boundary.Length )
                    {
                        found = true;
                        ch = input.ReadByte();
                        if ( ch == '\r' )
                        {
                            input.ReadByte(); // '\n'
                            length -= 2;
                        }
                        else if ( ch == '-' )
                        {
                            input.ReadByte(); // '-'
                            input.ReadByte(); // \r
                            input.ReadByte(); // \n
                            length -= 4;
                        }
                        break;
                    }
                }
                else if ( bPos > 0 )
                {
                    byte[] bytes = Encoding.ASCII.GetBytes(boundary.Substring(0, bPos));
                    output.Write(bytes, 0, bytes.Length);
                    if ( ch == boundary.ElementAt(0) )
                    {
                        bPos = 1;
                    }
                    else
                    {
                        bPos = 0;
                        output.WriteByte((byte) ch);
                    }
                }
                else
                {
                    output.WriteByte((byte) ch);
                }
            }
            if ( found && dataOut != null )
            {
                dataOut[0] = output.ToArray();
            }
            return length;
        }
    }
}
