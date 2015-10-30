﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace easyfis.Models
{
    public class TrnJournal
    {
        [Key]
        public Int32 Id { get; set; }
        public String JournalDate { get; set; }
        public Int32 BranchId { get; set; }
        public Int32 AccountId { get; set; }
        public Int32 ArticleId { get; set; }
        public String Particulars { get; set; }
        public Decimal DebitAmount { get; set; }
        public Decimal CreditAmount { get; set; }
        public Int32 ORId { get; set; }
        public Int32 CVId { get; set; }
        public Int32 JVId { get; set; }
        public Int32 RRId { get; set; }
        public Int32 SIId { get; set; }
        public Int32 INId { get; set; }
        public Int32 OTId { get; set; }
        public Int32 STId { get; set; }
        public String DocumentReference { get; set; }
        public Int32 APRRId { get; set; }
        public Int32 ARSIId { get; set; }

    }
}