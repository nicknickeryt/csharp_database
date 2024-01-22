//TODO: edit (select elements/rows), sort, better row and data handling
<<<<<<< HEAD
using System.Reflection.PortableExecutable;
using System.Runtime.InteropServices.Marshalling;
=======
>>>>>>> 6f2a3fa (something)
using static BazaDanych.Utils;
namespace BazaDanych
{
    class Table
    {

        public enum Direction
        {
            ASC,
            DESC
        }

        //private Dictionary<Header, List<Object>> columns = new Dictionary<Header, List<Object>>();
        private List<List<Object>> rows = new List<List<Object>>();


        private List<Object> getColumn(Header header)
        {
            //TODO: MUDI byc sprawdzenie tu i wszedzie indziej czy podany HEADER istnieje w aktualnej tabeli, w liscie headerow
            if (!Headers.Contains(header))
            {
                return null;
            }
            List<Object> column = new List<Object>();
            int i = Headers.IndexOf(header);
            
            foreach (List<Object> row in rows)
            {
                column.Add(row[i]);
            }

            return column;

        }


        public void sortByHeader(Header header, Direction direction)
        {
            if(!Headers.Contains(header))
            {
                return;
            }
            List<Object> sortId = getColumn(header);

            
            switch (direction)
            {
                case Direction.ASC:
                    sortId = sortId.OrderBy(x => x).ToList();
                    break;
                case Direction.DESC:
                    sortId = sortId.OrderByDescending(x => x).ToList();
                    break;

            }
        
            List<List<Object>> newRows = new List<List<Object>>();

            rows = rows.OrderBy(x => x[Headers.IndexOf(header)]).ToList();


        }

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

                printDebug("Wrong amount of data.");
                return -1;
            }


            int i = 0;
            foreach (Object value in values)
            {
                if (!checkDataType(Headers[i], value))
                {
                    printDebug("Wrong datatype.");
                    return -1;
                }
                i++;
            }

            i = 0;
            rows.Add(values.ToList());

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
        public Header GetHeaderByName(string name)
        {
            foreach (Header header in Headers)
            {
                if (header.Name == name) return header;
            }
            return null;
        }

        private void initColumnHeader(Header header, object defaultValue)
        {

            if (checkDataType(header, defaultValue))
            {
                return;
            }

            int i = 0;


            foreach (Header heade1 in Headers)
            {
                foreach (List<object> row in rows)
                {
                    if (row.ElementAtOrDefault(i) == null)
                    {
                        if (defaultValue == null)
                        {
                            row.Insert(i, "NULL");
                        }
                        else
                        {
                            row.Insert(i, defaultValue);
                        }
                    }
                }
                i++;
            }

        }
        private void deinitColumnHeader(Header header)
        {
            if (!Headers.Contains(header)) return;
            int i = 0;
            foreach (List<object> row in rows)
            {
                if (Headers[i] == header)
                {
                    row[i] = null;
                }
                i++;
            }
        }

        public int addHeader(Header header, object defaultValue = null)
        {

            Headers.Add(header);
            initColumnHeader(header, defaultValue);
            return 0;
        }
        public int addNewHeader(String name, DataType dataType, int maxSize, object defaultValue = null)
        {

            Header header = new Header(name, dataType, maxSize);
            Headers.Add(header);
            initColumnHeader(header, defaultValue);
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
        public int removeHeader(Header header)
        {
            if (!Headers.Contains(header)) return -1;
            List<List<object>> newRows = new List<List<object>>(rows);
            int j = 0;
            foreach (List<object> row in rows)
            {
                List<object> newRow = new List<object>(row);
                int i = 0;
                foreach (object o in row)
                {
                    if (GetHeaderAt(i) == header)
                    {
                        newRow.Remove(o);
                    }
                    i++;
                }
                newRows[j] = newRow;
                j++;
            }
            rows = newRows;

            Headers.Remove(header);

            return 1;
        }

        public void printHeader(bool brief = false)
        {

            if (brief)
            {
                printSpacer();
                foreach (Header header in Headers)
                {
                    Console.WriteLine("Header: " + header.Name);
                }
                printSpacer();
                return;
            }


            printSpacer();
            int i = 0;
            foreach (Header header in Headers)
            {
                Console.WriteLine("Header no. " + i);
                Console.WriteLine("Name: " + header.Name);
                Console.WriteLine("DataType: " + header.DataType);
                Console.WriteLine("MaxSize: " + header.MaxSize);
                i++;
            }
            printSpacer();
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
            if (!Headers.Contains(header))
            {
                printLine("Head not found."); 
                return; 
            }
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
            if (!Headers.Contains(header))
            {
                return null;
            }
            Dictionary<Header, List<object>> resultDict = new Dictionary<Header, List<object>>();


            foreach (List<object> row in rows)
            {
                int i = 0;

                if (row.Contains(value) && GetHeaderAt(row.IndexOf(value)) == header)
                {

                    foreach (object value1 in row)
                    {
                        Header currentHeader = GetHeaderAt(i);
                        if (!resultDict.ContainsKey(currentHeader)) resultDict[currentHeader] = new List<object>();
                        resultDict[currentHeader].Add(value1);
                        i++;
                    }
                }
            }

            return resultDict;

        }

        public Dictionary<Header, List<object>> GetRowByHeaderValues(Dictionary<Header, object> headerValues)
        {
            foreach(KeyValuePair<Header, object> kvp in headerValues)
            {
                if (!Headers.Contains(kvp.Key))
                {
                    return null;
                }
            }
            Dictionary<Header, List<object>> resultDict = new Dictionary<Header, List<object>>();

            foreach (List<object> row in rows)
            {

                int i = 0;
                foreach (KeyValuePair<Header, object> kvp in headerValues)
                {
                    if (row.Contains(kvp.Value) && GetHeaderAt(row.IndexOf(kvp.Value)) == kvp.Key)
                    {
                        i++;
                    }
                }

                if (i == headerValues.Count())
                {
                    for (int j = 0; j < Headers.Count(); j++)
                    {
                        Header currentHeader = GetHeaderAt(j);

                        if (!resultDict.ContainsKey(currentHeader)) resultDict[currentHeader] = new List<object>();

                        resultDict[currentHeader].Add(row[j]);
                    }

                }


            }

            return resultDict;
        }

        public void RemoveRowByHeaderValue(Header header, object value)
        {

            if (!Headers.Contains(header))
            {
                return;
            }
            List<List<object>> newRows = new List<List<object>>(rows);
            foreach (List<object> row in rows)
            {

                if (row.Contains(value) && GetHeaderAt(row.IndexOf(value)) == header)
                {
                    newRows.Remove(row);
                }
            }

            rows = newRows;


        }

        public void RemoveRowByHeaderValues(Dictionary<Header, object> headerValues)
        {
            foreach (KeyValuePair<Header, object> kvp in headerValues)
            {
                if (!Headers.Contains(kvp.Key))
                {
                    return;
                }
            }

            List<List<object>> newRows = new List<List<object>>(rows);
            foreach (List<object> row in rows)
            {

                int i = 0;
                foreach (KeyValuePair<Header, object> kvp in headerValues)
                {
                    if (row.Contains(kvp.Value) && GetHeaderAt(row.IndexOf(kvp.Value)) == kvp.Key)
                    {
                        i++;
                    }
                }
                if (i == headerValues.Count()) newRows.Remove(row);

            }

            rows = newRows;
        }

        public int SetElementByHeaderValue(Header header, object value, Header destHeader, object newValue)
        {
                if (!Headers.Contains(header))
                {
                return -1;
                }

            List<List<object>> newRows = new List<List<object>>(rows);
            foreach (List<object> row in rows)
            {
                List<object> newRow = new List<object>(row);

                if (row.Contains(value) && GetHeaderAt(row.IndexOf(value)) == header)
                {
                    if (checkDataType(destHeader, newValue)) newRow[Headers.IndexOf(destHeader)] = newValue;
                }
                newRows[newRows.IndexOf(row)] = newRow;
            }
            rows = newRows;
            return 1;


        }
        public int SetElementByHeaderValues(Dictionary<Header, object> headerValues, Dictionary<Header, object> destHeaderValues)
        {
            foreach (KeyValuePair<Header, object> kvp in headerValues)
            {
                if (!Headers.Contains(kvp.Key))
                {
                    return -1;
                }
            }
            List<List<object>> newRows = new List<List<object>>(rows);
            foreach (List<object> row in rows)
            {
                List<object> newRow = new List<object>(row);
                int i = 0;
                foreach (KeyValuePair<Header, object> kvp in headerValues)
                {
                    if (row.Contains(kvp.Value) && GetHeaderAt(row.IndexOf(kvp.Value)) == kvp.Key)
                    {
                        foreach (KeyValuePair<Header, object> kvp1 in destHeaderValues)
                        {
                            Header currentDestHeader = kvp1.Key;
                            newRow[Headers.IndexOf(currentDestHeader)] = kvp1.Value;
                        }
                        i++;
                    }
                }
                if (i == headerValues.Count())
                {
                    newRows[newRows.IndexOf(row)] = newRow;
                }

            }

            rows = newRows;

            return 1;
        }


    }
}
