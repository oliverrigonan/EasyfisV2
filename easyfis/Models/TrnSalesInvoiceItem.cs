﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace easyfis.Models
{
    public class TrnSalesInvoiceItem
    {
        [Key]

        public Int32 Id { get; set; }
        public Int32 SIId { get; set; }
        public String SI { get; set; }
        public String SIDate { get; set; }
        public Int32 ItemId { get; set; }
        public String ItemCode { get; set; }
        public String Item { get; set; }
        public Int32? ItemInventoryId { get; set; }
        public String ItemInventory { get; set; }
        public String ItemCategory { get; set; }
        public String Customer { get; set; }
        public String Particulars { get; set; }
        public Int32 UnitId { get; set; }
        public String Unit { get; set; }
        public Decimal Quantity { get; set; }
        public Decimal Price { get; set; }
        public Int32 DiscountId { get; set; }
        public String Discount { get; set; }
        public Decimal DiscountRate { get; set; }
        public Decimal DiscountAmount { get; set; }
        public Decimal NetPrice { get; set; }
        public Decimal Amount { get; set; }
        public Int32 VATId { get; set; }
        public String VAT { get; set; }
        public Decimal VATPercentage { get; set; }
        public Decimal VATAmount { get; set; }
        public Int32 BaseUnitId { get; set; }
        public String BaseUnit { get; set; }
        public Decimal BaseQuantity { get; set; }
        public Decimal BasePrice { get; set; }
        public String SalesItemTimeStampDate { get; set; }
        public String SalesItemTimeStampHour { get; set; }
        public String SalesItemTimeStampMinutes { get; set; }
        public String SalesItemTimeStampSeconds { get; set; }
        public String SalesItemTimeStamp { get; set; }
        public DateTime SalesItemTimeStampDateTime { get; set; }
        public Int32 SalesAccountId { get; set; }
        public Int32 CostAccountId { get; set; }
        public Int32 AccountId { get; set; }
        public Decimal Cost { get; set; }
        public String Branch { get; set; }
        public String SoldBy { get; set; }
    }
}