using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using yard.application.Services.Interface;
using yard.domain.ViewModels;


namespace yard.application.Services.Implementation
{
    public class PaymentService : IPaymentService
    {
        private readonly HttpClient _httpClient;

        public PaymentService(IHttpClientFactory httpClient)
        {
            _httpClient = httpClient.CreateClient("Paystack");


        }

        public async Task<PaystackResponseVM> ProcessPayment(string _email, long _amount)
        {
            try
            {
                decimal amnt = _amount / 100.0m;
                string apiUrl = $"transaction/initialize";

                var requestData = new
                {
                    email = _email,
                    amount = amnt
                };

                string jsonContent = JsonSerializer.Serialize(requestData);

                var response = await _httpClient.PostAsync(apiUrl, new StringContent(jsonContent, Encoding.UTF8, "application/json"));

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var responseData = JsonSerializer.Deserialize<PaystackResponseVM>(content);

                    return responseData;

                }

                return new PaystackResponseVM()
                {
                   
                    message = "Unable to verify payment",
                };

            }
            catch (Exception ex)
            {
                var err = new PaystackResponseVM()
                {
                   
                    message = ex.Message,
                    data = null!
                };
                return err;
            }

        }

        public async Task<PaystackResponseVM> VerifyPayment(string reference)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(reference))
                {
                    throw new Exception("please provide a valid reference number");
                }

                //var client = _httpClient.CreateClient("Paystack");
                string apiUrl = $"transaction/verify/{reference}";

                var response = await _httpClient.GetAsync(apiUrl);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var responseData = JsonSerializer.Deserialize<PaystackResponseVM>(content);
                    return new PaystackResponseVM()
                    {
                        status = true,
                        message = "Payment Verified Successfully",
                    };
                }

                return new PaystackResponseVM()
                {
                    status = false,
                    message = "Unable to verify payment",
                };
            }
            catch (Exception ex)
            {

                var err = new PaystackResponseVM()
                {
                    status = false,
                    message = ex.Message,
                    data = null!
                };
                return err;
            }
        }
    }
}
