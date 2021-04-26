namespace OrangeTransactionsExtractor
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Request
    {
        public string RequestId { get; set; }

        [StringLength(30)]
        public string MeterNumber { get; set; }

        [Required]
        [StringLength(50)]
        public string MessageIDDateTimeRequest { get; set; }

        public DateTime RequestDateTime { get; set; }

        public bool? StatusIndicator { get; set; }

        public string ResponseStatus { get; set; }

        [Required]
        [StringLength(25)]
        public string TerminalId { get; set; }

        public decimal? TransactionValue { get; set; }

        [Required]
        public string XMLRequest { get; set; }

        public string XMLResponse { get; set; }

        public DateTime? ResponseDateTime { get; set; }

        [StringLength(25)]
        public string MessageIDDateTimeResponse { get; set; }

        [StringLength(25)]
        public string ReceiptNumber { get; set; }

        [StringLength(25)]
        public string Token { get; set; }

        [StringLength(25)]
        public string ReversePaymentId { get; set; }

        [StringLength(128)]
        public string Client_Client_Id { get; set; }

        public int MethodType { get; set; }

        public float? Units { get; set; }

        public int OriginalUniqueNumber { get; set; }

        public int UniqueNumber { get; set; }

        public virtual Client Client { get; set; }
    }
}
