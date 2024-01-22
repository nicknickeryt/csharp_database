namespace SimpleDatabase
{

    // Supported data types.
    enum DataType
    {
        INT,
        STRING,
        FLOAT,
        DOUBLE,
        BOOLEAN,
        UNKNOWN
    }
    class Header
    {

        // Constructor. Note that whenever the data type given is BOOLEAN then maxSize is always be equal to 1 as setting a BOOLEAN size is redundant.
        public Header(string name, DataType dataType, int maxSize)
        {
            Name = name;
            DataType = dataType;
            MaxSize = maxSize;
            if (dataType == DataType.BOOLEAN) MaxSize = 1;

        }

        // Properties
        public string Name
        {
            get;
            set;
        }
        public DataType DataType
        {
            get;
            set;
        }
        public int MaxSize
        {
            get;
            set;
        }
    }

}
