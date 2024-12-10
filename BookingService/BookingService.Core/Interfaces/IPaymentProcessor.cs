using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingService.Core.Interfaces
{
    public interface IPaymentProcessor
    {
        Task<bool> ProcessPaymentAsync(Guid bookingId, decimal amount);
    }
}
