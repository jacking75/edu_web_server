namespace Henacat_sharp.servlet.http
{
    public class Cookie
    {
        private string name;
        private string value;
        private string domain;
        private int maxAge = -1;
        private string path;
        private bool secure;
        private bool httpOnly;

        public Cookie( string name, string value )
        {
            this.name = name;
            this.value = value;
        }

        public void setDomain( string pattern )
        {
            this.domain = pattern;
        }

        public string getDomain()
        {
            return this.domain;
        }

        public void setMaxAge( int expiry )
        {
            this.maxAge = expiry;
        }

        public int getMaxAge()
        {
            return this.maxAge;
        }

        public void setPath( string uri )
        {
            this.path = uri;
        }

        public string getPath()
        {
            return this.path;
        }

        public void setSecure( bool flag )
        {
            this.secure = flag;
        }

        public bool getSecure()
        {
            return this.secure;
        }

        public void setHttpOnly( bool httpOnly )
        {
            this.httpOnly = httpOnly;
        }

        public bool isHttpOnly()
        {
            return this.httpOnly;
        }

        public string getName()
        {
            return this.name;
        }

        public void setValue( string newValue )
        {
            this.value = newValue;
        }

        public string getValue()
        {
            return this.value;
        }
    }
}
