//TODO: remove, edit, sort, search better row and data handling, maxSize etc
namespace BazaDanych
{
    class Table
    {

        private Dictionary<Header, List<Object>> columns = new Dictionary<Header, List<Object>>();
        private List<List<Object>> rows = new List<List<Object>>();

        public string Name
        {
            get;
            set;
        }

        public List<Header> Headers = new List<Header>();

        //TODO: better error handling, row remove, honour maxSize
        public int AddRow(params Object[] values)
        {
            if (Headers.Count != values.Count())
            {

                Utils.printDebug("wrong amount of data.");
                return -1;
            }


            int i = 0;
            foreach (Object value in values)
            {
                if (Headers[i].DataType != Utils.getDataType(value))
                {
                    Utils.printDebug("wrong datatype.");
                    return -1;
                }
                i++;
            }

            i = 0;
            rows.Add(values.ToList());
            foreach (Header header in Headers)
            {
                List<object> list = columns[header];
                list.Add(values[i]);
                columns[header] = list;
                i++;
            }

            return 0;

        }
        public int Size()
        {
            return rows.Count;
        }

        public Table(String name, List<Header> headers)
        {
            Name = name;
            Headers = headers;
        }

        
        public Table(String name)
        {
            Name = name;
        }

        //FIXME ale to jest chamskie XD (add try catch etc.)
        public Header GetHeaderAt(int index){
            return Headers[index];
        }

        private void initColumnHeader(Header header)
        {
            columns.Add(header, new List<object>());
        }
        private void deinitColumnHeader(Header header)
        {
            columns.Remove(header);
        }

        public int addHeader(Header header)
        {
            Headers.Add(header);
            initColumnHeader(header);
            return 0;
        }
        public int addNewHeader(String name, DataType dataType, int maxSize)
        {
            Header header = new Header(name, dataType, maxSize);
            initColumnHeader(header);
            return 0;
        }
        public int removeHeader(String name)
        {
            foreach (Header header in Headers)
            {
                if (header.Name == name)
                {
                    Headers.Remove(header);
                    deinitColumnHeader(header);
                    return 0;
                }
            }
            return 1;
        }

        public void printHeader(bool brief = false)
        {   

            if(brief) {
            Utils.printSpacer();
            foreach (Header header in Headers)
            {
                Console.WriteLine("Header: " + header.Name);
            }
            Utils.printSpacer();
            return;
            }


            Utils.printSpacer();
            int i = 0;
            foreach (Header header in Headers)
            {
                Console.WriteLine("Header no. " + i);
                Console.WriteLine("Name: " + header.Name);
                Console.WriteLine("DataType: " + header.DataType);
                Console.WriteLine("MaxSize: " + header.MaxSize);
                i++;
            }
            Utils.printSpacer();
        }

        public void printColumns()
        {
            foreach (Header header in Headers)
            {
                Utils.printSpacer();
                Console.WriteLine("Header: " + header.Name);
                Utils.printSpacer();
                foreach (Object o in columns[header])
                {
                    Console.WriteLine(Utils.getDataType(o) + "(" + o.ToString().Length + "/" + header.MaxSize + "): " + o.ToString());
                }
            }
        }
        public void printRows()
        {
            int i = 0;
            foreach (List<object> list in rows)
            {
                Utils.printSpacer();
                Console.WriteLine("Row: " + i);
                foreach (Object o in list)
                {
                    Console.WriteLine(Utils.getDataType(o) + "(" + o.ToString().Length + "/" + GetHeaderAt(i).MaxSize + "): " + o.ToString());
                }

                Utils.printSpacer();
                i++;
            }
        }
    }
}
