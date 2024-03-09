using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using yard.domain.ViewModels;

namespace yard.application.Services.Interface
{
    public interface IPaymentService
    {
        Task<PaystackResponseVM> ProcessPayment(string email, long amount);
        Task<PaystackResponseVM> VerifyPayment(string reference);
    }
}
