using static SimpleDatabase.Utils;
using System.Text.Json;


namespace SimpleDatabase
{

    /* Table
     *
     * This is the crucial class for SimpleDatabse. It manages almost all the stuff going on with tables
     *
     * These include:
     *  creating, removing a table with various arguments
     *  printing tables
     *  sorting rows
     *  table row management: add, remove & edit
     *  writing tables to CSV or JSON
     *
     */

    class Table
    {

        public enum Direction
        {
            ASC,
            DESC
        }

        private JsonSerializerOptions opts = new JsonSerializerOptions() { WriteIndented = true, IncludeFields = true };

        public string Name
        {
            get;
            set;
        }


        public List<Header> Headers
        {
            get;
            set;
        }

        public List<List<object>> Rows
        {
            get;
            set;
        }


        // Costructors
        public Table(string name, List<Header> headers)
        {
            Name = name;
            Headers = headers;
            Rows = new List<List<object>>();
        }


        public Table(string name)
        {
            Name = name;
            Rows = new List<List<object>>();
            Headers = new List<Header>();
        }

        public Table()
        {
            Name = "NULL";
            Headers = new List<Header>();
            Rows = new List<List<object>>();
        }


        // Gets the whole column by header. That is the replacement of SimepleDatabase legacy setup with columns & rows declared separately.
        //  Returns the list of objects in the given column.
        public List<object> getColumn(Header header)
        {
            if (!Headers.Contains(header))
            {
                printDebugError(ErrorType.HEADER);
                return null;
            }
            List<object> column = new List<object>();
            int i = Headers.IndexOf(header);

            foreach (List<object> row in Rows)
            {
                column.Add(row[i]);
            }

            return column;

        }


        // Sorts rows by header & direction.
        public void sortByHeader(Header header, Direction direction)
        {
            if (!Headers.Contains(header))
            {
                return;
            }
            List<object> sortId = getColumn(header);


            switch (direction)
            {
                case Direction.ASC:
                    sortId = sortId.OrderBy(x => x).ToList();
                    break;
                case Direction.DESC:
                    sortId = sortId.OrderByDescending(x => x).ToList();
                    break;

            }

            List<List<object>> newRows = new List<List<object>>();

            Rows = Rows.OrderBy(x => x[Headers.IndexOf(header)]).ToList();


        }


        // Adds a row.
        public int AddRow(params object[] values)
        {
            if (Headers.Count != values.Count())
            {
                printDebugError(ErrorType.AMOUNT);
                return -1;
            }


            int i = 0;
            foreach (object value in values)
            {
                if (!checkDataType(GetHeaderAt(i), value))
                {
                    printDebugError(ErrorType.TYPE);
                    return -1;
                }
                if (getDataType(value) != DataType.BOOLEAN && value.ToString().Length > GetHeaderAt(i).MaxSize)
                {
                    printDebugError(ErrorType.SIZE);
                    return -1;
                }
                i++;
            }

            Rows.Add(values.ToList());

            return 0;

        }


        public int Size()
        {
            return Rows.Count;
        }

        // Gets a header at a given index.
        //  This is helpful to determine which header is the current one while looping over row.
        public Header GetHeaderAt(int index)
        {
            if (index >= Headers.Count())
            {
                printDebugError(ErrorType.HEADER);
                return null;
            }
            return Headers[index];
        }
        public Header GetHeaderByName(string name)
        {
            foreach (Header header in Headers)
            {
                if (header.Name == name) return header;
            }

            printDebugError(ErrorType.HEADER);
            return null;
        }

        // Initializes row/column headers when adding a row.
        private void initColumnHeader(Header header, object defaultValue)
        {

            if (defaultValue != null && !checkDataType(header, defaultValue)) return;

            int i = 0;


            foreach (Header header1 in Headers)
            {
                foreach (List<object> row in Rows)
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
            foreach (List<object> row in Rows)
            {
                if (Headers[i] == header)
                {
                    row[i] = null;
                }
                i++;
            }
        }

        // Adds a new header by Header object.
        public int addHeader(Header header, object defaultValue = null)
        {

            Headers.Add(header);
            initColumnHeader(header, defaultValue);
            return 0;
        }

        // Constructs a new Header object & adds a new header.
        public int addNewHeader(string name, DataType dataType, int maxSize, object defaultValue = null)
        {

            Header header = new Header(name, dataType, maxSize);
            Headers.Add(header);
            initColumnHeader(header, defaultValue);
            return 0;
        }

        // Removes header by name.
        public int removeHeader(string name)
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

        // Removes header by Header object.
        public int removeHeader(Header header)
        {
            if (!Headers.Contains(header)) return -1;
            List<List<object>> newRows = new List<List<object>>(Rows);
            int j = 0;
            foreach (List<object> row in Rows)
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
            Rows = newRows;

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
                    printf("Header: " + header.Name);
                }
                printSpacer();
                return;
            }


            printSpacer();
            int i = 0;
            foreach (Header header in Headers)
            {
                printf("Header no. " + i);
                printf("Name: " + header.Name);
                printf("DataType: " + header.DataType);
                printf("MaxSize: " + header.MaxSize);
                i++;
            }
            printSpacer();
        }

        public void printTableName()
        {
            printf("Table: " + Name);
        }

        // A helper method to print the table.
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
                    printf("┐");
                    break;

                case 2:
                    printf("┤");
                    break;

                case 3:
                    printf("┘");
                    break;

            }
        }

        // Prints the table in console using fancy formatting.
        public void printTable()
        {
            printTableName();
            printTableSpacer(1);
            foreach (Header header in Headers)
            {
                print($"│ {header.Name,-20} ");
            }
            printf("│");

            foreach (Header header in Headers)
            {
                string combinedData = header.DataType + "(" + header.MaxSize + ")";
                print($"│ {combinedData,-20} ");
            }

            printf("│");

            printTableSpacer(2);

            foreach (List<object> row in Rows)
            {
                int i = 0;
                foreach (object value in row)
                {
                    print($"│ {value,-20} ");
                    i++;
                }
                printf("│");
            }

            printTableSpacer(3);
        }




        /* Search methods
         *
         * Those methods are used to manage values in rows.
         *  These are similar & pretty much self-explanatory.
         *   All the methods require some kind of a header value and a search keyword.
         *   Multiple keyword methods require a variation of Dictionary<Header, object>.
         *
         *  SimpleDatabse supports the following operations:
         *   Print the whole row by header value
         *    Note that it's only a test method. 
         *    Get* methods are preferred and should be used whenever willing to print a table.
         *   Get the whole row header value(s)
         *   Remove the whole row header value(s)
         *   Set elements row by header value(s)
         *   
         */


        public void PrintRowByHeaderValue(Header header, object value)
        {
            if (!Headers.Contains(header))
            {
                printError(ErrorType.HEADER);
                return;
            }
            printf("Resulting rows with value '" + value.ToString() + "' in header '" + header.Name + "' in table '" + Name + "'");

            printTableSpacer(1);
            foreach (Header header1 in Headers)
            {
                print($"│ {header1.Name,-20} ");
            }
            printf("│");

            foreach (Header header1 in Headers)
            {
                string combinedData = header1.DataType + "(" + header1.MaxSize + ")";
                print($"│ {combinedData,-20} ");
            }

            printf("│");

            printTableSpacer(2);

            foreach (List<object> row in Rows)
            {
                int i = 0;

                if (row.Contains(value) && GetHeaderAt(row.IndexOf(value)) == header)
                {

                    foreach (object value1 in row)
                    {

                        print($"│ {value1,-20} ");
                        i++;
                    }
                    printf("│");
                }
            }

            printTableSpacer(3);

        }

        public Dictionary<Header, List<object>> GetRowByHeaderValue(Header header, object value)
        {
            if (!Headers.Contains(header))
            {
                printDebugError(ErrorType.HEADER);
                return null;
            }
            Dictionary<Header, List<object>> resultDict = new Dictionary<Header, List<object>>();


            foreach (List<object> row in Rows)
            {
                int i = 0;

                // Note that row.Contains(value) may seem unnecessary there, but it's crucial
                // for improving the search time in large tables.

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
            foreach (KeyValuePair<Header, object> kvp in headerValues)
            {
                if (!Headers.Contains(kvp.Key))
                {
                    printDebugError(ErrorType.HEADER);
                    return null;
                }
            }
            Dictionary<Header, List<object>> resultDict = new Dictionary<Header, List<object>>();

            foreach (List<object> row in Rows)
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
                printDebugError(ErrorType.HEADER);
                return;
            }
            List<List<object>> newRows = new List<List<object>>(Rows);
            foreach (List<object> row in Rows)
            {

                if (row.Contains(value) && GetHeaderAt(row.IndexOf(value)) == header)
                {
                    newRows.Remove(row);
                }
            }

            Rows = newRows;


        }

        public void RemoveRowByHeaderValues(Dictionary<Header, object> headerValues)
        {
            foreach (KeyValuePair<Header, object> kvp in headerValues)
            {
                if (!Headers.Contains(kvp.Key))
                {
                    printDebugError(ErrorType.HEADER);
                    return;
                }
            }

            List<List<object>> newRows = new List<List<object>>(Rows);
            foreach (List<object> row in Rows)
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

            Rows = newRows;
        }

        public int SetRowByHeaderValue(Header header, object value, Header destHeader, object newValue)
        {
            if (!Headers.Contains(header))
            {
                printDebugError(ErrorType.HEADER);
                return -1;
            }

            List<List<object>> newRows = new List<List<object>>(Rows);
            foreach (List<object> row in Rows)
            {
                List<object> newRow = new List<object>(row);

                if (row.Contains(value) && GetHeaderAt(row.IndexOf(value)) == header)
                {
                    if (checkDataType(destHeader, newValue)) newRow[Headers.IndexOf(destHeader)] = newValue;
                }
                newRows[newRows.IndexOf(row)] = newRow;
            }
            Rows = newRows;
            return 1;


        }
        public int SetRowByHeaderValues(Dictionary<Header, object> headerValues, Dictionary<Header, object> destHeaderValues)
        {
            foreach (KeyValuePair<Header, object> kvp in headerValues)
            {
                if (!Headers.Contains(kvp.Key))
                {
                    printDebugError(ErrorType.HEADER);
                    return -1;
                }
            }
            List<List<object>> newRows = new List<List<object>>(Rows);
            foreach (List<object> row in Rows)
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

            Rows = newRows;

            return 1;
        }


        /* Write methods
         *
         * Those methods are used to write the table to a file.
         *
         *  SimpleDatabse supports serializing a Table object to a JSON file and deserializing it.
         *  SimpleDatabase also allows to write a Table object to CSV file.
         *   
         */


        public void Serialize(string filePath)
        {
            string tableToJson = JsonSerializer.Serialize(this, opts);
            try
            {
                File.WriteAllText(filePath, tableToJson);
            }
            catch (Exception e)
            {
                printDebugError(ErrorType.JSON);
                printDebug(e.Message);

            }
        }

        public static Table Deserialize(string filePath)
        {
            try
            {
                string jsonFromFile = File.ReadAllText(filePath);
                Table tableFromJson = JsonSerializer.Deserialize<Table>(jsonFromFile);
                return tableFromJson;
            }
            catch (Exception e)
            {
                printDebugError(ErrorType.JSON);
                printDebug(e.Message);
            }
            return null;
        }



        public void WriteToCsv(string filePath)
        {
            using (StreamWriter writer = new StreamWriter(filePath))
            {
                try
                {
                    WriteHeaders(writer);
                    WriteRows(writer);
                }
                catch (Exception)
                {
                    printDebugError(ErrorType.CSV);
                    return;
                }
            }

            printDebug($"CSV exported at {filePath}");
        }


        // Those are the helper methods for WriteToCsv.
        private void WriteHeaders(StreamWriter writer)
        {
            foreach (Header header in Headers)
            {
                writer.Write($"{header.Name} [{header.DataType}({header.MaxSize})]");
                if (header != Headers[Headers.Count - 1])
                {
                    writer.Write(",");
                }
            }
            writer.WriteLine();
        }

        private void WriteRows(StreamWriter writer)
        {
            foreach (List<object> row in Rows)
            {
                for (int i = 0; i < row.Count; i++)
                {
                    writer.Write(row[i]);
                    if (i != row.Count - 1)
                    {
                        writer.Write(",");
                    }
                }
                writer.WriteLine();
            }
        }

    }
}