//TODO: remove, edit (select elements/rows), sort, search better row and data handling, maxSize etc
using static BazaDanych.Utils;
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

                Utils.printDebug("Wrong amount of data.");
                return -1;
            }


            int i = 0;
            foreach (Object value in values)
            {
                if (Headers[i].DataType != Utils.getDataType(value))
                {
                    Utils.printDebug("Wrong datatype.");
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
        public Header GetHeaderAt(int index)
        {
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

            if (brief)
            {
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
                printSpacer();
                Console.WriteLine("Row: " + i);
                int j = 0;
                foreach (Object o in list)
                {

                    Console.WriteLine(GetHeaderAt(j).Name +
                    " [" +
                    getDataType(o) +
                    "(" +
                    o.ToString().Length +
                    "/" +
                    GetHeaderAt(j).MaxSize +
                    ")]: " +
                    o.ToString());

                    j++;
                }

                printSpacer();
                i++;
            }
        }

        public void printTableName()
        {
            printLine("Table: " + Name);
        }

        public void printTableSpacer(int type)
        {
            switch (type)
            {
                case 1:
                    print("┌");
                    break;

                case 2:
                    print("├");
                    break;

                case 3:
                    print("└");
                    break;

            }
            int spacerWidth = Headers.Count * 23;

            for (int i = 0; i < spacerWidth - 1; i++)
            {
                print("─");
            }

            switch (type)
            {
                case 1:
                    Console.WriteLine("┐");
                    break;

                case 2:
                    Console.WriteLine("┤");
                    break;

                case 3:
                    Console.WriteLine("┘");
                    break;

            }
        }

        public void printTable()
        {
            printTableName();
            printTableSpacer(1);
            foreach (Header header in Headers)
            {
                print($"│ {header.Name,-20} ");
            }
            Console.WriteLine("│");

            foreach (Header header in Headers)
            {
                string combinedData = header.DataType + "(" + header.MaxSize + ")";
                print($"│ {combinedData,-20} ");
            }

            Console.WriteLine("│");

            printTableSpacer(2);

            foreach (List<object> row in rows)
            {
                int i = 0;
                foreach (object value in row)
                {
                    print($"│ {value,-20} ");
                    i++;
                }
                Console.WriteLine("│");
            }

            printTableSpacer(3);
        }


        public void PrintRowByHeaderValue(Header header, object value)
        {

            printLine("Resulting rows with value '" + value.ToString() + "' in header '" + header.Name + "' in table '" + Name + "'");

            printTableSpacer(1);
            foreach (Header header1 in Headers)
            {
                print($"│ {header1.Name,-20} ");
            }
            Console.WriteLine("│");

            foreach (Header header1 in Headers)
            {
                string combinedData = header1.DataType + "(" + header1.MaxSize + ")";
                print($"│ {combinedData,-20} ");
            }

            Console.WriteLine("│");

            printTableSpacer(2);

            foreach (List<object> row in rows)
            {
                int i = 0;

                if (row.Contains(value) && GetHeaderAt(row.IndexOf(value)) == header)
                {

                    foreach (object value1 in row)
                    {

                        print($"│ {value1,-20} ");
                        i++;
                    }
                    Console.WriteLine("│");
                }
            }

            printTableSpacer(3);

        }

        public Dictionary<Header, List<object>> GetRowByHeaderValue(Header header, object value)
        {

        Dictionary<Header, List<object>> resultDict = new Dictionary<Header, List<object>>();


            foreach (List<object> row in rows)
            {
                int i = 0;

                if (row.Contains(value) && GetHeaderAt(row.IndexOf(value)) == header)
                {

                    foreach (object value1 in row)
                    {
                        Header currentHeader = GetHeaderAt(i);
                        if(!resultDict.ContainsKey(currentHeader)) resultDict[currentHeader] = new List<object>();
                        resultDict[currentHeader].Add(value1);
                        i++;
                    }
                }
            }

            return resultDict;

        }

    }
}
