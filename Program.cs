//TODO add class for data cell
using static BazaDanych.Utils;
namespace BazaDanych
{
    class Program
    {
        static void Main(string[] args)
        {

            Utils.isDebugEnabled = true;
            Console.WriteLine("hi, that's a test");


            Header id = new Header("ID", DataType.INT, 10);
            Header name = new Header("Name", DataType.STRING, 16);
            Header surname = new Header("Surname", DataType.STRING, 16);

            Header isStillWorking = new Header("isStillWorking", DataType.BOOLEAN, 1);


            Table table = new Table("pracownicy");

            table.addHeader(id);
            table.addHeader(name);
            table.addHeader(surname);
            table.addHeader(isStillWorking);

            //table.printHeader(true);

            Database database = new Database();

            database.AddTable(table);


            //testy//

            table.AddRow(1, "Jan", "Kowalski", true);

            table.AddRow(2, "Damian", "Nowak", false);

            table.AddRow(2, "Alan", "Nowak", true);

            //table.printColumns();

            //table.printRows();

            table.printTable();



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


            table1.printTable();


            table1.sortByHeader(nazwa, Table.Direction.ASC);

            table1.printTable();

            table1.sortByHeader(id1, Table.Direction.DESC);

            table1.printTable();
                        
            table1.sortByHeader(year, Table.Direction.ASC);

            table1.printTable();

            table1.sortByHeader(id, Table.Direction.DESC);

            table1.printTable();
            /*table1.PrintRowByHeaderValue(year, 2004);

                Dictionary<Header, List<object>> results = table1.GetRowByHeaderValue(year, 2004);

                Dictionary<Header, List<object>> resultsEmployees = table.GetRowByHeaderValue(isStillWorking, true);

                table.PrintRowByHeaderValue(isStillWorking, true);

                foreach(object o in resultsEmployees[name]){
                    printLine(o.ToString());
                }

                foreach(object o in results[nazwa]){
                    printLine(o.ToString());
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

                //attempt to add wrogn data type
                table.SetElementByHeaderValue(header, "WARTOSC domyslna", isStillWorking, "WARTOSC nowa");

                table.printTable();

                //now add properly
                table.SetElementByHeaderValue(header, "WARTOSC domyslna", header, true);

                table.printTable();

                Dictionary<Header, object> var1 = new Dictionary<Header, object>
                {
                    { name, "Adrian" },
                    { isStillWorking, true }
                };

                Dictionary<Header, List<object>> resultsEmployees2 = table.GetRowByHeaderValues(var1);

                foreach(object o in resultsEmployees2[name]){
                    printLine(o.ToString());
                }

                table.printTable();

                table.AddRow(3, "Adrian", false, false);

                table.printTable();

                 Dictionary<Header, List<object>> resultsEmployees3 = table.GetRowByHeaderValues(var1);

                foreach(object o in resultsEmployees3[name]){
                    printLine(o.ToString());
                }
                */

        }
    }
}
