﻿using Orkidea.Bretano.WebMiddle.FrontEnd.Business;
using Orkidea.Bretano.WebMiddle.FrontEnd.Entities;
using Orkidea.Bretano.WebMiddle.FrontEnd.Models;
using Orkidea.Bretano.WebMiddle.FrontEnd.WebMiddleBackEnd;
using Orkidea.Framework.Security;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Security.Principal;
using System.ServiceModel;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;

namespace Orkidea.Bretano.WebMiddle.FrontEnd.Controllers
{
    public class SalesOrderController : Controller
    {
        WSSAPClient backEnd = new WSSAPClient();

        public ActionResult Index()
        {
            #region User identification
            IIdentity context = HttpContext.User.Identity;
            string userId = "";
            bool admin = false;
            bool customerCreator = false;
            bool purchaseOrderCreator = false;
            int companyId = 0;
            string userName = "";
            AppConnData appConnData = new AppConnData();

            if (context.IsAuthenticated)
            {

                System.Web.Security.FormsIdentity ci = (System.Web.Security.FormsIdentity)HttpContext.User.Identity;
                string[] userRole = ci.Ticket.UserData.Split('|');
                userId = userRole[0];
                admin = int.Parse(userRole[1]) == 1 ? true : false;
                customerCreator = int.Parse(userRole[2]) == 1 ? true : false;
                purchaseOrderCreator = int.Parse(userRole[3]) == 1 ? true : false;
                companyId = int.Parse(userRole[4]);
                userName = ci.Name;
                appConnData = GetAppConnData(companyId);
            }
            #endregion

            List<ORDR> orders = BizSalesOrderDraft.GetPendingList(userId, companyId)
                //.Where(x => x.draftDM.Equals(false) && x.draftLC.Equals(false))
                .ToList();
            List<ORDRViewModel> listOrders = new List<ORDRViewModel>();

            //List<string> customers = orders.Select(x => x.cardCode).Distinct().ToList();
            //List<GenericBusinessPartner> customerList = backEnd.GetBusinessPartnersByIds(CardType.Customer, customers, appConnData);

            foreach (ORDR item in orders)
            {
                ORDRViewModel order = new ORDRViewModel()
                {
                    id = HexSerialization.StringToHex(item.id.ToString()),
                    cardCode = item.cardCode,
                    cardName = !string.IsNullOrEmpty(item.cardName) ? item.cardName : "",
                    docDate = item.docDate
                };

                listOrders.Add(order);
            }



            return View(listOrders);
        }

        //
        // GET: /SalesOrder/
        [Authorize]
        public ActionResult itemIndex(string id)
        {
            ViewBag.id = id;
            return View();
        }

        [Authorize]
        public ActionResult Create()
        {
            #region User identification
            IIdentity context = HttpContext.User.Identity;
            string userId = "";
            bool admin = false;
            bool customerCreator = false;
            bool purchaseOrderCreator = false;
            int companyId = 0;
            string userName = "";
            AppConnData appConnData = new AppConnData();

            if (context.IsAuthenticated)
            {

                System.Web.Security.FormsIdentity ci = (System.Web.Security.FormsIdentity)HttpContext.User.Identity;
                string[] userRole = ci.Ticket.UserData.Split('|');
                userId = userRole[0];
                admin = int.Parse(userRole[1]) == 1 ? true : false;
                customerCreator = int.Parse(userRole[2]) == 1 ? true : false;
                purchaseOrderCreator = int.Parse(userRole[3]) == 1 ? true : false;
                companyId = int.Parse(userRole[4]);
                userName = ci.Name;
                appConnData = GetAppConnData(companyId);
            }
            #endregion

            List<CompanyParameter> companyParameters = BizCompanyParameter.GetList(companyId).ToList();
            string defaulSeries = companyParameters.Where(x => x.idParameter.Equals(8)).Select(x => x.value).FirstOrDefault();
            string ShowOnlyCustomerGroupNum = companyParameters.Where(x => x.idParameter.Equals(9)).Select(x => x.value).FirstOrDefault();

            ViewBag.defaulSeries = string.IsNullOrEmpty(defaulSeries) ? "" : defaulSeries;
            ViewBag.ShowOnlyCustomerGroupNum = string.IsNullOrEmpty(ShowOnlyCustomerGroupNum) ? "No" : ShowOnlyCustomerGroupNum;

            return View();
        }

        [Authorize]
        [HttpPost]
        public ActionResult Create(ORDR order)
        {
            #region User identification
            IIdentity context = HttpContext.User.Identity;
            string userId = "";
            bool admin = false;
            bool customerCreator = false;
            bool purchaseOrderCreator = false;
            int companyId = 0;
            string userName = "";
            int slpCode = 0;
            AppConnData appConnData = new AppConnData();

            if (context.IsAuthenticated)
            {

                System.Web.Security.FormsIdentity ci = (System.Web.Security.FormsIdentity)HttpContext.User.Identity;
                string[] userRole = ci.Ticket.UserData.Split('|');

                userId = userRole[0];
                admin = int.Parse(userRole[1]) == 1 ? true : false;
                customerCreator = int.Parse(userRole[2]) == 1 ? true : false;
                purchaseOrderCreator = int.Parse(userRole[3]) == 1 ? true : false;
                companyId = int.Parse(userRole[4]);
                slpCode = int.Parse(userRole[5]);
                userName = ci.Name;
                appConnData = GetAppConnData(companyId);
            }
            #endregion

            List<CompanyParameter> companyParameters = BizCompanyParameter.GetList(companyId).ToList();
            string defaulSeries = companyParameters.Where(x => x.idParameter.Equals(8)).Select(x => x.value).FirstOrDefault();

            if (!string.IsNullOrEmpty(defaulSeries))
            {
                List<DocumentSeries> docSeries = backEnd.GetDocumentSeriesList(SapDocumentType.SalesOrder, appConnData);
                int serie = docSeries.Where(x => x.seriesName == defaulSeries).Select(x => x.series).FirstOrDefault();

                if (serie != null)
                    order.series = serie;

            }

            order.uOrkUsuarioWeb = userId;
            order.idCompania = companyId;
            order.slpCode = slpCode;
            order.uLatitud = !string.IsNullOrEmpty(order.uLatitud) ? order.uLatitud : "4.690449";
            order.uLongitud = !string.IsNullOrEmpty(order.uLongitud) ? order.uLongitud : "-74.0515331";
            int orderNum = BizSalesOrderDraft.Add(order);

            return RedirectToAction("AddLine", new { id = HexSerialization.StringToHex(orderNum.ToString()) });
        }

        [Authorize]
        public ActionResult AddLine(string id)
        {
            #region User identification
            IIdentity context = HttpContext.User.Identity;
            bool admin = false;
            bool customerCreator = false;
            bool purchaseOrderCreator = false;
            int companyId = 0;
            string userName = "";
            AppConnData appConnData = new AppConnData();

            if (context.IsAuthenticated)
            {

                System.Web.Security.FormsIdentity ci = (System.Web.Security.FormsIdentity)HttpContext.User.Identity;
                string[] userRole = ci.Ticket.UserData.Split('|');

                admin = int.Parse(userRole[1]) == 1 ? true : false;
                customerCreator = int.Parse(userRole[2]) == 1 ? true : false;
                purchaseOrderCreator = int.Parse(userRole[3]) == 1 ? true : false;
                companyId = int.Parse(userRole[4]);
                userName = ci.Name;
                appConnData = GetAppConnData(companyId);
            }
            #endregion

            #region Company Parameters
            Company company = BizCompany.GetSingle(companyId);

            string realId = HexSerialization.HexToString(id);
            ORDR ordr = BizSalesOrderDraft.GetSingle(int.Parse(realId));

            BusinessPartner customer = backEnd.GetBusinessPartner(ordr.cardCode, appConnData);
            bool creditStatus = backEnd.GetBusinessPartnerCreditStatus(ordr.cardCode, appConnData);

            ViewBag.cardCode = ordr.cardCode;
            ViewBag.cardName = customer.cardName;
            ViewBag.docDate = ordr.docDate.ToString("yyyy-MM-dd");
            ViewBag.docDueDate = ordr.docDueDate.ToString("yyyy-MM-dd");
            ViewBag.taxDate = ordr.taxDate.ToString("yyyy-MM-dd");
            ViewBag.uCssComentarios = ordr.uCssComentarios;
            ViewBag.listNum = customer.listNum;
            ViewBag.orderId = realId;
            ViewBag.id = id;
            ViewBag.editaPrecio = company.editaPrecio;


            List<CompanyParameter> companyParameters = BizCompanyParameter.GetList(companyId).ToList();
            string defaulSeries = companyParameters.Where(x => x.idParameter.Equals(8)).Select(x => x.value).FirstOrDefault();
            string ShowOnlyItemWarehouse = companyParameters.Where(x => x.idParameter.Equals(10)).Select(x => x.value).FirstOrDefault();
            string ShowOnlyItemTaxcode = companyParameters.Where(x => x.idParameter.Equals(11)).Select(x => x.value).FirstOrDefault();
            string ShowOcrCode = companyParameters.Where(x => x.idParameter.Equals(12)).Select(x => x.value).FirstOrDefault();
            string showEnvaseDevolutivo = companyParameters.Where(x => x.idParameter.Equals(14)).Select(x => x.value).FirstOrDefault();
            string MaxOverdraftAuthorization = companyParameters.Where(x => x.idParameter.Equals(17)).Select(x => x.value).FirstOrDefault();

            ViewBag.defaulSeries = string.IsNullOrEmpty(defaulSeries) ? "" : defaulSeries;

            if (ShowOnlyItemWarehouse == "Si")
                ViewBag.ShowOnlyItemWarehouse = true;
            else
                ViewBag.ShowOnlyItemWarehouse = false;

            if (ShowOnlyItemTaxcode == "Si")
                ViewBag.ShowOnlyItemTaxcode = true;
            else
                ViewBag.ShowOnlyItemTaxcode = false;

            if (ShowOcrCode == "Si")
                ViewBag.ShowOcrCode = true;
            else
                ViewBag.ShowOcrCode = false;

            if (showEnvaseDevolutivo == "Si")
                ViewBag.showEnvaseDevolutivo = true;
            else
                ViewBag.showEnvaseDevolutivo = false;
            #endregion

            #region Validate Credit Rules
            List<RDR1> items = BizSalesOrderDraft.GetLinesList(int.Parse(realId)).ToList();

            decimal totalOrder = 0;
            decimal totalIva = 0;

            foreach (RDR1 item in items)
            {
                totalOrder += item.quantity * item.price;
                totalIva += ((item.quantity * item.price) * (item.taxRate == null ? 0 : (decimal)item.taxRate)) / 100;
            }

            //Overstep credits            
            if (customer.groupNum != null)
            {
                if (customer.groupNum != -1 && customer.creditLine == 0)
                    ViewBag.overStepCreditLine = false;
                else if (customer.groupNum != -1 && customer.creditLine > 0)
                {
                    if (MaxOverdraftAuthorization != null)
                    {
                        double balance = (double)customer.balance;
                        double dNotesBal = (double)customer.dNotesBal;
                        double ordersBal = (double)customer.ordersBal;
                        double avalaible = (double)customer.creditLine - (balance + dNotesBal + ordersBal);
                        double maxOverdraftRate = double.Parse(MaxOverdraftAuthorization);
                        totalOrder = totalOrder + totalIva;

                        if ((double)totalOrder >= (avalaible + (avalaible * (maxOverdraftRate / 100))))
                            ViewBag.overStepCreditLine = true;
                        else
                            ViewBag.overStepCreditLine = false;
                    }
                    else
                        ViewBag.overStepCreditLine = false;
                }
            }
            else
                ViewBag.overStepCreditLine = false;

            //Delay credits 
            if (creditStatus)
                ViewBag.delayCredits = true;

            #endregion

            return View();
        }

        [HttpPost]
        [Authorize]
        public ActionResult AddLine(string id, RDR1 line)
        {
            #region User identification
            IIdentity context = HttpContext.User.Identity;
            bool admin = false;
            bool customerCreator = false;
            bool purchaseOrderCreator = false;
            int companyId = 0;
            string userName = "";
            AppConnData appConnData = new AppConnData();

            if (context.IsAuthenticated)
            {

                System.Web.Security.FormsIdentity ci = (System.Web.Security.FormsIdentity)HttpContext.User.Identity;
                string[] userRole = ci.Ticket.UserData.Split('|');

                admin = int.Parse(userRole[1]) == 1 ? true : false;
                customerCreator = int.Parse(userRole[2]) == 1 ? true : false;
                purchaseOrderCreator = int.Parse(userRole[3]) == 1 ? true : false;
                companyId = int.Parse(userRole[4]);
                userName = ci.Name;
                appConnData = GetAppConnData(companyId);
            }
            #endregion

            List<CompanyParameter> companyParameters = BizCompanyParameter.GetList(companyId).ToList();

            try
            {
                string realId = HexSerialization.HexToString(id);
                line.orderId = int.Parse(realId);

                #region parameters management
                string ShowOnlyItemWarehouse = companyParameters.Where(x => x.idParameter.Equals(10)).Select(x => x.value).FirstOrDefault();
                string ShowOnlyItemTaxcode = companyParameters.Where(x => x.idParameter.Equals(11)).Select(x => x.value).FirstOrDefault();
                string ShowOcrCode = companyParameters.Where(x => x.idParameter.Equals(12)).Select(x => x.value).FirstOrDefault();
                string DefaultOcrCode = companyParameters.Where(x => x.idParameter.Equals(13)).Select(x => x.value).FirstOrDefault();
                string showEnvaseDevolutivo = companyParameters.Where(x => x.idParameter.Equals(14)).Select(x => x.value).FirstOrDefault();
                string UseOnlyItemEnvaseDevolutivo = companyParameters.Where(x => x.idParameter.Equals(15)).Select(x => x.value).FirstOrDefault();

                Item item = backEnd.GetItem(line.itemCode, appConnData);

                if (ShowOnlyItemWarehouse == "Si")
                {
                    if (!string.IsNullOrEmpty(item.DefaultWarehouse))
                        line.whsCode = item.DefaultWarehouse;
                    else
                        throw new Exception("No se puede pedir el articulo porque no tiene asociada una bodega");

                }

                if (ShowOnlyItemTaxcode == "Si")
                {
                    if (!string.IsNullOrEmpty(item.TaxCodeAR))
                    {
                        SalesTaxCode tax = backEnd.GetSingleTaxCode(item.TaxCodeAR, appConnData);

                        line.taxCode = item.TaxCodeAR;
                        line.taxRate = (decimal)tax.rate;
                    }
                    else
                        throw new Exception("No se puede pedir el articulo porque no tiene codigo de impuestos");
                }

                if (ShowOcrCode == "No")
                {
                    if (!string.IsNullOrEmpty(DefaultOcrCode))
                    {
                        List<SapDistributionRule> sapDistributionRulesList = backEnd.GetDistributionRulesList(appConnData);

                        string ocrCode = sapDistributionRulesList.Where(x => x.prcName == DefaultOcrCode).Select(x => x.prcCode).FirstOrDefault();

                        if (!string.IsNullOrEmpty(ocrCode))
                            line.ocrCode = ocrCode;
                        else
                            throw new Exception("No se puede pedir el articulo porque no se ha parametrizado una norma de reparto");
                    }
                    else
                        throw new Exception("No se puede pedir el articulo porque no se ha parametrizado una norma de reparto");
                }

                if (!string.IsNullOrEmpty(UseOnlyItemEnvaseDevolutivo))
                {
                    if (UseOnlyItemEnvaseDevolutivo == "Si")
                        line.uCssEnvaseDevol = item.userDefinedFields[0].value;
                    else
                        if (showEnvaseDevolutivo == "No")
                            line.uCssEnvaseDevol = "NO";
                }

                #endregion

                BizSalesOrderDraft.AddLine(line);
                ViewBag.orderId = id;
            }
            #region Exception management
            catch (DbUpdateException e)
            {
                #region Model management
                Company company = BizCompany.GetSingle(companyId);

                string realId = HexSerialization.HexToString(id);
                ORDR ordr = BizSalesOrderDraft.GetSingle(int.Parse(realId));

                BusinessPartner customer = backEnd.GetBusinessPartner(ordr.cardCode, appConnData);


                ViewBag.serie = backEnd.GetDocumentSeriesSingle(ordr.series, appConnData).seriesName;
                ViewBag.cardCode = ordr.cardCode;
                ViewBag.cardName = customer.cardName;
                ViewBag.docDate = ordr.docDate.ToString("yyyy-MM-dd");
                ViewBag.docDueDate = ordr.docDueDate.ToString("yyyy-MM-dd");
                ViewBag.taxDate = ordr.taxDate.ToString("yyyy-MM-dd");
                ViewBag.uCssComentarios = ordr.uCssComentarios;
                ViewBag.listNum = customer.listNum;
                ViewBag.orderId = realId;
                ViewBag.id = id;
                ViewBag.editaPrecio = company.editaPrecio;

                string defaulSeries = companyParameters.Where(x => x.idParameter.Equals(8)).Select(x => x.value).FirstOrDefault();

                ViewBag.defaulSeries = string.IsNullOrEmpty(defaulSeries) ? "" : defaulSeries;
                #endregion

                SqlException s = e.InnerException.InnerException as SqlException;
                if (s != null && s.Number == 2627)
                {
                    ViewBag.message = string.Format("Este pedido ya tiene registrado este Item en la bodega seleccionada.");
                }
                else
                {
                    ViewBag.message = string.Format("Se presentó un error al intentar añadir el item al pedido. - {0}", s.Message);
                }

                return View(line);
            }

            catch (SqlException ex)
            {
                #region Model management
                Company company = BizCompany.GetSingle(companyId);

                string realId = HexSerialization.HexToString(id);
                ORDR ordr = BizSalesOrderDraft.GetSingle(int.Parse(realId));

                BusinessPartner customer = backEnd.GetBusinessPartner(ordr.cardCode, appConnData);


                ViewBag.serie = backEnd.GetDocumentSeriesSingle(ordr.series, appConnData).seriesName;
                ViewBag.cardCode = ordr.cardCode;
                ViewBag.cardName = customer.cardName;
                ViewBag.docDate = ordr.docDate.ToString("yyyy-MM-dd");
                ViewBag.docDueDate = ordr.docDueDate.ToString("yyyy-MM-dd");
                ViewBag.taxDate = ordr.taxDate.ToString("yyyy-MM-dd");
                ViewBag.uCssComentarios = ordr.uCssComentarios;
                ViewBag.listNum = customer.listNum;
                ViewBag.orderId = realId;
                ViewBag.id = id;
                ViewBag.editaPrecio = company.editaPrecio;

                string defaulSeries = companyParameters.Where(x => x.idParameter.Equals(8)).Select(x => x.value).FirstOrDefault();

                ViewBag.defaulSeries = string.IsNullOrEmpty(defaulSeries) ? "" : defaulSeries;
                #endregion

                if (ex.Number == 2627)
                {
                    ViewBag.message = string.Format("Este pedido ya tiene registrado este Item en la bodega seleccionada.");
                }
                else
                {
                    ViewBag.message = string.Format("Se presentó un error al intentar añadir el item al pedido. - {0}", ex.Message);
                }

                return View(line);
            }
            catch (Exception ex)
            {
                #region Model management
                Company company = BizCompany.GetSingle(companyId);

                string realId = HexSerialization.HexToString(id);
                ORDR ordr = BizSalesOrderDraft.GetSingle(int.Parse(realId));

                BusinessPartner customer = backEnd.GetBusinessPartner(ordr.cardCode, appConnData);


                ViewBag.serie = backEnd.GetDocumentSeriesSingle(ordr.series, appConnData).seriesName;
                ViewBag.cardCode = ordr.cardCode;
                ViewBag.cardName = customer.cardName;
                ViewBag.docDate = ordr.docDate.ToString("yyyy-MM-dd");
                ViewBag.docDueDate = ordr.docDueDate.ToString("yyyy-MM-dd");
                ViewBag.taxDate = ordr.taxDate.ToString("yyyy-MM-dd");
                ViewBag.uCssComentarios = ordr.uCssComentarios;
                ViewBag.listNum = customer.listNum;
                ViewBag.orderId = realId;
                ViewBag.id = id;
                ViewBag.editaPrecio = company.editaPrecio;

                string defaulSeries = companyParameters.Where(x => x.idParameter.Equals(8)).Select(x => x.value).FirstOrDefault();

                ViewBag.defaulSeries = string.IsNullOrEmpty(defaulSeries) ? "" : defaulSeries;
                #endregion

                ViewBag.message = string.Format("Se presentó un error al intentar añadir el item al pedido. - {0}", ex.Message);
                return View(line);
            }
            #endregion

            return RedirectToAction("AddLine", new { id = id });
        }

        [Authorize]
        public ActionResult ViewOrder(string id)
        {
            #region User identification
            IIdentity context = HttpContext.User.Identity;
            string userId = "";
            bool admin = false;
            bool customerCreator = false;
            bool purchaseOrderCreator = false;
            bool orderApprover = false;
            int companyId = 0;
            string userName = "";
            AppConnData appConnData = new AppConnData();

            if (context.IsAuthenticated)
            {

                System.Web.Security.FormsIdentity ci = (System.Web.Security.FormsIdentity)HttpContext.User.Identity;
                string[] userRole = ci.Ticket.UserData.Split('|');
                userId = userRole[0];
                admin = int.Parse(userRole[1]) == 1 ? true : false;
                customerCreator = int.Parse(userRole[2]) == 1 ? true : false;
                purchaseOrderCreator = int.Parse(userRole[3]) == 1 ? true : false;
                companyId = int.Parse(userRole[4]);
                userName = ci.Name;
                appConnData = GetAppConnData(companyId);
                orderApprover = int.Parse(userRole[7]) == 1 ? true : false;
            }
            #endregion

            string realId = HexSerialization.HexToString(id);

            #region Order header            
            ORDR ordr = BizSalesOrderDraft.GetSingle(int.Parse(realId));

            BusinessPartner customer = backEnd.GetBusinessPartner(ordr.cardCode, appConnData);
            
            ViewBag.cardCode = ordr.cardCode;
            ViewBag.cardName = customer.cardName;
            ViewBag.docDate = ordr.docDate.ToString("yyyy-MM-dd");
            ViewBag.docDueDate = ordr.docDueDate.ToString("yyyy-MM-dd");
            ViewBag.taxDate = ordr.taxDate.ToString("yyyy-MM-dd");
            ViewBag.uCssComentarios = ordr.uCssComentarios;
            ViewBag.listNum = customer.listNum;
            ViewBag.orderId = realId;
            ViewBag.id = id;

            ViewBag.orderApprover = orderApprover;
            #endregion            

            #region order detail
            List<RDR1> items = BizSalesOrderDraft.GetLinesList(int.Parse(realId)).ToList();

            decimal totalOrder = 0;
            decimal totalIva = 0;

            foreach (RDR1 item in items)
            {
                totalOrder += item.quantity * item.price;
                totalIva += ((item.quantity * item.price) * (item.taxRate == null ? 0 : (decimal)item.taxRate)) / 100;
            }

            ViewBag.totalOrder = totalOrder;
            ViewBag.totalIva = totalIva;
            ViewBag.total = totalIva + totalOrder;
            ViewBag.id = id;
            #endregion

            return View(items);
        }

        [Authorize]
        public ActionResult GetOrderLines(string id)
        {
            string realId = HexSerialization.HexToString(id);
            List<RDR1> items = BizSalesOrderDraft.GetLinesList(int.Parse(realId)).ToList();

            decimal totalOrder = 0;
            decimal totalIva = 0;

            foreach (RDR1 item in items)
            {
                totalOrder += item.quantity * item.price;
                totalIva += ((item.quantity * item.price) * (item.taxRate == null ? 0 : (decimal)item.taxRate)) / 100;
            }

            ViewBag.totalOrder = totalOrder;
            ViewBag.totalIva = totalIva;
            ViewBag.id = id;

            return View(items);
        }

        [Authorize]
        public ActionResult Remove(string id)
        {

            string realId = HexSerialization.HexToString(id);
            ORDR ordr = BizSalesOrderDraft.GetSingle(int.Parse(realId));

            BizSalesOrderDraft.Delete(ordr);

            return RedirectToAction("Index");
        }

        [Authorize]
        public ActionResult RemoveLine(string id)
        {

            string[] realId = id.Split('|');

            BizSalesOrderDraft.DeleteLine(new RDR1() { orderId = int.Parse(HexSerialization.HexToString(realId[0])), itemCode = realId[1] });

            return RedirectToAction("AddLine", new { id = realId[0] });
        }

        [Authorize]
        public ActionResult Finish(string id)
        {
            try
            {
                #region User identification
                IIdentity context = HttpContext.User.Identity;
                int user = 0;
                bool admin = false;
                bool customerCreator = false;
                bool purchaseOrderCreator = false;
                int companyId = 0;
                string userName = "";
                int slpCode = 0;
                AppConnData appConnData = new AppConnData();

                if (context.IsAuthenticated)
                {

                    System.Web.Security.FormsIdentity ci = (System.Web.Security.FormsIdentity)HttpContext.User.Identity;
                    string[] userRole = ci.Ticket.UserData.Split('|');
                    user = int.Parse(userRole[0]);
                    admin = int.Parse(userRole[1]) == 1 ? true : false;
                    customerCreator = int.Parse(userRole[2]) == 1 ? true : false;
                    purchaseOrderCreator = int.Parse(userRole[3]) == 1 ? true : false;
                    companyId = int.Parse(userRole[4]);
                    slpCode = int.Parse(userRole[5]);
                    userName = ci.Name;
                    appConnData = GetAppConnData(companyId);
                }
                #endregion

                string realId = HexSerialization.HexToString(id);
                ORDR ordr = BizSalesOrderDraft.GetSingle(int.Parse(realId));

                if (ordr.docEntry != null)
                    throw new Exception("Este pedido ya fué registrado en SAP");

                List<RDR1> lines = BizSalesOrderDraft.GetLinesList(int.Parse(realId)).ToList();

                MarketingDocument document = new MarketingDocument()
                {
                    cardCode = ordr.cardCode,
                    serie = ordr.series,
                    docDate = ordr.docDate,
                    docDueDate = ordr.docDueDate,
                    taxDate = ordr.taxDate,
                    shipToCode = ordr.shipToCode,
                    payToCode = ordr.payToCode,
                    groupNum = ordr.groupNum,
                    lines = new List<MarketingDocumentLine>(),
                    userDefinedFields = new List<UserDefinedField>()
                };

                if (slpCode > 0)
                    document.slpCode = slpCode;

                document.userDefinedFields.Add(new UserDefinedField()
                {
                    name = "U_CSS_COMENTARIOS",
                    type = UdfType.Text,
                    value = ordr.uCssComentarios
                });

                document.userDefinedFields.Add(new UserDefinedField()
                {
                    name = "U_orkWebDocument",
                    type = UdfType.Text,
                    value = ordr.id.ToString() 
                });

                foreach (RDR1 item in lines)
                {

                    MarketingDocumentLine line = new MarketingDocumentLine()
                    {
                        itemCode = item.itemCode,
                        quantity = (double)item.quantity,
                        whsCode = item.whsCode,
                        taxCode = item.taxCode,
                        ocrCode = item.ocrCode,
                        price = (double)item.price,
                        batchNumbers = new List<BatchNumber>(),
                        serialNumbers = new List<SerialNumber>(),
                        userDefinedFields = new List<UserDefinedField>()
                    };

                    line.userDefinedFields.Add(new UserDefinedField()
                    {
                        name = "U_CSS_ENVASEDEVOL",
                        type = UdfType.Alphanumeric,
                        value = item.uCssEnvaseDevol
                    });

                    document.lines.Add(line);
                }

                if (userName.ToLower() != "root")
                {
                    WebUserCompany wuc = BizWebUserCompany.GetSingle(user, companyId);
                    if (wuc.slpCode != 0)
                        document.slpCode = wuc.slpCode;
                }


                document = backEnd.AddSalesOrder(document, appConnData);
                document.docNum = backEnd.GetOrderNum(document.docEntry, appConnData);
                ViewBag.colorMensaje = "success";
                ViewBag.mensaje = "Orden de venta creada con éxito";
                ViewBag.docEntry = string.Format("Se creó la orden no {0}. Numero interno de documento {1}", document.docNum, document.docEntry);

                ordr.docEntry = document.docEntry;
                BizSalesOrderDraft.Update(ordr);
            }
            catch (FaultException<DataAccessFault> ex)
            {
                ViewBag.colorMensaje = "danger";
                ViewBag.mensaje = "No se pudo crear la Orden de venta";
                ViewBag.docEntry = string.Format("Codigo {0} error:{1} {2}", ex.Code, ex.Detail.Description, ex.Message);
            }

            catch (Exception ex)
            {
                ViewBag.colorMensaje = "danger";
                ViewBag.mensaje = "Atención:";
                ViewBag.docEntry = string.Format(" - {0}", ex.Message);
            }
            return View();
        }

        [Authorize]
        public ActionResult Draft(string id)
        {
            try
            {
                string[] inParams = id.Split('|');
                string[] crIssues = inParams[1].Split('-');

                string realId = HexSerialization.HexToString(inParams[0]);
                ORDR ordr = BizSalesOrderDraft.GetSingle(int.Parse(realId));

                if (crIssues.Length == 1)
                    if (crIssues[0] == "LC")
                        ordr.draftLC = true;
                    else
                        ordr.draftDM = true;
                else if (crIssues.Length == 2)
                {
                    ordr.draftLC = true;
                    ordr.draftDM = true;
                }

                BizSalesOrderDraft.Update(ordr);
            }
            catch (FaultException<DataAccessFault> ex)
            {
                ViewBag.colorMensaje = "danger";
                ViewBag.mensaje = "No se pudo crear la Orden de venta";
                ViewBag.docEntry = string.Format("Codigo {0} error:{1} {2}", ex.Code, ex.Detail.Description, ex.Message);
            }

            catch (Exception ex)
            {
                ViewBag.colorMensaje = "danger";
                ViewBag.mensaje = "Atención:";
                ViewBag.docEntry = string.Format(" - {0}", ex.Message);
            }
            return View();
        }

        [Authorize]
        public ActionResult Search()
        {
            return View();
        }

        [Authorize]
        public ActionResult SearchOrders(string id)
        {
            ViewBag.data = id;

            return View();
        }

        [Authorize]
        public ActionResult Details(string id)
        {
            #region User identification
            IIdentity context = HttpContext.User.Identity;
            int user = 0;
            bool admin = false;
            bool customerCreator = false;
            bool purchaseOrderCreator = false;
            int companyId = 0;
            string userName = "";
            AppConnData appConnData = new AppConnData();

            if (context.IsAuthenticated)
            {

                System.Web.Security.FormsIdentity ci = (System.Web.Security.FormsIdentity)HttpContext.User.Identity;
                string[] userRole = ci.Ticket.UserData.Split('|');
                user = int.Parse(userRole[0]);
                admin = int.Parse(userRole[1]) == 1 ? true : false;
                customerCreator = int.Parse(userRole[2]) == 1 ? true : false;
                purchaseOrderCreator = int.Parse(userRole[3]) == 1 ? true : false;
                companyId = int.Parse(userRole[4]);
                userName = ci.Name;
                appConnData = GetAppConnData(companyId);
            }
            #endregion

            LightMarketingDocument order = backEnd.GetSingleOrder(id, appConnData);

            double total = 0;
            foreach (LightMarketingDocumentLine item in order.lines)
                total += item.quantity * item.price;

            CultureInfo culture = new CultureInfo("es-co");
            ViewBag.total = total.ToString("N", culture);
            return View(order);
        }

        [Authorize]
        public ActionResult Delete(int id)
        {
            #region User identification
            IIdentity context = HttpContext.User.Identity;
            int user = 0;
            bool admin = false;
            bool customerCreator = false;
            bool purchaseOrderCreator = false;
            int companyId = 0;
            string userName = "";
            AppConnData appConnData = new AppConnData();

            if (context.IsAuthenticated)
            {

                System.Web.Security.FormsIdentity ci = (System.Web.Security.FormsIdentity)HttpContext.User.Identity;
                string[] userRole = ci.Ticket.UserData.Split('|');
                user = int.Parse(userRole[0]);
                admin = int.Parse(userRole[1]) == 1 ? true : false;
                customerCreator = int.Parse(userRole[2]) == 1 ? true : false;
                purchaseOrderCreator = int.Parse(userRole[3]) == 1 ? true : false;
                companyId = int.Parse(userRole[4]);
                userName = ci.Name;
                appConnData = GetAppConnData(companyId);
            }
            #endregion

            try
            {
                backEnd.CancelOrder(id, appConnData);
                ViewBag.colorMensaje = "success";
                ViewBag.mensaje = "Cancelación de pedidos";
                ViewBag.docEntry = string.Format("el pedido fue cancelado");
            }
            catch (FaultException<DataAccessFault> ex)
            {
                ViewBag.colorMensaje = "danger";
                ViewBag.mensaje = "Cancelación de pedidos";
                ViewBag.docEntry = string.Format(" No se pudo cancelar el pedido. Codigo {0} error:{1} {2}", ex.Code, ex.Detail.Description, ex.Message);
            }

            return View();
        }

        [Authorize]
        public ActionResult Georeport()
        {
            return View();
        }

        [Authorize]
        public ActionResult ShowGeoReport(string id)
        {
            ViewBag.str = id;

            return View();
        }

        [Authorize]
        public ActionResult ItemStock()
        {
            return View();
        }

        [Authorize]
        public ActionResult AuthorizationReport()
        {
            #region User identification
            IIdentity context = HttpContext.User.Identity;
            string userId = "";
            bool admin = false;
            bool customerCreator = false;
            bool purchaseOrderCreator = false;
            bool orderApprover = false;
            int companyId = 0;
            string userName = "";
            AppConnData appConnData = new AppConnData();

            if (context.IsAuthenticated)
            {

                System.Web.Security.FormsIdentity ci = (System.Web.Security.FormsIdentity)HttpContext.User.Identity;
                string[] userRole = ci.Ticket.UserData.Split('|');
                userId = userRole[0];
                admin = int.Parse(userRole[1]) == 1 ? true : false;
                customerCreator = int.Parse(userRole[2]) == 1 ? true : false;
                purchaseOrderCreator = int.Parse(userRole[3]) == 1 ? true : false;
                companyId = int.Parse(userRole[4]);
                userName = ci.Name;
                appConnData = GetAppConnData(companyId);
                orderApprover = int.Parse(userRole[7]) == 1 ? true : false;
            }
            #endregion

            List<ORDR> orders = new List<ORDR>();

            if (orderApprover)
            {
                orders = BizSalesOrderDraft.GetPendingList(companyId).Where(x => x.draftDM.Equals(true) || x.draftLC.Equals(true)).ToList();
            }
            else
            {
                orders = BizSalesOrderDraft.GetPendingList(userId, companyId).Where(x => x.draftDM.Equals(true) || x.draftLC.Equals(true)).ToList();
            }

            List<ORDRViewModel> listOrders = new List<ORDRViewModel>();

            //List<string> customers = orders.Select(x => x.cardCode).Distinct().ToList();
            //List<GenericBusinessPartner> customerList = backEnd.GetBusinessPartnersByIds(CardType.Customer, customers, appConnData);

            foreach (ORDR item in orders)
            {
                ORDRViewModel order = new ORDRViewModel()
                {
                    id = HexSerialization.StringToHex(item.id.ToString()),
                    cardCode = item.cardCode,
                    cardName = !string.IsNullOrEmpty(item.cardName) ? item.cardName : "",
                    docDate = item.docDate,
                    //comment = item.draftDM.Equals(true) ? "Dias de mora": "Sobrecupo"
                };
                if (item.draftDM == null)
                    item.draftDM = false;

                if (item.draftLC == null)
                    item.draftLC = false;

                if ((bool)item.draftDM && (bool)item.draftLC)
                    order.comment = "Dias de mora y Sobrecupo";
                else if ((bool)item.draftDM && !(bool)item.draftLC)
                    order.comment = "Dias de mora";
                else if (!(bool)item.draftDM && (bool)item.draftLC)
                    order.comment = "Sobrecupo";

                listOrders.Add(order);
            }

            ViewBag.orderApprover = orderApprover;

            return View(listOrders);
        }

        [Authorize]
        public ActionResult ShowAuthorizationReport(string id)
        {
            ViewBag.str = id;

            return View();
        }

        [Authorize]
        //[OutputCache(Duration = int.MaxValue, VaryByParam = "none")]
        public JsonResult AsyncSeriesList()
        {
            #region User identification
            IIdentity context = HttpContext.User.Identity;
            bool admin = false;
            bool customerCreator = false;
            bool purchaseOrderCreator = false;
            int companyId = 0;
            string userName = "";
            AppConnData appConnData = new AppConnData();

            if (context.IsAuthenticated)
            {

                System.Web.Security.FormsIdentity ci = (System.Web.Security.FormsIdentity)HttpContext.User.Identity;
                string[] userRole = ci.Ticket.UserData.Split('|');

                admin = int.Parse(userRole[1]) == 1 ? true : false;
                customerCreator = int.Parse(userRole[2]) == 1 ? true : false;
                purchaseOrderCreator = int.Parse(userRole[3]) == 1 ? true : false;
                companyId = int.Parse(userRole[4]);
                userName = ci.Name;
                appConnData = GetAppConnData(companyId);
            }
            #endregion

            List<DocumentSeries> docSeries = backEnd.GetDocumentSeriesList(SapDocumentType.SalesOrder, appConnData);
            return Json(docSeries, JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        //[OutputCache(Duration = 3600, VaryByParam = "none")]
        public JsonResult AsyncCustomersList()
        {
            #region User identification
            IIdentity context = HttpContext.User.Identity;
            bool admin = false;
            bool customerCreator = false;
            bool purchaseOrderCreator = false;
            int companyId = 0;
            string userName = "";
            int slpCode = 0;

            AppConnData appConnData = new AppConnData();

            if (context.IsAuthenticated)
            {

                System.Web.Security.FormsIdentity ci = (System.Web.Security.FormsIdentity)HttpContext.User.Identity;
                string[] userRole = ci.Ticket.UserData.Split('|');

                admin = int.Parse(userRole[1]) == 1 ? true : false;
                customerCreator = int.Parse(userRole[2]) == 1 ? true : false;
                purchaseOrderCreator = int.Parse(userRole[3]) == 1 ? true : false;
                companyId = int.Parse(userRole[4]);
                slpCode = int.Parse(userRole[5]);
                userName = ci.Name;
                appConnData = GetAppConnData(companyId);
            }
            #endregion


            List<CompanyParameter> companyParameters = BizCompanyParameter.GetList(companyId).ToList();
            string ShowOnlyAsociateCustomers = companyParameters.Where(x => x.idParameter.Equals(7)).Select(x => x.value).FirstOrDefault();

            List<GenericBusinessPartner> customers = new List<GenericBusinessPartner>();

            if (ShowOnlyAsociateCustomers == "Si")
                customers = backEnd.GetBusinessPartnersBySalesPerson(CardType.Customer, slpCode.ToString(), appConnData);
            else
                customers = backEnd.GetBusinessPartners(CardType.Customer, appConnData);

            var data = from c in customers select new[] { c.cardCode, c.cardName, string.Empty };
            return Json(new
            {
                data = data
            }, JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        //[OutputCache(Duration = int.MaxValue, VaryByParam = "none")]
        public JsonResult AsyncItemsList()
        {
            #region User identification
            IIdentity context = HttpContext.User.Identity;
            bool admin = false;
            bool customerCreator = false;
            bool purchaseOrderCreator = false;
            int companyId = 0;
            string userName = "";
            AppConnData appConnData = new AppConnData();

            if (context.IsAuthenticated)
            {

                System.Web.Security.FormsIdentity ci = (System.Web.Security.FormsIdentity)HttpContext.User.Identity;
                string[] userRole = ci.Ticket.UserData.Split('|');

                admin = int.Parse(userRole[1]) == 1 ? true : false;
                customerCreator = int.Parse(userRole[2]) == 1 ? true : false;
                purchaseOrderCreator = int.Parse(userRole[3]) == 1 ? true : false;
                companyId = int.Parse(userRole[4]);
                userName = ci.Name;
                appConnData = GetAppConnData(companyId);
            }
            #endregion

            List<GenericItem> items = backEnd.GetItems(appConnData);
            var data = from c in items select new[] { c.ItemCode, c.ItemName, string.Empty };

            return Json(new
            {
                data = data
            }, JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        public JsonResult AsyncStockList(string id)
        {
            #region User identification
            IIdentity context = HttpContext.User.Identity;
            bool admin = false;
            bool customerCreator = false;
            bool purchaseOrderCreator = false;
            int companyId = 0;
            string userName = "";
            AppConnData appConnData = new AppConnData();

            if (context.IsAuthenticated)
            {

                System.Web.Security.FormsIdentity ci = (System.Web.Security.FormsIdentity)HttpContext.User.Identity;
                string[] userRole = ci.Ticket.UserData.Split('|');

                admin = int.Parse(userRole[1]) == 1 ? true : false;
                customerCreator = int.Parse(userRole[2]) == 1 ? true : false;
                purchaseOrderCreator = int.Parse(userRole[3]) == 1 ? true : false;
                companyId = int.Parse(userRole[4]);
                userName = ci.Name;
                appConnData = GetAppConnData(companyId);
            }
            #endregion

            List<StockLevel> items = backEnd.GetItemStockLevel(id, appConnData);

            var data =
                from c in items
                select new[] { c.WhsCode, c.WhsName, c.OnHand.ToString(), c.OnOrder.ToString(), c.IsCommited.ToString(), c.IsAvailable.ToString(), string.Empty };

            return Json(new
            {
                data = data
            }, JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        public JsonResult AsyncGetItemPrice(string id)
        {
            #region User identification
            IIdentity context = HttpContext.User.Identity;
            bool admin = false;
            bool customerCreator = false;
            bool purchaseOrderCreator = false;
            int companyId = 0;
            string userName = "";
            AppConnData appConnData = new AppConnData();

            if (context.IsAuthenticated)
            {

                System.Web.Security.FormsIdentity ci = (System.Web.Security.FormsIdentity)HttpContext.User.Identity;
                string[] userRole = ci.Ticket.UserData.Split('|');

                admin = int.Parse(userRole[1]) == 1 ? true : false;
                customerCreator = int.Parse(userRole[2]) == 1 ? true : false;
                purchaseOrderCreator = int.Parse(userRole[3]) == 1 ? true : false;
                companyId = int.Parse(userRole[4]);
                userName = ci.Name;
                appConnData = GetAppConnData(companyId);
            }
            #endregion

            double price = backEnd.GetItemPrice(id.Split('|')[0], int.Parse(id.Split('|')[1]), appConnData);
            return Json(price.ToString(), JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        //[OutputCache(Duration = int.MaxValue, VaryByParam = "none")]
        public JsonResult AsyncGetDistributionRulesList()
        {
            #region User identification
            IIdentity context = HttpContext.User.Identity;
            bool admin = false;
            bool customerCreator = false;
            bool purchaseOrderCreator = false;
            int companyId = 0;
            string userName = "";
            AppConnData appConnData = new AppConnData();

            if (context.IsAuthenticated)
            {

                System.Web.Security.FormsIdentity ci = (System.Web.Security.FormsIdentity)HttpContext.User.Identity;
                string[] userRole = ci.Ticket.UserData.Split('|');

                admin = int.Parse(userRole[1]) == 1 ? true : false;
                customerCreator = int.Parse(userRole[2]) == 1 ? true : false;
                purchaseOrderCreator = int.Parse(userRole[3]) == 1 ? true : false;
                companyId = int.Parse(userRole[4]);
                userName = ci.Name;
                appConnData = GetAppConnData(companyId);
            }
            #endregion
            List<SapDistributionRule> sapDistributionRulesList = backEnd.GetDistributionRulesList(appConnData);
            return Json(sapDistributionRulesList, JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        public JsonResult AsyncOrderLinesList(string id)
        {
            List<RDR1> items = BizSalesOrderDraft.GetLinesList(int.Parse(id)).ToList();

            var data =
                from c in items
                select new[] { c.itemCode, c.quantity.ToString(), c.price.ToString(), (c.price * c.quantity).ToString("#.##") };

            return Json(new
            {
                data = data
            }, JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        public JsonResult AsyncAddressList(string id)
        {
            #region User identification
            IIdentity context = HttpContext.User.Identity;
            bool admin = false;
            bool customerCreator = false;
            bool purchaseOrderCreator = false;
            int companyId = 0;
            string userName = "";
            AppConnData appConnData = new AppConnData();

            if (context.IsAuthenticated)
            {

                System.Web.Security.FormsIdentity ci = (System.Web.Security.FormsIdentity)HttpContext.User.Identity;
                string[] userRole = ci.Ticket.UserData.Split('|');

                admin = int.Parse(userRole[1]) == 1 ? true : false;
                customerCreator = int.Parse(userRole[2]) == 1 ? true : false;
                purchaseOrderCreator = int.Parse(userRole[3]) == 1 ? true : false;
                companyId = int.Parse(userRole[4]);
                userName = ci.Name;
                appConnData = GetAppConnData(companyId);
            }
            #endregion

            string[] par = id.Split('|');

            List<BusinessPartnerAddress> addresses = backEnd.GetAddressList(par[0], par[1] == "S" ? AddressType.ShipTo : AddressType.BillTo, appConnData);

            return Json(addresses, JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        private AppConnData GetAppConnData(int companyId)
        {
            Company company = BizCompany.GetSingle(companyId);

            return new AppConnData()
            {
                adoConnString = company.connStringName,
                dataBaseName = company.dataBaseName,
                sapUser = company.sapUser,
                sapUserPassword = company.sapPassword,
                wsAppKey = ConfigurationManager.AppSettings["WSAppKey"].ToString(),
                wsSecret = ConfigurationManager.AppSettings["WSSecret"].ToString()
            };
        }

        [Authorize]
        public JsonResult AsyncSearchOrders(string id)
        {
            #region User identification
            IIdentity context = HttpContext.User.Identity;
            int user = 0;
            bool admin = false;
            bool customerCreator = false;
            bool purchaseOrderCreator = false;
            int companyId = 0;
            string userName = "";
            AppConnData appConnData = new AppConnData();

            if (context.IsAuthenticated)
            {

                System.Web.Security.FormsIdentity ci = (System.Web.Security.FormsIdentity)HttpContext.User.Identity;
                string[] userRole = ci.Ticket.UserData.Split('|');
                user = int.Parse(userRole[0]);
                admin = int.Parse(userRole[1]) == 1 ? true : false;
                customerCreator = int.Parse(userRole[2]) == 1 ? true : false;
                purchaseOrderCreator = int.Parse(userRole[3]) == 1 ? true : false;
                companyId = int.Parse(userRole[4]);
                userName = ci.Name;
                appConnData = GetAppConnData(companyId);
            }
            #endregion

            string[] parameters = id.Split('|');

            DateTime from = DateTime.Parse(parameters[0]), to = DateTime.Parse(parameters[1]);
            string cardCode = parameters[2];

            List<LightMarketingDocument> ordrs = backEnd.ListSaleOrders(from, to, cardCode, appConnData);

            var data = from c in ordrs select new[] { c.docNum.ToString(), c.docDate.ToString("yyyy-MM-dd"), c.docDueDate.ToString("yyyy-MM-dd"), c.docStatus };

            return Json(new
            {
                data = data
            }, JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        public JsonResult AsyncGeoReport(string id)
        {
            #region User identification
            IIdentity context = HttpContext.User.Identity;
            int user = 0;
            bool admin = false;
            bool customerCreator = false;
            bool purchaseOrderCreator = false;
            int companyId = 0;
            string userName = "";
            AppConnData appConnData = new AppConnData();

            if (context.IsAuthenticated)
            {

                System.Web.Security.FormsIdentity ci = (System.Web.Security.FormsIdentity)HttpContext.User.Identity;
                string[] userRole = ci.Ticket.UserData.Split('|');
                user = int.Parse(userRole[0]);
                admin = int.Parse(userRole[1]) == 1 ? true : false;
                customerCreator = int.Parse(userRole[2]) == 1 ? true : false;
                purchaseOrderCreator = int.Parse(userRole[3]) == 1 ? true : false;
                companyId = int.Parse(userRole[4]);
                userName = ci.Name;
                appConnData = GetAppConnData(companyId);
            }
            #endregion

            string[] parameters = id.Split('|');

            DateTime from = DateTime.Parse(parameters[0]), to = DateTime.Parse(parameters[1]);
            int slpCode = int.Parse(parameters[2]);

            List<ORDR> ordrs = BizSalesOrderDraft.GetList(from, to, slpCode, companyId).ToList();

            return Json(ordrs, JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        public JsonResult AsyncAuthorizationList(string id)
        {
            #region User identification
            IIdentity context = HttpContext.User.Identity;
            int user = 0;
            bool admin = false;
            bool customerCreator = false;
            bool purchaseOrderCreator = false;
            int companyId = 0;
            string userName = "";
            AppConnData appConnData = new AppConnData();

            if (context.IsAuthenticated)
            {

                System.Web.Security.FormsIdentity ci = (System.Web.Security.FormsIdentity)HttpContext.User.Identity;
                string[] userRole = ci.Ticket.UserData.Split('|');
                user = int.Parse(userRole[0]);
                admin = int.Parse(userRole[1]) == 1 ? true : false;
                customerCreator = int.Parse(userRole[2]) == 1 ? true : false;
                purchaseOrderCreator = int.Parse(userRole[3]) == 1 ? true : false;
                companyId = int.Parse(userRole[4]);
                userName = ci.Name;
                appConnData = GetAppConnData(companyId);
            }
            #endregion

            string[] parms = id.Split('|');
            DateTime from = DateTime.Parse(parms[0]);
            DateTime to = DateTime.Parse(parms[1]);

            List<AuthorizationStatus> items = backEnd.GetAuthorizationStatusList(from, to, appConnData);

            var data =
                from c in items
                select new[] { "Pedido", c.createDate.ToString("yyyy-MM-dd"), c.createTime.ToString(), c.currStepName, c.docNum.ToString(), c.isDraft, c.ownerName, c.remarks };

            return Json(new
            {
                data = data
            }, JsonRequestBehavior.AllowGet);
        }

    }
}