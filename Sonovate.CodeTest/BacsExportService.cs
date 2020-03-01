using Microsoft.Extensions.DependencyInjection;
using Sonovate.CodeTest.Domain;
using Sonovate.CodeTest.Service;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sonovate.CodeTest
{
    public class BacsExportService
    {
        private IAgencyPaymentService _agencyPaymentService;
        private ISupplierPaymentService _supplierPaymentService;
        private IFileService _fileService;
        private static IServiceProvider _serviceProvider;
        public BacsExportService()
        {
            _serviceProvider = RegisterService.RegisterServices();
            _agencyPaymentService = _serviceProvider.GetService<IAgencyPaymentService>(); ;
            _supplierPaymentService = _serviceProvider.GetService<ISupplierPaymentService>(); 
            _fileService = _serviceProvider.GetService<IFileService>();            
        }        
        
        public async Task ExportZip(BacsExportType bacsExportType)
        {
            if (bacsExportType == BacsExportType.None)
            {
                const string invalidExportTypeMessage = "No export type provided.";
                throw new Exception(invalidExportTypeMessage);
            }

            var startDate = new DateTime(2019, 2, 1);
            var endDate = new DateTime(2020, 3, 1); ;

            try
            {
                List<BacsResult> payments;
                switch (bacsExportType)
                {
                    case BacsExportType.Agency:
                        if (Application.Settings["EnableAgencyPayments"].ToLower() == "true")
                        {
                            payments = await _agencyPaymentService.GetAgencyPayments(startDate, endDate);
                            _fileService.SaveBacsExportAsCSV(payments, bacsExportType);
                        }
                        break;
                    case BacsExportType.Supplier:
                        var supplierBacsExport = _supplierPaymentService.GetSupplierPayments(startDate, endDate);
                        _fileService.SaveBacsExportAsCSV(supplierBacsExport.SupplierPayment,bacsExportType);
                        break;
                    default:
                        throw new Exception("Invalid BACS Export Type.");
                }

            }
            catch (InvalidOperationException inOpEx)
            {
                throw new Exception(inOpEx.Message);
            }            
            finally
            {
                DisposeServices();
            }
        }

        private static void DisposeServices()
        {
            if (_serviceProvider == null)
            {
                return;
            }
            if (_serviceProvider is IDisposable)
            {
                ((IDisposable)_serviceProvider).Dispose();
            }
        }
    }
}