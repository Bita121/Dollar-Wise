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
            await _database.CreateTableAsync<Goal>();
            await _database.CreateTableAsync<RecurringPayment>();
            await _database.CreateTableAsync<RecurringPaymentExpenseLink>(); // Create RecurringPaymentExpenseLink table
        }

        // CRUD Expense

        public async Task AddExpense(Expense expense)
        {
            await _database.InsertAsync(expense);
        }

        public async Task<List<Expense>> GetExpenses()
        {
            return await _database.Table<Expense>().ToListAsync();
        }

        public async Task<Expense> GetExpenseById(int expenseId)
        {
            return await _database.Table<Expense>()
                                  .Where(expense => expense.Id == expenseId)
                                  .FirstOrDefaultAsync();
        }

        public async Task UpdateExpense(Expense expense)
        {
            await _database.UpdateAsync(expense);
        }

        public async Task DeleteExpense(Expense expense)
        {
            await _database.DeleteAsync(expense);
        }

        // CRUD Income

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

        // CRUD Goal

        public async Task AddGoal(Goal goal)
        {
            await _database.InsertAsync(goal);
        }

        public async Task<List<Goal>> GetGoals()
        {
            return await _database.Table<Goal>().ToListAsync();
        }

        public async Task UpdateGoal(Goal goal)
        {
            await _database.UpdateAsync(goal);
        }

        public async Task DeleteGoal(Goal goal)
        {
            await _database.DeleteAsync(goal);
        }

        // CRUD RecurringPayment

        public async Task AddRecurringPayment(RecurringPayment recurringPayment)
        {
            await _database.InsertAsync(recurringPayment);
        }

        public async Task<List<RecurringPayment>> GetRecurringPayments()
        {
            return await _database.Table<RecurringPayment>().ToListAsync();
        }

        public async Task UpdateRecurringPayment(RecurringPayment recurringPayment)
        {
            await _database.UpdateAsync(recurringPayment);
        }

        public async Task DeleteRecurringPayment(RecurringPayment recurringPayment)
        {
            await _database.DeleteAsync(recurringPayment);
        }

        // CRUD RecurringPaymentExpenseLink

        public async Task AddRecurringPaymentExpenseLink(RecurringPaymentExpenseLink link)
        {
            await _database.InsertAsync(link);
        }

        public async Task<List<RecurringPaymentExpenseLink>> GetRecurringPaymentExpenseLinks()
        {
            return await _database.Table<RecurringPaymentExpenseLink>().ToListAsync();
        }

        public async Task<List<int>> GetExpenseIdsForRecurringPayment(int recurringPaymentId)
        {
            var links = await _database.Table<RecurringPaymentExpenseLink>()
                                       .Where(link => link.RecurringPaymentId == recurringPaymentId)
                                       .ToListAsync();

            return links.Select(link => link.ExpenseId).ToList();
        }

        public async Task DeleteRecurringPaymentExpenseLink(RecurringPaymentExpenseLink link)
        {
            await _database.DeleteAsync(link);
        }
    }
}