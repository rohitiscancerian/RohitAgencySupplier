using Sonovate.CodeTest.Domain;
using System;
using System.Collections.Generic;

namespace Sonovate.CodeTest.Repository
{
    public interface IPaymentsRepository
    {
        IList<Payment> GetPaymentsBetweenDates(DateTime start, DateTime end);
    }
}