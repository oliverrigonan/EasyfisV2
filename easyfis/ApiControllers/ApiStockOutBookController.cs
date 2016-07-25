﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Microsoft.AspNet.Identity;
using System.Net.Http;
using System.Web.Http;

namespace easyfis.ApiControllers
{
    public class ApiStockOutBookController : ApiController
    {
        private Data.easyfisdbDataContext db = new Data.easyfisdbDataContext();


        // list account
        [Authorize]
        [HttpGet]
        [Route("api/StockOutBook/list/{startDate}/{endDate}/{companyId}")]
        public List<Models.TrnJournal> listStockOutBook(String startDate, String endDate, String companyId)
        {

            var journalsDocumentReferences = from d in db.TrnJournals
                                             where d.JournalDate >= Convert.ToDateTime(startDate)
                                             && d.JournalDate <= Convert.ToDateTime(endDate)
                                             && d.MstBranch.CompanyId == Convert.ToInt32(companyId)
                                              && d.OTId != null
                                             select new Models.TrnJournal
                                             {
                                                 DocumentReference = d.DocumentReference,
                                                 AccountCode = d.MstAccount.AccountCode,
                                                 Account = d.MstAccount.Account,
                                                 Article = d.MstArticle.Article,
                                                 Particulars = d.Particulars,
                                                 DebitAmount = d.DebitAmount,
                                                 CreditAmount = d.CreditAmount,
                                                 Balance = d.DebitAmount - d.CreditAmount
                                             };

            return journalsDocumentReferences.ToList();
        }
    }
}

