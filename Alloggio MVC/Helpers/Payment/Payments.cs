using Iyzipay;
using Iyzipay.Model;
using Iyzipay.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Alloggio_MVC.Helpers.Payment
{
    public class Payments
    {
        public Iyzipay.Model.Payment payment()
        {
            Options options = new Options();
            options.ApiKey = "sandbox-ZuQLQoJiMAUQfwReWIZ8KGc9zD5SoBZL";
            options.SecretKey = "sandbox-r1pp562iddiTWuuZMBQouh5Kb2o9nQKY";
            options.BaseUrl = "https://sandbox-api.iyzipay.com";

            CreatePaymentRequest request = new CreatePaymentRequest();
            request.Locale = Locale.TR.ToString();
            request.ConversationId = "123456789";
            request.Price = "0.3";
            request.PaidPrice = "1.2";
            request.Currency = Currency.TRY.ToString();
            request.Installment = 1;
            request.BasketId = "B67832";
            request.PaymentChannel = PaymentChannel.WEB.ToString();
            request.PaymentGroup = PaymentGroup.PRODUCT.ToString();

            PaymentCard paymentCard = new PaymentCard();
            paymentCard.CardHolderName = "JZiya Mammadov";
            paymentCard.CardNumber = "5528790000000008";
            paymentCard.ExpireMonth = "12";
            paymentCard.ExpireYear = "2030";
            paymentCard.Cvc = "123";
            paymentCard.RegisterCard = 0;
            request.PaymentCard = paymentCard;

            Buyer buyer = new Buyer();
            buyer.Id = "BY789";
            buyer.Name = "Ziya";
            buyer.Surname = "Mammadov";
            buyer.GsmNumber = "+905350000000";
            buyer.Email = "ziya.memmedov@hotmail.com";
            buyer.IdentityNumber = "74300864791";
            buyer.LastLoginDate = "2015-10-05 12:43:35";
            buyer.RegistrationDate = "2013-04-21 15:12:09";
            buyer.RegistrationAddress = "Nidasul street Haci no: 6";
            buyer.Ip = "85.34.78.112";
            buyer.City = "Istanbul";
            buyer.Country = "Turkey";
            buyer.ZipCode = "34732";
            request.Buyer = buyer;

            Address shippingAddress = new Address();
            shippingAddress.ContactName = "Contact name";
            shippingAddress.City = "London";
            shippingAddress.Country = "England";
            shippingAddress.Description = "Nidakule Göztepe, Merdivenköy Mah. Bora Sok. No:1";
            shippingAddress.ZipCode = "34742";
            request.ShippingAddress = shippingAddress;

            Address billingAddress = new Address();
            billingAddress.ContactName = "Contact name";
            billingAddress.City = "Weekand";
            billingAddress.Country = "Monday";
            billingAddress.Description = "Nidakule Göztepe, Merdivenköy Mah. Bora Sok. No:1";
            billingAddress.ZipCode = "34742";
            request.BillingAddress = billingAddress;

            List<Iyzipay.Model.BasketItem> basketItems = new List<Iyzipay.Model.BasketItem>();
            Iyzipay.Model.BasketItem firstBasketItem = new Iyzipay.Model.BasketItem();
            firstBasketItem.Id = "BI101";
            firstBasketItem.Name = "Binocular";
            firstBasketItem.Category1 = "Collectibles";
            firstBasketItem.Category2 = "Accessories";
            firstBasketItem.ItemType = BasketItemType.PHYSICAL.ToString();
            firstBasketItem.Price = "0.3";
            basketItems.Add(firstBasketItem);

            request.BasketItems = basketItems;

            Iyzipay.Model.Payment payment = Iyzipay.Model.Payment.Create(request, options);

            return payment;
        }

    }
}
