using Microsoft.AspNetCore.Mvc;
using yard.application.Services.Interface;
using yard.domain.ViewModels;

namespace yard.api.Controllers
{
    [Route("api/Transaction")]
    [ApiController]
    public class TransactionController : BaseApiController
    {
        private readonly ITransactionService _transactionService;

        public TransactionController(ITransactionService transactionService)
        {
            _transactionService = transactionService;
        }

     
        [HttpGet("GetAllSuccessfulTransaction")]
        public async Task<IActionResult> GetAllSuccessfulTransaction(GenericReportQuery query)
        {
            var result = await _transactionService.GetAllSuccessfulTransactions(query);

            return Response(result);
        }

    }
}
