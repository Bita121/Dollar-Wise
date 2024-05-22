using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dollar_Wise.Models
{
    public class RecurringPaymentExpenseLink
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public int RecurringPaymentId { get; set; }
        public int ExpenseId { get; set; }
    }
}
