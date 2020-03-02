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
    public class TestPaymentRepository
    {
        private IPaymentsRepository _paymentsRepository;
        [TestInitialize]
        public void Setup()
        {
            _paymentsRepository = new PaymentsRepository();
        }

        [TestMethod]
        public  void ShouldReturnTwoInvoiceTransactions()
        {
            //Act
            var payments = _paymentsRepository.GetPaymentsBetweenDates(new DateTime(2019, 9, 11), new DateTime(2019, 9, 29));

            //Assert
            Assert.AreEqual(4, payments.Count);
        }

        [TestMethod]
        public void ShouldNotReturnAnyPayments()
        {           
            //Act
            var payments = _paymentsRepository.GetPaymentsBetweenDates(new DateTime(2020, 4, 1), new DateTime(2020, 4, 5));

            //Assert
            Assert.AreEqual(0, payments.Count);
        }
    }
}
