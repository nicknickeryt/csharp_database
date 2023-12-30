//TODO add class for data cell

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


            Table table = new Table("pracownicy");

            table.addHeader(id);
            table.addHeader(name);
            table.addHeader(surname);

            table.printHeader(true);

            Database database = new Database();

            database.AddTable(table);


            table.AddRow(1, "Jan", "Kowalski");

            table.AddRow(2, "Damian", "Nowak");

            table.printColumns();

            table.printRows();

        }
    }
}
