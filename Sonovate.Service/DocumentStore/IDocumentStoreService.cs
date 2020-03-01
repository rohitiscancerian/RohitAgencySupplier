using Raven.Client.Documents;

namespace Sonovate.CodeTest.Service
{
    public interface IDocumentStoreService
    {
        IDocumentStore GetDocumentStore();
    }
}
