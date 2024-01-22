using static SimpleDatabase.Utils;
namespace SimpleDatabase
{
    class Program
    {
        static void Main(string[] args)
        {


            // Enable DB debug mode
            isDebugEnabled = true;
            printf("Hi. That's a SimpleDatabase test.");

            // Basics: add some headers, create table and db.
            Header id = new Header("ID", DataType.INT, 10);
            Header name = new Header("Name", DataType.STRING, 16);
            Header surname = new Header("Surname", DataType.STRING, 16);

            Header isStillWorking = new Header("isStillWorking", DataType.BOOLEAN, 1);


            Table table = new Table("pracownicy");

            table.addHeader(id);
            table.addHeader(name);
            table.addHeader(surname);
            table.addHeader(isStillWorking);


            // Create example db and add a table to that db. Note that a table doesn't have to be in db.
            Database database = new Database("Testowa baza danych");

            database.AddTable(table);


            // Add some example data.

            table.AddRow(1, "Jan", "Kowalski", true);

            table.AddRow(2, "Damian", "Nowak", false);

            table.AddRow(2, "Alan", "Nowak", true);


            // Wrong data types
            printDebugError(ErrorType.INTENTATIONAL);
            table.AddRow("ąąąą", "Alan", 1, "sdsad");

            // Wrong amount of data
            printDebugError(ErrorType.INTENTATIONAL);
            table.AddRow(2, "Alan", "S", true, true);

            table.printTable();

            Header dummyHeader = new Header("dummy", DataType.STRING, 8);

            table.addHeader(dummyHeader, "default");

            printDebugError(ErrorType.INTENTATIONAL);
            table.AddRow(2, "Alan", "Akshat", true, "zadługawartość");

            table.printTable();

            // Add another table and add to our db.

            Header id1 = new Header("ID", DataType.INT, 10);
            Header nazwa = new Header("Model", DataType.STRING, 16);
            Header year = new Header("Rok produkcji", DataType.INT, 4);

            Table table1 = new Table("samoloty");

            table1.addHeader(id1);
            table1.addHeader(nazwa);
            table1.addHeader(year);

            database.AddTable(table1);

            table1.AddRow(1, "Antonov 223", 2004);

            table1.AddRow(9, "Ił-15", 1983);

            table1.AddRow(8, "PZL-Świdnik M-3", 2013);

            table1.AddRow(9, "Boeing 737", 1999);
            table1.AddRow(3, "PZL-23 Sokół", 2004);



            // Basic sorting by header example.
            table1.printTable();

            table1.sortByHeader(nazwa, Table.Direction.ASC);

            table1.printTable();

            table1.sortByHeader(id1, Table.Direction.DESC);

            table1.printTable();

            table1.sortByHeader(year, Table.Direction.ASC);

            table1.printTable();

            table1.sortByHeader(id, Table.Direction.DESC);

            table1.printTable();

            // Try to get header out of range.
            printDebugError(ErrorType.INTENTATIONAL);
            table1.GetHeaderAt(100);


            // Table & db serialize test.
            string tableCsv = "./examples/testowa.csv";
            string tableJson = "./examples/tableJson.json";
            string dbJson = "./examples/dbJson.json";

            table1.WriteToCsv(tableCsv);

            table1.Serialize(tableJson);


            Table table2 = Table.Deserialize(tableJson);

            table2.printTable();

            // Try wrong filename
            printDebugError(ErrorType.INTENTATIONAL);
            Table table3 = Table.Deserialize("wrongfilename");



            database.printTables();
            printf(database.Name);

            database.Serialize(dbJson);


            Database database1 = Database.Deserialize(dbJson);

            database1.printTables();
            printf(database1.Name);




            /* More examples:
             *
             * print row by header value(s)
             * get row by header value(s) 
             * remove row by header value(s) 
             * set row by header value(s) 
             *
            */

            /*
            table1.PrintRowByHeaderValue(year, 2004);

                Dictionary<Header, List<object>> results = table1.GetRowByHeaderValue(year, 2004);

                Dictionary<Header, List<object>> resultsEmployees = table.GetRowByHeaderValue(isStillWorking, true);

                table.PrintRowByHeaderValue(isStillWorking, true);

                foreach(object o in resultsEmployees[name]){
                    printf(o.ToString());
                }

                foreach(object o in results[nazwa]){
                    printf(o.ToString());
                }


                table.RemoveRowByHeaderValue(isStillWorking, true);

                table.printTable();

                table.AddRow(3, "Adrian", "Kowalik", true);

                table.printTable();

                table.RemoveRowByHeaderValue(surname, "Nowak");

                table.printTable();
                table.AddRow(4, "Anna", "Kowalik", true);

                table.printTable();

                Dictionary<Header, object> var = new Dictionary<Header, object>
                {
                    { name, "Anna" },
                    { surname, "Kowalik" }
                };

                table.RemoveRowByHeaderValues(var);

                table.printTable();

                table.removeHeader(surname);

                table.printTable();

                table.addNewHeader("hasSuperPowers", DataType.BOOLEAN, 1, "WARTOSC domyslna");

                table.printTable();

                Header header = table.GetHeaderByName("hasSuperPowers");

                // Attempt to add wrong data type
                printDebugError(ErrorType.INTENTATIONAL);
                table.SetRowByHeaderValue(header, "WARTOSC domyslna", isStillWorking, "WARTOSC nowa");

                table.printTable();

                // Now add properly
                table.SetRowByHeaderValue(header, "WARTOSC domyslna", header, true);

                table.printTable();

                Dictionary<Header, object> var1 = new Dictionary<Header, object>
                {
                    { name, "Adrian" },
                    { isStillWorking, true }
                };


                Dictionary<Header, List<object>> resultsEmployees2 = table.GetRowByHeaderValues(var1);

                foreach(object o in resultsEmployees2[name]){
                    printf(o.ToString());
                }

                table.printTable();

                table.AddRow(3, "Adrian", false, false);

                table.printTable();

                Dictionary<Header, List<object>> resultsEmployees3 = table.GetRowByHeaderValues(var1);

                foreach(object o in resultsEmployees3[name]){
                    printf(o.ToString());
                }
                



                // Set row elements by multiple values
                table.printTable();

                Dictionary<Header, object> findSet = new Dictionary<Header, object>(){
                { name, "Adrian" },
                { isStillWorking, true }
                };
                
                Dictionary<Header, object> toSet = new Dictionary<Header, object>(){
                { id, 89 },
                { isStillWorking, false }
                };

                table.SetRowByHeaderValues(findSet, toSet);

                table.printTable();
                */

        }
    }
}
