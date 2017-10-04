﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace easyfis.Entities
{
    public class MstArticleInventory
    {
        public Int32 Id { get; set; }
        public Int32 BranchId { get; set; }
        public Int32 ArticleId { get; set; }
        public String InventoryCode { get; set; }
        public Decimal Quantity { get; set; }
        public Decimal Cost { get; set; }
        public Decimal Amount { get; set; }
        public String Particulars { get; set; }
    }
}