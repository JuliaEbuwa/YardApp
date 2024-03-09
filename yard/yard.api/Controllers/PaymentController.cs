using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using yard.application.Services.Interface;
using yard.domain.ViewModels;
using yard.domain.ViewModels.yard.domain.ViewModels;

namespace yard.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentService _paymentService;

        public PaymentController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        [HttpGet("verify_payment/{reference}")]
        public async Task<IActionResult> VerifyPayment(string reference)
        {
            var response = await _paymentService.VerifyPayment(reference);
            if (response != null && response.status == true)
            {
                return Ok(response);

            }
            return BadRequest(response);
        }

        [HttpPost("process")]
        public async Task<IActionResult> ProcessPayment(InitializePaymentVM paymentDetails)
        {
            var res = await _paymentService.ProcessPayment(paymentDetails.email, paymentDetails.amount);
            if (res != null && res.status == true)
            {

                return Ok(res);

            }
            return BadRequest();
        }
    }
}

