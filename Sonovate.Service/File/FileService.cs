using CsvHelper;
using Sonovate.CodeTest.Domain;
using System.Collections.Generic;
using System.IO;

namespace Sonovate.CodeTest.Service
{
    public class FileService : IFileService 
    {
        public void SaveBacsExportAsCSV(IEnumerable<BacsBase> payments, BacsExportType type)
        {
            var fileName = string.Format("{0}_BACSExport.csv", type);

            using (var csv = new CsvWriter(new StreamWriter(new FileStream(fileName, FileMode.Create))))
            {
                csv.WriteRecords(payments);
            }
        }
    }
}
