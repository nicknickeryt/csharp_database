namespace BazaDanych
{
    
    //TODO more data types
    enum DataType {
        INT,
        STRING,
        FLOAT,
        DOUBLE,
        BOOLEAN,
        UNKNOWN
    }
    class Header
    {

        public Header(String name, DataType dataType, int maxSize) {
            Name = name;
            DataType = dataType;
            MaxSize = maxSize;

        }

        public string Name {
            get;
            set;
        }
        public DataType DataType {
            get;
            set;
        }
        public int MaxSize {
            get;
            set;
        }
    }
        
}   
