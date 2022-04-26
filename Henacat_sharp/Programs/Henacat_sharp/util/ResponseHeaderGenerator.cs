using System.IO;

namespace Henacat_sharp.util
{
    public interface ResponseHeaderGenerator
    {
        void generate( Stream output );
    }
}
