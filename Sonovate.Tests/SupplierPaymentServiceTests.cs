using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Raven.Client.Documents;
using Sonovate.CodeTest.Domain;
using Sonovate.CodeTest.Repository;
using Sonovate.CodeTest.Service;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Sonovate.Tests
{
    [TestClass]
    public class TestSupplierPaymentService
    {
        private Mock<IInvoiceTransactionRepository> _invoiceTransactionRepository;
        private Mock<ICandidateRepository> _candidateRepository;
        [TestInitialize]
        public void Setup()
        {
            _invoiceTransactionRepository = new Mock<IInvoiceTransactionRepository>();
            _invoiceTransactionRepository.Setup(x => x.GetTransactionBetweenDates(It.IsAny<DateTime>(), It.IsAny<DateTime>())).Returns((DateTime stDate, DateTime endDate) => { return GetTestInvoiceTransactions().Where(transaction => transaction.InvoiceDate >= stDate && transaction.InvoiceDate <= endDate).ToList();});
            _candidateRepository = new Mock<ICandidateRepository>();
            _candidateRepository.Setup(x => x.GetById(It.IsAny<string>())).Returns((string supplierId) => { return GetTestCandidates()[supplierId];});
        }
        private IList<InvoiceTransaction> GetTestInvoiceTransactions()
        {
           return new List<InvoiceTransaction>
            {
                new InvoiceTransaction { InvoiceDate = new DateTime(2019, 4, 26), InvoiceId = "0001", SupplierId = "Supplier 1", InvoiceRef = "Ref0001", Gross = 10000.00m},
                new InvoiceTransaction { InvoiceDate = new DateTime(2019, 4, 14), InvoiceId = "0002", SupplierId = "Supplier 2", InvoiceRef = "Ref0002", Gross = 7300.00m},
                new InvoiceTransaction { InvoiceDate = new DateTime(2019, 4, 17), InvoiceId = "0003", SupplierId = "Supplier 3", InvoiceRef = "Ref0003", Gross = 2000.60m},
                new InvoiceTransaction { InvoiceDate = new DateTime(2019, 4, 1), InvoiceId = "0004", SupplierId = "Supplier 4", InvoiceRef = "Ref0004", Gross = 9800.00m},
                new InvoiceTransaction { InvoiceDate = new DateTime(2019, 4, 5), InvoiceId = "0005", SupplierId = "Supplier 5", InvoiceRef = "Ref0005", Gross = 4000.60m},
                new InvoiceTransaction { InvoiceDate = new DateTime(2020, 2, 5), InvoiceId = "0005", SupplierId = "Supplier 6", InvoiceRef = "Ref0005", Gross = 4000.60m},
            };
        }

        private Dictionary<string, Candidate> GetTestCandidates()
        {
            return new Dictionary<string, Candidate>
            {
                { "Supplier 1", new Candidate { BankDetails = new BankDetails{ AccountName = "Account 1", AccountNumber = "00000001", SortCode = "00-00-01"}}},
                { "Supplier 2", new Candidate { BankDetails = new BankDetails{ AccountName = "Account 2", AccountNumber = "00000001", SortCode = "00-00-02"}}},
                { "Supplier 3", new Candidate { BankDetails = new BankDetails{ AccountName = "Account 3", AccountNumber = "00000001", SortCode = "00-00-03"}}},
                { "Supplier 4", new Candidate { BankDetails = new BankDetails{ AccountName = "Account 4", AccountNumber = "00000001", SortCode = "00-00-04"}}},
                { "Supplier 5", new Candidate { BankDetails = new BankDetails{ AccountName = "Account 5", AccountNumber = "00000001", SortCode = "00-00-05"}}},
            };
        }
        [TestMethod]
        public void ShouldReturnTwoSupplierPayments()
        {
            //Arrange
            var mockSupplierPaymentService = new SupplierPaymentService(_invoiceTransactionRepository.Object,_candidateRepository.Object);
            
            //Act
            var supplierBacs =  mockSupplierPaymentService.GetSupplierPayments(new DateTime(2019, 4, 1), new DateTime(2019, 4, 5));

            //Assert
            Assert.AreEqual(2, supplierBacs.SupplierPayment.Count);
        }

        [TestMethod]
        public void ShouldReturnFourSupplierPayments()
        {
            //Arrange
            var mockSupplierPaymentService = new SupplierPaymentService(_invoiceTransactionRepository.Object, _candidateRepository.Object);

            //Act
            var supplierBacs = mockSupplierPaymentService.GetSupplierPayments(new DateTime(2019, 4, 1), new DateTime(2019, 4, 25));

            //Assert
            Assert.AreEqual(4, supplierBacs.SupplierPayment.Count);
        }

        [TestMethod]
        public void ShouldReturnFiveSupplierPayments()
        {
            //Arrange
            var mockSupplierPaymentService = new SupplierPaymentService(_invoiceTransactionRepository.Object, _candidateRepository.Object);

            //Act
            var supplierBacs = mockSupplierPaymentService.GetSupplierPayments(new DateTime(2019, 4, 1), new DateTime(2019, 4, 26));

            //Assert
            Assert.AreEqual(5, supplierBacs.SupplierPayment.Count);
        }

        [TestMethod]
        public void ShouldReturnInvalidOperationExceptionDueToNoInvoice()
        {
            //Arrange
            var mockSupplierPaymentService = new SupplierPaymentService(_invoiceTransactionRepository.Object, _candidateRepository.Object);
            //Act and Assert
            Assert.ThrowsException<InvalidOperationException>(() =>  mockSupplierPaymentService.GetSupplierPayments(new DateTime(2019, 12, 5), new DateTime(2020, 2, 4)));
        }

        [TestMethod]
        public void ShouldReturnInvalidOperationExceptionDueToNoMachingCandidateSupplier()
        {
            //Arrange
            var mockSupplierPaymentService = new SupplierPaymentService(_invoiceTransactionRepository.Object, _candidateRepository.Object);
            //Act and Assert
            Assert.ThrowsException<InvalidOperationException>(() => mockSupplierPaymentService.GetSupplierPayments(new DateTime(2019, 12, 5),new DateTime(2020, 2, 5)));
        }

    }
}
