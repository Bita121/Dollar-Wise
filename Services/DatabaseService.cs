using System;
using System.IO;
using Dollar_Wise.Models;
using SQLite;

namespace Dollar_Wise.Services
{
    public class DatabaseService
    {
        SQLiteAsyncConnection _database;

        public DatabaseService()
        {
            // Specify the path for the SQLite database file
            string dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "MyDatabase.db3");

            // Initialize SQLite connection
            _database = new SQLiteAsyncConnection(dbPath);

            // Create tables if they don't exist
            _database.CreateTableAsync<Expense>().Wait();
            _database.CreateTableAsync<Income>().Wait();
        }

        public SQLiteAsyncConnection GetDatabaseConnection()
        {
            return _database;
        }
    }
}
