using Microsoft.AspNetCore.Http;
using Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface IMomoService
    {
        Task<MomoCreatePaymentResponseModel> CreatePaymentAsync(OrderInfoModel model, string? returnUrl);
        MomoExecuteResponseModel PaymentExecuteAsync(IQueryCollection collection);
    }
}
