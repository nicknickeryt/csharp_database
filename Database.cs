// TODO write to file, search, sort, edit idk
namespace BazaDanych
{
    class Database
    {

        private List<Table> tables = new List<Table>();

        public Database()
        {

        }

        public void printTables()
        {
            Utils.printSpacer();
            int i = 0;
            foreach (Table table in tables)
            {
                Console.WriteLine("Table no. " + i);
                Console.WriteLine("Name: " + table.Name);
                Console.WriteLine("Size: " + table.Size());

                i++;
            }
            Utils.printSpacer();

        }

        public Table CreateTable(String name)
        {
            Table table = new Table(name);
            tables.Add(table);
            return table;
        }
        public Table CreateTable(String name, List<Header> headers)
        {
            Table table = new Table(name, headers);
            tables.Add(table);
            return table;
        }
        public int AddTable(Table table)
        {
            tables.Add(table);
            return 0;
        }
        public int RemoveTable(string name)
        {
            foreach (Table table in tables)
            {
                if (table.Name == name)
                {
                    tables.Remove(table);
                }
            }
            return 0;
        }
    }
}
