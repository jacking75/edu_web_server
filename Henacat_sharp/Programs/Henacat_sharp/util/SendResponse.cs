using System.IO;

namespace Henacat_sharp.util
{
    public class SendResponse
    {
        public static void sendOkResponseHeader( Stream output,
                                                string contentType,
                                                ResponseHeaderGenerator hg )
        {
            Util.writeLine(output, "HTTP/1.1 200 OK");
            Util.writeLine(output, "Date: " + Util.getDateStringUtc());
            Util.writeLine(output, "Server: Henacat/0.4");
            Util.writeLine(output, "Connection: close");
            Util.writeLine(output, "Content-type: " + contentType);
            hg.generate(output);
            Util.writeLine(output, "");
        }

        public static void sendOkResponse( Stream output, Stream fis,
                                          string ext )
        {
            Util.writeLine(output, "HTTP/1.1 200 OK");
            Util.writeLine(output, "Date: " + Util.getDateStringUtc());
            Util.writeLine(output, "Server: Henacat/0.4");
            Util.writeLine(output, "Connection: close");
            Util.writeLine(output, "Content-type: "
                       + Util.getContentType(ext));
            Util.writeLine(output, "");

            int ch;
            while ( (ch = fis.ReadByte()) != -1 )
            {
                output.WriteByte((byte) ch);
            }
        }

        public static void sendMovePermanentlyResponse( Stream output,
                                                       string location )

        {
            Util.writeLine(output, "HTTP/1.1 301 Moved Permanently");
            Util.writeLine(output, "Date: " + Util.getDateStringUtc());
            Util.writeLine(output, "Server: Henacat/0.4");
            Util.writeLine(output, "Location: " + location);
            Util.writeLine(output, "Connection: close");
            Util.writeLine(output, "");
        }

        public static void sendFoundResponse( Stream output,
                                             string location )

        {
            Util.writeLine(output, "HTTP/1.1 302 Found");
            Util.writeLine(output, "Date: " + Util.getDateStringUtc());
            Util.writeLine(output, "Server: Henacat/0.4");
            Util.writeLine(output, "Location: " + location);
            Util.writeLine(output, "Connection: close");
            Util.writeLine(output, "");
        }

        public static void sendNotFoundResponse( Stream output,
                                                string errorDocumentRoot )

        {
            Util.writeLine(output, "HTTP/1.1 404 Not Found");
            Util.writeLine(output, "Date: " + Util.getDateStringUtc());
            Util.writeLine(output, "Server: Henacat/0.4");
            Util.writeLine(output, "Connection: close");
            Util.writeLine(output, "Content-type: text/html");
            Util.writeLine(output, "");


            using ( FileStream fis = new FileStream(errorDocumentRoot + "/404.html", FileMode.Open) )
            {
                int ch;
                while ( (ch = fis.ReadByte()) != -1 )
                {
                    output.WriteByte((byte) ch);
                }
            }

        }

    }

}
