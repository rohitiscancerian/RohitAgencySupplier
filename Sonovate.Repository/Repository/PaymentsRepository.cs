﻿using System;
using System.Collections.Generic;
using System.Linq;
using Sonovate.CodeTest.Domain;


namespace Sonovate.CodeTest.Repository
{
    public class PaymentsRepository : IPaymentsRepository
    {
        public IList<Payment> GetPaymentsBetweenDates(DateTime start, DateTime end)
        {
            var payments = new List<Payment>()
            {
                new Payment { AgencyId = "Agency 1", Balance = 20000.00m, PaymentDate = new DateTime(2019, 9, 01)},
                new Payment { AgencyId = "Agency 2", Balance = 7500.00m, PaymentDate = new DateTime(2019, 9, 16)},
                new Payment { AgencyId = "Agency 3", Balance = 960.25m, PaymentDate = new DateTime(2019, 9, 20)},
                new Payment { AgencyId = "Agency 4", Balance = 14000.50m, PaymentDate = new DateTime(2019, 9, 11)},
                new Payment { AgencyId = "Agency 5", Balance = 70500.00m, PaymentDate = new DateTime(2019, 9, 29)},
            };

            return payments.Where(payment => payment.PaymentDate >= start && payment.PaymentDate <= end).ToList();
        }
    }
}