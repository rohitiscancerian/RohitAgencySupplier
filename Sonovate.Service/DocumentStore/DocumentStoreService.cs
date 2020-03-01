using Raven.Client.Documents;

namespace Sonovate.CodeTest.Service
{
    public class DocumentStoreService : IDocumentStoreService
    {
        private readonly IDocumentStore documentStore;
        public DocumentStoreService()
        {
            documentStore = new DocumentStore { Urls = new[] { "http://localhost" }, Database = "Export" };
            documentStore.Initialize();
        }

        public IDocumentStore GetDocumentStore()
        {
            return documentStore;
        }
    }
}
