using System;
using System.Collections.Generic;
using System.IO;
using System.Globalization;

namespace Henacat_sharp.util
{
    class Util
    {
        // InputStreamからのバイト列を、行単位で読み込むユーティリティメソッド
        public static string readLine( Stream input )
        {
            int ch;
            string ret = "";
            while ( (ch = input.ReadByte()) != -1 )
            {
                if ( ch == '\r' )
                {
                    // 何もしない
                }
                else if ( ch == '\n' )
                {
                    break;
                }
                else
                {
                    ret += (char) ch;
                }
            }
            if ( ch == -1 )
            {
                return null;
            }
            else
            {
                return ret;
            }
        }

        // 1行の文字列を、バイト列としてStreamに書き込む
        // ユーティリティメソッド
        public static void writeLine( Stream output, string str )
        {
            foreach ( char ch in str.ToCharArray() )
            {
                output.WriteByte((byte) ch);
            }
            output.WriteByte((byte) '\r');
            output.WriteByte((byte) '\n');
        }

        // 現在時刻から、HTTP標準に合わせてフォーマットされた日付文字列を返す
        public static string getDateStringUtc()
        {
            DateTime time = DateTime.Now.ToUniversalTime();
            string timestr = time.ToString("ddd, dd MMM yyyy HH:mm:ss", CultureInfo.CreateSpecificCulture("en-US"));
            return timestr + " GMT";
        }

        // 拡張子とContent-Typeの対応表
        private static HashMap<string, string> contentTypeMap = new HashMap<string, string>()
            {
                {"html", "text/html"},
                {"htm", "text/html"},
                {"txt", "text/plain"},
                {"css", "text/css"},
                {"png", "image/png"},
                {"jpg", "image/jpeg"},
                {"jpeg", "image/jpeg"},
                {"gif", "image/gif"}
            };


        // 拡張子を受け取りContent-Typeを返す
        public static string getContentType( string ext )
        {
            string ret = contentTypeMap.get(ext.ToLower());
            if ( ret == null )
            {
                return "application/octet-stream";
            }
            else
            {
                return ret;
            }
        }

        // リクエスト(等の)ヘッダを名前と値に分離し、headerMapに追加する。
        public static void parseHeader( Map<string, string> headerMap,
             string line )
        {
            int colonPos = line.IndexOf(':');
            if ( colonPos == -1 )
                return;

            string headerName = line.Substring(0, colonPos).ToUpper();
            string headerValue = line.Substring(colonPos + 1).Trim();
            headerMap.Add(headerName, headerValue);
        }

        // Content-Typeの文字列をパースする
        public static ContentType parseContentType( string str )
        {
            string[] delimiter = { "; " };

            string[] temp = str.Split(delimiter, StringSplitOptions.None);
            string[] typeSubType = temp[0].Split('/');
            string type = typeSubType[0];
            string subType = null;
            if ( typeSubType.Length > 1 )
            {
                subType = typeSubType[1];
            }
            Map<string, string> attributes = new HashMap<string, string>();

            for ( int i = 1; i < temp.Length; i++ )
            {
                string[] keyValue = temp[i].Split('=');
                attributes.Add(keyValue[0].ToUpper(), keyValue[1]);
            }

            return new ContentType(type, subType, attributes);
        }
    }
}
