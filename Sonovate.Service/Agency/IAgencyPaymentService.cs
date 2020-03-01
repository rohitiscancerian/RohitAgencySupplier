using Sonovate.CodeTest.Domain;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sonovate.CodeTest.Service
{
    public interface IAgencyPaymentService
    {
        Task<List<BacsResult>> GetAgencyPayments(DateTime startDate, DateTime endDate);       
    }
}
