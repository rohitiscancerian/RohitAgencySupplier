using Microsoft.Extensions.DependencyInjection;
using Sonovate.CodeTest.Repository;
using Sonovate.CodeTest.Service;
using System;

namespace Sonovate.CodeTest
{
    public static class RegisterService
    {
        public static IServiceProvider RegisterServices()
        {
            var collection = new ServiceCollection();
            collection.AddScoped<IPaymentsRepository, PaymentsRepository>();
            collection.AddScoped<ICandidateRepository, CandidateRepository>();
            collection.AddScoped<IInvoiceTransactionRepository, InvoiceTransactionRepository>();
            collection.AddScoped<IDocumentStoreService, DocumentStoreService>();
            collection.AddScoped<IFileService, FileService>();
            collection.AddScoped<IAgencyPaymentService, AgencyPaymentService>();
            collection.AddScoped<ISupplierPaymentService, SupplierPaymentService>();

            return collection.BuildServiceProvider();
        }
    }
}
