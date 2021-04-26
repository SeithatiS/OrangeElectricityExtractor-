namespace BotswanaLifeTransactionsExtractor
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class RequestType
    {
        public string RequestTypeId { get; set; }

        [StringLength(30)]
        public string Name { get; set; }

        public int Code { get; set; }
    }
}
