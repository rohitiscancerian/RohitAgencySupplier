namespace Sonovate.CodeTest.Domain
{
    public class SupplierBacs : BacsBase
    {        
        public decimal PaymentAmount { get; set; }
        public string InvoiceReference { get; set; }
        public string PaymentReference { get; set; }
    }
}