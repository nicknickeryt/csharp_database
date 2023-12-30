//TODO Element as data cell. Each element should be bundled with its own header (datatype, maxsize, etc), we'll use it instead of object
namespace BazaDanych
{
    class Element
    {

        public Header header {
            get;
            set;
        }

        public String value {
            get;
            set;
        }

    }
}