using Sonovate.CodeTest.Domain;
using Sonovate.CodeTest.Repository;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Sonovate.CodeTest.Service
{
    public class SupplierPaymentService : ISupplierPaymentService
    {
        private const string NOT_AVAILABLE = "NOT AVAILABLE";

        IInvoiceTransactionRepository _invoiceTransactionRepository;
        ICandidateRepository _candidateRepository;
        public SupplierPaymentService(IInvoiceTransactionRepository invoiceTransactionRepository,ICandidateRepository candidateRepository)
        {
            _invoiceTransactionRepository = invoiceTransactionRepository;
            _candidateRepository = candidateRepository;
        }         
        

        public SupplierBacsExport GetSupplierPayments(DateTime startDate, DateTime endDate)
        {
            var candidateInvoiceTransactions = _invoiceTransactionRepository.GetTransactionBetweenDates(startDate, endDate);

            if (!candidateInvoiceTransactions.Any())
            {
                throw new InvalidOperationException(string.Format("No supplier invoice transactions found between dates {0} to {1}", startDate, endDate));
            }

            var candidateBacsExport = CreateCandidateBacxExportFromSupplierPayments(candidateInvoiceTransactions);

            return candidateBacsExport;
        }

        SupplierBacsExport CreateCandidateBacxExportFromSupplierPayments(IList<InvoiceTransaction> supplierPayments)
        {
            var candidateBacsExport = new SupplierBacsExport
            {
                SupplierPayment = new List<SupplierBacs>()
            };

            candidateBacsExport.SupplierPayment = BuildSupplierPayments(supplierPayments);

            return candidateBacsExport;
        }

        List<SupplierBacs> BuildSupplierPayments(IEnumerable<InvoiceTransaction> invoiceTransactions)
        {
            var results = new List<SupplierBacs>();

            var transactionsByCandidateAndInvoiceId = invoiceTransactions.GroupBy(transaction => new
            {
                transaction.InvoiceId,
                transaction.SupplierId
            });

            foreach (var transactionGroup in transactionsByCandidateAndInvoiceId)
            {
                Candidate candidate;
                try
                {
                    candidate = _candidateRepository.GetById(transactionGroup.Key.SupplierId);
                }
                catch (Exception)
                {

                    throw new InvalidOperationException(string.Format("Could not load candidate with Id {0}",
                        transactionGroup.Key.SupplierId));
                }              

                var supplierBacs = new SupplierBacs();

                var bankDetails = candidate.BankDetails;

                SetSupplierBacs(supplierBacs, bankDetails, transactionGroup);

                results.Add(supplierBacs);
            }

            return results;
        }
        void SetSupplierBacs(SupplierBacs supplierBacs, BankDetails bankDetails, IGrouping<object, InvoiceTransaction> transactionGroup)
        {
            supplierBacs.AccountName = bankDetails.AccountName;
            supplierBacs.AccountNumber = bankDetails.AccountNumber;
            supplierBacs.SortCode = bankDetails.SortCode;
            supplierBacs.PaymentAmount = transactionGroup.Sum(invoiceTransaction => invoiceTransaction.Gross);
            supplierBacs.InvoiceReference = string.IsNullOrEmpty(transactionGroup.First().InvoiceRef)
                ? NOT_AVAILABLE
                : transactionGroup.First().InvoiceRef;
            supplierBacs.PaymentReference = string.Format("SONOVATE{0}",
                transactionGroup.First().InvoiceDate.GetValueOrDefault().ToString("ddMMyyyy"));
        }
    }
}
