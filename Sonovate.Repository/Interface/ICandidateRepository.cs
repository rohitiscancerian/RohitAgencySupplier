using Sonovate.CodeTest.Domain;

namespace Sonovate.CodeTest.Repository
{
    public interface ICandidateRepository
    {
        Candidate GetById(global::System.String supplierId);
    }
}