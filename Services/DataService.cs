using Dollar_Wise.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Dollar_Wise.Services
{
    public class DataService
    {
        private readonly SQLiteAsyncConnection _database;

        public DataService(DatabaseService databaseService)
        {
            _database = databaseService.GetDatabaseConnection();
            InitializeTables();
        }

        private async void InitializeTables()
        {
            await _database.CreateTableAsync<Expense>();
            await _database.CreateTableAsync<Income>();
        }

        // CRUD operations for Expense

        public async Task AddExpense(Expense expense)
        {
            await _database.InsertAsync(expense);
        }

        public async Task<List<Expense>> GetExpenses()
        {
            return await _database.Table<Expense>().ToListAsync();
        }

        public async Task UpdateExpense(Expense expense)
        {
            await _database.UpdateAsync(expense);
        }

        public async Task DeleteExpense(Expense expense)
        {
            await _database.DeleteAsync(expense);
        }

        // CRUD operations for Income

        public async Task AddIncome(Income income)
        {
            await _database.InsertAsync(income);
        }

        public async Task<List<Income>> GetIncomes()
        {
            return await _database.Table<Income>().ToListAsync();
        }

        public async Task UpdateIncome(Income income)
        {
            await _database.UpdateAsync(income);
        }

        public async Task DeleteIncome(Income income)
        {
            await _database.DeleteAsync(income);
        }
    }
}
