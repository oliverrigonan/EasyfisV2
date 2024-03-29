﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Microsoft.AspNet.Identity;
using System.Diagnostics;

namespace easyfis.ModifiedApiControllers
{
    public class ApiTrnBankReconciliationController : ApiController
    {
        // ============
        // Data Context
        // ============
        private Data.easyfisdbDataContext db = new Data.easyfisdbDataContext();

        // ============================
        // Dropdown List - Bank (Field)
        // ============================
        [Authorize, HttpGet, Route("api/bankReconciliation/dropdown/list/bank")]
        public List<Entities.MstArticle> DropdownListBankReconciliationBank()
        {
            var depositoryBanks = from d in db.MstArticles.OrderBy(d => d.Article)
                                  where d.ArticleTypeId == 5
                                  && d.IsLocked == true
                                  select new Entities.MstArticle
                                  {
                                      Id = d.Id,
                                      Article = d.Article
                                  };

            return depositoryBanks.ToList();
        }

        // ==========================================
        // List Bank Reconciliation - Collection Line
        // ==========================================
        [Authorize, HttpGet, Route("api/bankReconciliation/collectionLine/list/{depositoryBankId}/{startDate}/{endDate}")]
        public List<Entities.TrnCollectionLine> ListBankReconciliationCollectionLine(String depositoryBankId, String startDate, String endDate)
        {
            var previousUnclearedCollectionLines = from d in db.TrnCollectionLines.OrderBy(d => d.TrnCollection.ORDate)
                                                   where d.DepositoryBankId == Convert.ToInt32(depositoryBankId)
                                                   && d.TrnCollection.ORDate < Convert.ToDateTime(startDate)
                                                   && d.Amount > 0
                                                   && d.IsClear == false
                                                   && d.TrnCollection.IsLocked == true
                                                   select new Entities.TrnCollectionLine
                                                   {
                                                       Id = d.Id,
                                                       ORNumber = d.TrnCollection.ORNumber,
                                                       ORDate = d.TrnCollection.ORDate.ToShortDateString(),
                                                       Customer = d.TrnCollection.MstArticle.Article,
                                                       PayType = d.MstPayType.PayType,
                                                       CheckNumber = d.CheckNumber,
                                                       CheckDate = d.CheckDate.ToShortDateString(),
                                                       Amount = d.Amount,
                                                       IsClear = d.IsClear
                                                   };

            var currentCollectionLines = from d in db.TrnCollectionLines.OrderBy(d => d.TrnCollection.ORDate)
                                         where d.DepositoryBankId == Convert.ToInt32(depositoryBankId)
                                         && d.TrnCollection.ORDate >= Convert.ToDateTime(startDate)
                                         && d.TrnCollection.ORDate <= Convert.ToDateTime(endDate)
                                         && d.Amount > 0
                                         && d.TrnCollection.IsLocked == true
                                         select new Entities.TrnCollectionLine
                                         {
                                             Id = d.Id,
                                             ORNumber = d.TrnCollection.ORNumber,
                                             ORDate = d.TrnCollection.ORDate.ToShortDateString(),
                                             Customer = d.TrnCollection.MstArticle.Article,
                                             PayType = d.MstPayType.PayType,
                                             CheckNumber = d.CheckNumber,
                                             CheckDate = d.CheckDate.ToShortDateString(),
                                             Amount = d.Amount,
                                             IsClear = d.IsClear
                                         };

            return previousUnclearedCollectionLines.Union(currentCollectionLines).ToList();
        }

        // =======================================
        // List Bank Reconciliation - Disbursement
        // =======================================
        [Authorize, HttpGet, Route("api/bankReconciliation/disbursement/list/{bankId}/{startDate}/{endDate}")]
        public List<Entities.TrnDisbursement> ListBankReconciliationDisbursement(String bankId, String startDate, String endDate)
        {
            var previousUnclearedDisbursements = from d in db.TrnDisbursements.OrderBy(d => d.CVDate)
                                                 where d.BankId == Convert.ToInt32(bankId)
                                                 && d.CVDate < Convert.ToDateTime(startDate)
                                                 && d.Amount > 0
                                                 && d.IsClear == false
                                                 && d.IsLocked == true
                                                 select new Entities.TrnDisbursement
                                                 {
                                                     Id = d.Id,
                                                     CVNumber = d.CVNumber,
                                                     CVDate = d.CVDate.ToShortDateString(),
                                                     Payee = d.Payee,
                                                     PayType = d.MstPayType.PayType,
                                                     CheckNumber = d.CheckNumber,
                                                     CheckDate = d.CheckDate.ToShortDateString(),
                                                     Amount = d.Amount,
                                                     IsClear = d.IsClear
                                                 };

            var currentDisbursements = from d in db.TrnDisbursements.OrderBy(d => d.CVDate)
                                       where d.BankId == Convert.ToInt32(bankId)
                                       && d.CVDate >= Convert.ToDateTime(startDate)
                                       && d.CVDate <= Convert.ToDateTime(endDate)
                                       && d.Amount > 0
                                       && d.IsLocked == true
                                       select new Entities.TrnDisbursement
                                       {
                                           Id = d.Id,
                                           CVNumber = d.CVNumber,
                                           CVDate = d.CVDate.ToShortDateString(),
                                           Payee = d.Payee,
                                           PayType = d.MstPayType.PayType,
                                           CheckNumber = d.CheckNumber,
                                           CheckDate = d.CheckDate.ToShortDateString(),
                                           Amount = d.Amount,
                                           IsClear = d.IsClear
                                       };

            return previousUnclearedDisbursements.Union(currentDisbursements).ToList();
        }

        // ===============================================
        // List Bank Reconciliation - Journal Voucher Line
        // ===============================================
        [Authorize, HttpGet, Route("api/bankReconciliation/journalVoucherLine/list/{articleId}/{startDate}/{endDate}")]
        public List<Entities.TrnJournalVoucherLine> ListBankReconciliationJournalVoucherLine(String articleId, String startDate, String endDate)
        {
            var previousJournalVoucherLines = from d in db.TrnJournalVoucherLines
                                              where d.ArticleId == Convert.ToInt32(articleId)
                                              && d.TrnJournalVoucher.JVDate < Convert.ToDateTime(startDate)
                                              && d.DebitAmount - d.CreditAmount != 0
                                              && d.IsClear == false
                                              && d.TrnJournalVoucher.IsLocked == true
                                              select new Entities.TrnJournalVoucherLine
                                              {
                                                  Id = d.Id,
                                                  JVNumber = d.TrnJournalVoucher.JVNumber,
                                                  JVDate = d.TrnJournalVoucher.JVDate.ToShortDateString(),
                                                  Particulars = d.Particulars,
                                                  DebitAmount = d.DebitAmount,
                                                  CreditAmount = d.CreditAmount,
                                                  BalanceAmount = d.DebitAmount - d.CreditAmount,
                                                  IsClear = d.IsClear
                                              };

            var currentJournalVoucherLines = from d in db.TrnJournalVoucherLines
                                             where d.ArticleId == Convert.ToInt32(articleId)
                                             && d.TrnJournalVoucher.JVDate >= Convert.ToDateTime(startDate)
                                             && d.TrnJournalVoucher.JVDate <= Convert.ToDateTime(endDate)
                                             && d.DebitAmount - d.CreditAmount != 0
                                             && d.TrnJournalVoucher.IsLocked == true
                                             select new Entities.TrnJournalVoucherLine
                                             {
                                                 Id = d.Id,
                                                 JVNumber = d.TrnJournalVoucher.JVNumber,
                                                 JVDate = d.TrnJournalVoucher.JVDate.ToShortDateString(),
                                                 Particulars = d.Particulars,
                                                 DebitAmount = d.DebitAmount,
                                                 CreditAmount = d.CreditAmount,
                                                 BalanceAmount = d.DebitAmount - d.CreditAmount,
                                                 IsClear = d.IsClear
                                             };

            return previousJournalVoucherLines.Union(currentJournalVoucherLines).ToList();
        }

        // ===========================================================
        // Update Bank Reconciliation Collection Line - Is Clear Field
        // ===========================================================
        [Authorize, HttpPut, Route("api/bankReconciliation/collectionLine/update/isClearField/{id}")]
        public HttpResponseMessage UpdateBankReconciliationCollectionLineIsClearField(String id)
        {
            try
            {
                var currentUser = from d in db.MstUsers
                                  where d.UserId == User.Identity.GetUserId()
                                  select d;

                if (currentUser.Any())
                {
                    var currentUserId = currentUser.FirstOrDefault().Id;

                    var userForms = from d in db.MstUserForms
                                    where d.UserId == currentUserId
                                    && d.SysForm.FormName.Equals("BankReconciliation")
                                    select d;

                    if (userForms.Any())
                    {
                        if (userForms.FirstOrDefault().CanEdit)
                        {
                            var collectionLines = from d in db.TrnCollectionLines
                                                  where d.Id == Convert.ToInt32(id)
                                                  && d.TrnCollection.IsLocked == true
                                                  select d;

                            if (collectionLines.Any())
                            {
                                var isClear = false;
                                var updateCollectionLine = collectionLines.FirstOrDefault();

                                if (!collectionLines.FirstOrDefault().IsClear)
                                {
                                    updateCollectionLine.IsClear = true;
                                    isClear = true;
                                }
                                else
                                {
                                    updateCollectionLine.IsClear = false;
                                }

                                db.SubmitChanges();

                                return Request.CreateResponse(HttpStatusCode.OK, isClear);
                            }
                            else
                            {
                                return Request.CreateResponse(HttpStatusCode.NotFound, "This collection line detail is no longer exist in the server.");
                            }
                        }
                        else
                        {
                            return Request.CreateResponse(HttpStatusCode.BadRequest, "Sorry. You have no rights to edit and update bank reconcilation in this bank reconcilation detail page.");
                        }
                    }
                    else
                    {
                        return Request.CreateResponse(HttpStatusCode.BadRequest, "Sorry. You have no access in this bank reconcilation detail page.");
                    }
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, "Theres no current user logged in.");
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, "Something's went wrong from the server.");
            }
        }

        // ========================================================
        // Update Bank Reconciliation Disbursement - Is Clear Field
        // ========================================================
        [Authorize, HttpPut, Route("api/bankReconciliation/disbursement/update/isClearField/{id}")]
        public HttpResponseMessage UpdateBankReconciliationDisbursementIsClearField(String id)
        {
            try
            {
                var currentUser = from d in db.MstUsers
                                  where d.UserId == User.Identity.GetUserId()
                                  select d;

                if (currentUser.Any())
                {
                    var currentUserId = currentUser.FirstOrDefault().Id;

                    var userForms = from d in db.MstUserForms
                                    where d.UserId == currentUserId
                                    && d.SysForm.FormName.Equals("BankReconciliation")
                                    select d;

                    if (userForms.Any())
                    {
                        if (userForms.FirstOrDefault().CanEdit)
                        {
                            var disbursements = from d in db.TrnDisbursements
                                                where d.Id == Convert.ToInt32(id)
                                                && d.IsLocked == true
                                                select d;

                            if (disbursements.Any())
                            {
                                var isClear = false;
                                var updateDisbursement = disbursements.FirstOrDefault();

                                if (!disbursements.FirstOrDefault().IsClear)
                                {
                                    updateDisbursement.IsClear = true;
                                    isClear = true;
                                }
                                else
                                {
                                    updateDisbursement.IsClear = false;
                                }

                                db.SubmitChanges();

                                return Request.CreateResponse(HttpStatusCode.OK, isClear);
                            }
                            else
                            {
                                return Request.CreateResponse(HttpStatusCode.NotFound, "This disbursement detail is no longer exist in the server.");
                            }
                        }
                        else
                        {
                            return Request.CreateResponse(HttpStatusCode.BadRequest, "Sorry. You have no rights to edit and update bank reconcilation in this bank reconcilation detail page.");
                        }
                    }
                    else
                    {
                        return Request.CreateResponse(HttpStatusCode.BadRequest, "Sorry. You have no access in this bank reconcilation detail page.");
                    }
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, "Theres no current user logged in.");
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, "Something's went wrong from the server.");
            }
        }

        // ================================================================
        // Update Bank Reconciliation Journal Voucher Line - Is Clear Field
        // ================================================================
        [Authorize, HttpPut, Route("api/bankReconciliation/journalVoucherLine/update/isClearField/{id}")]
        public HttpResponseMessage UpdateBankReconciliationJournalVoucherLineIsClearField(String id)
        {
            try
            {
                var currentUser = from d in db.MstUsers
                                  where d.UserId == User.Identity.GetUserId()
                                  select d;

                if (currentUser.Any())
                {
                    var currentUserId = currentUser.FirstOrDefault().Id;

                    var userForms = from d in db.MstUserForms
                                    where d.UserId == currentUserId
                                    && d.SysForm.FormName.Equals("BankReconciliation")
                                    select d;

                    if (userForms.Any())
                    {
                        if (userForms.FirstOrDefault().CanEdit)
                        {
                            var journalVoucherLines = from d in db.TrnJournalVoucherLines
                                                      where d.Id == Convert.ToInt32(id)
                                                      && d.TrnJournalVoucher.IsLocked == true
                                                      select d;

                            if (journalVoucherLines.Any())
                            {
                                var isClear = false;
                                var updateJournalVoucherLine = journalVoucherLines.FirstOrDefault();

                                if (!journalVoucherLines.FirstOrDefault().IsClear)
                                {
                                    updateJournalVoucherLine.IsClear = true;
                                    isClear = true;
                                }
                                else
                                {
                                    updateJournalVoucherLine.IsClear = false;
                                }

                                db.SubmitChanges();

                                return Request.CreateResponse(HttpStatusCode.OK, isClear);
                            }
                            else
                            {
                                return Request.CreateResponse(HttpStatusCode.NotFound, "This journal voucher line detail is no longer exist in the server.");
                            }
                        }
                        else
                        {
                            return Request.CreateResponse(HttpStatusCode.BadRequest, "Sorry. You have no rights to edit and update bank reconcilation in this bank reconcilation detail page.");
                        }
                    }
                    else
                    {
                        return Request.CreateResponse(HttpStatusCode.BadRequest, "Sorry. You have no access in this bank reconcilation detail page.");
                    }
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, "Theres no current user logged in.");
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, "Something's went wrong from the server.");
            }
        }
    }
}
