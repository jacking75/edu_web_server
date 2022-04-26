using System.Text;
using System.Security.Cryptography;

namespace Henacat_sharp.servletimpl
{
    class SessionIdGenerator
    {
        private RNGCryptoServiceProvider random;

        public string generateSessionId()
        {
            byte[] bytes = new byte[16];
            this.random.GetBytes(bytes);
            StringBuilder buffer = new StringBuilder();

            for ( int i = 0; i < bytes.Length; i++ )
            {
                buffer.Append(((byte) (bytes[i] & 0xff)).ToString("X2"));
            }
            return buffer.ToString();
        }

        public SessionIdGenerator()
        {
            random = new RNGCryptoServiceProvider();
        }
    }
}
