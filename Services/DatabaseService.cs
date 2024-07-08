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
            string dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "MyDatabase.db3");
            _database = new SQLiteAsyncConnection(dbPath);

            //create tables if they don t exist
            _database.CreateTableAsync<Expense>().Wait();
            _database.CreateTableAsync<Income>().Wait();
            _database.CreateTableAsync<Goal>().Wait();
            _database.CreateTableAsync<RecurringPayment>().Wait();
            _database.CreateTableAsync<RecurringPaymentExpenseLink>().Wait();
        }

        public SQLiteAsyncConnection GetDatabaseConnection()
        {
            return _database;
        }
    }
}
