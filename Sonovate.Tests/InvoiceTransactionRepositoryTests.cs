using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Raven.Client.Documents;
using Sonovate.CodeTest.Domain;
using Sonovate.CodeTest.Repository;
using Sonovate.CodeTest.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sonovate.Tests
{
    [TestClass]
    public class TestInvoiceTransactionRepository
    {
        private IInvoiceTransactionRepository _invoiceTransactionRepository;
        [TestInitialize]
        public void Setup()
        {
            _invoiceTransactionRepository = new InvoiceTransactionRepository();
        }
        [TestMethod]
        public  void ShouldReturnTwoInvoiceTransactions()
        {            
            //Act
            var invoiceTransactions = _invoiceTransactionRepository.GetTransactionBetweenDates(new DateTime(2019, 4, 1), new DateTime(2019, 4, 5));

            //Assert
            Assert.AreEqual(2, invoiceTransactions.Count);
        }

        [TestMethod]
        public void ShouldNotReturnAnyInvoiceTransactions()
        {

            //Act
            var invoiceTransactions = _invoiceTransactionRepository.GetTransactionBetweenDates(new DateTime(2020, 4, 1), new DateTime(2020, 4, 5));

            //Assert
            Assert.AreEqual(0, invoiceTransactions.Count);
        }
    }
}
