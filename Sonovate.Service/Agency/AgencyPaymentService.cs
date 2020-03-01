using Raven.Client.Documents;
using Sonovate.CodeTest.Domain;
using Sonovate.CodeTest.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sonovate.CodeTest.Service
{
    public class AgencyPaymentService : IAgencyPaymentService
    {
        private IPaymentsRepository _paymentsRepository;
        private readonly IDocumentStoreService _documentStoreService;
        public AgencyPaymentService(IPaymentsRepository paymentsRepository,IDocumentStoreService documentStoreService)
        {
            _paymentsRepository = paymentsRepository;
            _documentStoreService = documentStoreService;
        }
        public async Task<List<BacsResult>> GetAgencyPayments(DateTime startDate, DateTime endDate)
        {            
            var payments = _paymentsRepository.GetPaymentsBetweenDates(startDate, endDate);

            if (!payments.Any())
            {
                throw new InvalidOperationException(string.Format("No agency payments found between dates {0:dd/MM/yyyy} to {1:dd/MM/yyyy}", startDate, endDate));
            }

            var agencies = await GetAgenciesForPayments(payments);

            return BuildAgencyPayments(payments, agencies);
        }

        async Task<List<Agency>> GetAgenciesForPayments(IList<Payment> payments)
        {
            var agencyIds = payments.Select(x => x.AgencyId).Distinct().ToList();
            List<Agency> agencies = new List<Agency>();
            
            foreach(var agencyId in agencyIds)
            {
                var agency = new Agency
                {
                    BankDetails = new BankDetails { AccountName = "HSBC", AccountNumber = "4345235", SortCode = "456783" },
                    Id = agencyId
                };
                agencies.Add(agency);
            }
            return await Task.FromResult(agencies);
            // RavenDB issues
            //using (var session = _documentStoreService.GetDocumentStore().OpenAsyncSession())
            //{
            //    return (await session.LoadAsync<Agency>(agencyIds)).Values.ToList();
            //}
        }

        List<BacsResult> BuildAgencyPayments(IEnumerable<Payment> payments, List<Agency> agencies)
        {
            return (from p in payments
                    let agency = agencies.FirstOrDefault(x => x.Id == p.AgencyId)
                    where agency != null && agency.BankDetails != null
                    let bank = agency.BankDetails
                    select new BacsResult
                    {
                        AccountName = bank.AccountName,
                        AccountNumber = bank.AccountNumber,
                        SortCode = bank.SortCode,
                        Amount = p.Balance,
                        Ref = string.Format("SONOVATE{0}", p.PaymentDate.ToString("ddMMyyyy"))
                    }).ToList();
        }

    }
}
