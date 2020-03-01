using Sonovate.CodeTest.Domain;
using System;

namespace Sonovate.CodeTest.Service
{
    public interface ISupplierPaymentService
    {
        SupplierBacsExport GetSupplierPayments(DateTime startDate, DateTime endDate);        
    }
}
