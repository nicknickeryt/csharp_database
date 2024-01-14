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
            if(dataType == DataType.BOOLEAN) MaxSize = 1;

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
