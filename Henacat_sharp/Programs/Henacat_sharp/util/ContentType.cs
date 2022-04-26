using System.Collections.Generic;

namespace Henacat_sharp.util
{
    class ContentType
    {
        private string type;
        private string subType;
        private Map<string, string> attributes;

        public string getType()
        {
            return type;
        }

        public string getSubType()
        {
            return subType;
        }

        public string getAttribute( string key )
        {
            return attributes.get(key);
        }

        public ContentType( string type, string subType,
                Map<string, string> attributes )
        {
            this.type = type;
            this.subType = subType;
            this.attributes = attributes;
        }
    }
}
