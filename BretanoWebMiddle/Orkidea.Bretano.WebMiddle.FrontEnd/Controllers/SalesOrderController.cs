using Orkidea.Bretano.WebMiddle.FrontEnd.Business;
using Orkidea.Bretano.WebMiddle.FrontEnd.Entities;
using Orkidea.Bretano.WebMiddle.FrontEnd.Models;
using Orkidea.Bretano.WebMiddle.FrontEnd.WebMiddleBackEnd;
using Orkidea.Framework.Security;
using System;
using System.Collections.Generic;
using System.Configuration;
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

            order.uOrkUsuarioWeb = userId;

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
            return View();
        }

        [HttpPost]
        [Authorize]
        public ActionResult AddLine(string id, RDR1 line)
        {
            string realId = HexSerialization.HexToString(id);
            line.orderId = int.Parse(realId);
            BizSalesOrderDraft.AddLine(line);
            return RedirectToAction("AddLine", new { id = id });
        }

        [Authorize]
        public ActionResult Finish(string id)
        {
            try
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

                string realId = HexSerialization.HexToString(id);
                ORDR ordr = BizSalesOrderDraft.GetSingle(int.Parse(realId));
                List<RDR1> lines = BizSalesOrderDraft.GetLinesList(int.Parse(realId)).ToList();

                MarketingDocument document = new MarketingDocument()
                {
                    cardCode = ordr.cardCode,
                    serie = ordr.series,
                    docDate = ordr.docDate,
                    docDueDate = ordr.docDueDate,
                    taxDate = ordr.taxDate,
                    lines = new List<MarketingDocumentLine>(),
                    userDefinedFields = new List<UserDefinedField>()
                };

                document.userDefinedFields.Add(new UserDefinedField()
                {
                    name = "U_CSS_COMENTARIOS",
                    type = UdfType.Text,
                    value = ordr.uCssComentarios
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

                document = backEnd.AddSalesOrder(document, appConnData);
                ViewBag.colorMensaje = "success";
                ViewBag.mensaje = "Orden de venta creada con éxito";
                ViewBag.docEntry = string.Format("Se creó la orden no {0}", document.docEntry);
            }
            catch (FaultException<DataAccessFault> ex)
            {
                ViewBag.colorMensaje = "danger";
                ViewBag.mensaje = "No se pudo crear la Orden de venta";
                ViewBag.docEntry = string.Format("Codigo {0} error:{1} {2}", ex.Code, ex.Detail.Description, ex.Message);
            }
            return View();
        }

        [Authorize]
        [OutputCache(Duration = int.MaxValue, VaryByParam = "none")]
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

            List<GenericBusinessPartner> customers = backEnd.GetBusinessPartners(CardType.Customer, appConnData);
            var data = from c in customers select new[] { c.cardCode, c.cardName, string.Empty };
            return Json(new
            {
                data = data
            }, JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        [OutputCache(Duration = int.MaxValue, VaryByParam = "none")]
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
        [OutputCache(Duration = int.MaxValue, VaryByParam = "none")]
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
    }
}