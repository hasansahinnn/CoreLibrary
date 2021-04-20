using Iyzipay;
using Iyzipay.Model;
using Iyzipay.Request;
using Serilog;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Application
{
    public static class IyzicoPaymentService
    {
        public static string ipAddress = "10.10.10.10";
        public static Options options = new Options()
        {
            ApiKey = "",
            SecretKey = "",
            BaseUrl = "https://api.iyzipay.com"
        };
        public static (string,string) CreatePayment( string orderNo, PaymentType paymentType, double price,string firstname,string lastname,string phone,string email)
        {
            //var ipAddress = Request.HttpContext.Connection.RemoteIpAddress.ToString();
            string paymentId = "";
            var buyer = new Buyer
            {
                Id = orderNo,
                Name = firstname,
                Surname = lastname,
                GsmNumber = phone,
                Email = email ?? "admin@alicinhazir.com",
                IdentityNumber = "11111111111",
                RegistrationAddress = "adres",
                Ip = ipAddress,
                City = "Istanbul",
                Country = "Turkey",
            };
            var basketItems = new List<BasketItem>();
            basketItems.Add(new BasketItem
            {
                Id = paymentType.Id,
                Name = "Premium Hesap",
                Category1 = "Premium Hesap",
                ItemType = "VIRTUAL",
                Price = price.ToString().Replace(',', '.')
            });
            var shippingAddress = new Address
            {
                ContactName = firstname+" "+lastname,
                City = "Istanbul",
                Country = "Turkey",
                Description = "Address"
            };

            var request = new CreateCheckoutFormInitializeRequest
            {
                Locale = "TR",
                ConversationId = orderNo,
                Price = price.ToString().Replace(',', '.'),
                PaidPrice = price.ToString().Replace(',', '.'),
                BasketId = orderNo.ToString(),
                PaymentGroup = "PRODUCT",
                CallbackUrl = paymentType.CallbackUrl,
                BasketItems = basketItems,
                ShippingAddress = shippingAddress,
                BillingAddress = shippingAddress,
                Buyer = buyer
            };

            var checkoutFormInitialize = CheckoutFormInitialize.Create(request, options);
            paymentId = checkoutFormInitialize.Token;
            if (checkoutFormInitialize.Status.Equals(Status.SUCCESS.ToString()))
            {
                return (paymentId,checkoutFormInitialize.PaymentPageUrl);
                //return (paymentId,checkoutFormInitialize.PaymentPageUrl);
            }
            else
            {
                return ("", checkoutFormInitialize.ErrorMessage);
            }
        }
        public static (bool, string) ConfirmPayment(string token)
        {
            try
            {
                var request = new RetrieveCheckoutFormRequest
                {
                    Token = token
                };

                var checkoutForm = CheckoutForm.Retrieve(request, options);

                if (checkoutForm.Status.Equals(Status.SUCCESS.ToString()))
                {
                    return (true, checkoutForm.PaymentId);
                }
                else
                {
                    return (false, checkoutForm.ErrorMessage);
                }
            }
            catch (Exception _)
            {
                Log.Error(_.Message);
                return (false, "");
            }
        }

        public static (bool, string) CancelPayment(string paymentId)
        {
            CreateCancelRequest request = new CreateCancelRequest();
            request.Locale = Locale.TR.ToString();
            request.PaymentId = paymentId;
            request.Ip = "10.10.10.10";
            Cancel refund = Cancel.Create(request, options);
            if (refund.Status == "failure")
            { 
               return (false, refund.ErrorMessage);
            }
             return (true, "");
        }
    }
    public static class PaymentTypes
    {
        public static readonly PaymentType PremiumPayment = new PaymentType("1", "/api/purchase/PremiumPaymentResult");

    }

    public class PaymentType
    {
        public string Id { get; set; }
        public string CallbackUrl { get; set; }

        public PaymentType(string ıd, string callbackUrl)
        {
            Id = ıd;
            CallbackUrl = callbackUrl;
        }
    }
}
