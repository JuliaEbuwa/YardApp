using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using yard.domain.Models;
using yard.domain.ViewModels;

namespace yard.infrastructure.Repositories.Interface
{
    public interface ITransactionRepository
    {
        IQueryable<TransactionVM> GetAllSuccessfulTransactions(DateTime startDate, DateTime endDate);
    }
}
