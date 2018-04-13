﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace easyfis.Entities
{
    public class TrnStockWithdrawalItem
    {
        public Int32 Id { get; set; }
        public Int32 SWId { get; set; }
        public Int32 ItemId { get; set; }
        public String ItemManualCode { get; set; }
        public String ItemSKUCode { get; set; }
        public String ItemDescription { get; set; }
        public Int32 ItemInventoryId { get; set; }
        public String ItemInventoryCode { get; set; }
        public String Particulars { get; set; }
        public Int32 UnitId { get; set; }
        public Decimal Quantity { get; set; }
        public Decimal Cost { get; set; }
        public Decimal Amount { get; set; }
        public Int32 BaseUnitId { get; set; }
        public Decimal BaseQuantity { get; set; }
        public Decimal BaseCost { get; set; }
    }
}