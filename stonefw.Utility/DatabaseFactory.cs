namespace Stonefw.Utility
{
    public class DatabaseFactory
    {
        public static Database CreateDatabase()
        {
            return new Database();
        }

        public static Database CreateDatabase(string dbName)
        {
            return new Database(dbName);
        }
    }
}