using static SimpleDatabase.Utils;
using System.Text.Json;

namespace SimpleDatabase
{
    class Database
    {

        // Database properties
        private JsonSerializerOptions opts = new JsonSerializerOptions() { WriteIndented = true, IncludeFields = true };

        public string Name
        {
            get;
            set;
        }
        public List<Table> Tables
        {
            get;
            set;
        }

        // Constructors
        public Database(string name)
        {
            Tables = new List<Table>();
            Name = name;
        }

        public Database()
        {
            Tables = new List<Table>();
            Name = "NULL";
        }

        // Simple wrapper that prints all the tables in current db.
        public void printTables()
        {
            printSpacer();
            printf($"│ Tables in database: {Name}");
            printSpacer();
            int i = 0;
            foreach (Table table in Tables)
            {
                table.printTable();

                i++;
            }

        }

        // Add existing table to db.
        public int AddTable(Table table)
        {
            Tables.Add(table);
            return 0;
        }

        // Removes table from db by its name.
        public int RemoveTable(string name)
        {
            foreach (Table table in Tables)
            {
                if (table.Name == name)
                {
                    Tables.Remove(table);
                }
            }
            return 0;
        }

        // Removes table from db by Table object.
        public int RemoveTable(Table table)
        {
            Tables.Remove(table);
            return 0;
        }

        // Serialize db.
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

        // Deserialize db.
        public static Database Deserialize(string filePath)
        {
            try
            {
                string jsonFromFile = File.ReadAllText(filePath);
                Database tableFromJson = JsonSerializer.Deserialize<Database>(jsonFromFile);
                return tableFromJson;
            }
            catch (Exception e)
            {
                printDebugError(ErrorType.JSON);
                printDebug(e.Message);
            }
            return null;
        }
    }
}
