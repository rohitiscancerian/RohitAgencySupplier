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
    public class TestAgencyPaymentService
    {
        private Mock<IPaymentsRepository> _paymentRepository;
        private Mock<IDocumentStoreService> _documentStoreService;
        [TestInitialize]
        public void Setup()
        {
            _paymentRepository = new Mock<IPaymentsRepository>();
            _paymentRepository.Setup(x => x.GetPaymentsBetweenDates(It.IsAny<DateTime>(), It.IsAny<DateTime>())).Returns((DateTime stDate, DateTime endDate) => { return GetTestPayments().Where(payment => payment.PaymentDate >= stDate && payment.PaymentDate <= endDate).ToList();});
            _documentStoreService = new Mock<IDocumentStoreService>();
            _documentStoreService.Setup(x => x.GetDocumentStore()).Returns(GetDocumentStore());
        }

        private IDocumentStore GetDocumentStore()
        {
            var documentStore = new DocumentStore { Urls = new[] { "http://localhost/" }, Database = "Export" };
            documentStore.Initialize();
            return documentStore;
        }

        private IList<Payment> GetTestPayments()
        {
           return new List<Payment>() {
                new Payment { AgencyId = "Agency 1", Balance = 20000.00m, PaymentDate = new DateTime(2019, 9, 01) },
                new Payment { AgencyId = "Agency 2", Balance = 7500.00m, PaymentDate = new DateTime(2019, 10, 16) },
                new Payment { AgencyId = "Agency 3", Balance = 960.25m, PaymentDate = new DateTime(2019, 11, 20) },
                new Payment { AgencyId = "Agency 4", Balance = 14000.50m, PaymentDate = new DateTime(2020, 1, 11) },
                new Payment { AgencyId = "Agency 5", Balance = 70500.00m, PaymentDate = new DateTime(2020, 2, 28) }
            };
        }
        [TestMethod]
        public async Task ShouldReturnTwoAgencyPayments()
        {
            //Arrange
            var mockAgencyPaymentService = new AgencyPaymentService(_paymentRepository.Object,_documentStoreService.Object);
            
            //Act
            var agencyPayments =  await mockAgencyPaymentService.GetAgencyPayments(new DateTime(2020, 1, 11), new DateTime(2020, 2, 28));

            //Assert
            Assert.AreEqual(2,agencyPayments.Count);
        }

        [TestMethod]
        public async Task ShouldReturnThreeAgencyPayments()
        {
            //Arrange
            var mockAgencyPaymentService = new AgencyPaymentService(_paymentRepository.Object, _documentStoreService.Object);
            //Act
            var agencyPayments = await mockAgencyPaymentService.GetAgencyPayments(new DateTime(2019, 11, 20), new DateTime(2020, 2, 28));
            //Assert
            Assert.AreEqual(3,agencyPayments.Count);
        }

        [TestMethod]
        public async Task ShouldReturnFiveAgencyPayments()
        {
            //Arrange
            var mockAgencyPaymentService = new AgencyPaymentService(_paymentRepository.Object, _documentStoreService.Object);
            //Act
            var agencyPayments = await mockAgencyPaymentService.GetAgencyPayments(new DateTime(2019, 03, 1), new DateTime(2020, 03, 01));
            //Assert
            Assert.AreEqual(5, agencyPayments.Count);
        }

        [TestMethod]
        public async Task ShouldReturnInvalidOperationException()
        {
            //Arrange
            var mockAgencyPaymentService = new AgencyPaymentService(_paymentRepository.Object, _documentStoreService.Object);
            //Act and Assert
            await Assert.ThrowsExceptionAsync<InvalidOperationException>(async () => await mockAgencyPaymentService.GetAgencyPayments(new DateTime(2020, 03, 01), new DateTime(2020, 03, 02)));
        }
    }
}
