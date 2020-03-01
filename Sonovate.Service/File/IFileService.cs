using Sonovate.CodeTest.Domain;
using System.Collections.Generic;

namespace Sonovate.CodeTest.Service
{
    public interface IFileService
    {
        void SaveBacsExportAsCSV(IEnumerable<BacsBase> payments, BacsExportType type);
    }
}
