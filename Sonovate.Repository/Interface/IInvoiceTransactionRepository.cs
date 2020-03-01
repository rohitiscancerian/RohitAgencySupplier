using Sonovate.CodeTest.Domain;
using System;
using System.Collections.Generic;

namespace Sonovate.CodeTest.Repository
{
    public interface IInvoiceTransactionRepository
    {
        List<InvoiceTransaction> GetTransactionBetweenDates(DateTime startDate, DateTime endDate);
    }
}