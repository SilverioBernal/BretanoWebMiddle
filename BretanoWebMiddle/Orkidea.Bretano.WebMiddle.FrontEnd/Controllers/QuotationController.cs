using Orkidea.Bretano.WebMiddle.FrontEnd.Business;
using Orkidea.Bretano.WebMiddle.FrontEnd.Entities;
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

namespace Orkidea.Bretano.WebMiddle.FrontEnd.Controllers
{
    public class QuotationController : Controller
    {
        WSSAPClient backEnd = new WSSAPClient();

        [Authorize]
        public ActionResult Create()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        public ActionResult Create(OQUT quotation)
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

            quotation.uOrkUsuarioWeb = userId;
            quotation.idCompania = companyId;
            quotation.slpCode = slpCode;
            int orderNum = BizQuotation.Add(quotation);
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
            OQUT OQUT = BizQuotation.GetSingle(int.Parse(realId));

            BusinessPartner customer = backEnd.GetBusinessPartner(OQUT.cardCode, appConnData);


            ViewBag.serie = backEnd.GetDocumentSeriesSingle(OQUT.series, appConnData).seriesName;
            ViewBag.cardCode = OQUT.cardCode;
            ViewBag.cardName = customer.cardName;
            ViewBag.docDate = OQUT.docDate.ToString("yyyy-MM-dd");
            ViewBag.docDueDate = OQUT.docDueDate.ToString("yyyy-MM-dd");
            ViewBag.taxDate = OQUT.taxDate.ToString("yyyy-MM-dd");
            ViewBag.uCssComentarios = OQUT.uCssComentarios;
            ViewBag.listNum = customer.listNum;
            ViewBag.orderId = realId;
            ViewBag.id = id;
            return View();
        }

        [HttpPost]
        [Authorize]
        public ActionResult AddLine(string id, QUT1 line)
        {
            string realId = HexSerialization.HexToString(id);
            line.orderId = int.Parse(realId);
            BizQuotation.AddLine(line);
            return RedirectToAction("AddLine", new { id = id });
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
                OQUT OQUT = BizQuotation.GetSingle(int.Parse(realId));
                List<QUT1> lines = BizQuotation.GetLinesList(int.Parse(realId)).ToList();

                MarketingDocument document = new MarketingDocument()
                {
                    cardCode = OQUT.cardCode,
                    serie = OQUT.series,
                    docDate = OQUT.docDate,
                    docDueDate = OQUT.docDueDate,
                    taxDate = OQUT.taxDate,
                    shipToCode = OQUT.shipToCode,
                    payToCode = OQUT.payToCode,
                    lines = new List<MarketingDocumentLine>(),
                    userDefinedFields = new List<UserDefinedField>()
                };

                if (slpCode > 0)
                    document.slpCode = slpCode;

                document.userDefinedFields.Add(new UserDefinedField()
                {
                    name = "U_CSS_COMENTARIOS",
                    type = UdfType.Text,
                    value = OQUT.uCssComentarios
                });

                foreach (QUT1 item in lines)
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


                document = backEnd.AddQuotation(document, appConnData);
                ViewBag.colorMensaje = "success";
                ViewBag.mensaje = "Cotización creada con éxito";
                ViewBag.docEntry = string.Format("Se creó la cotización no {0}", document.docEntry);

                OQUT.docEntry = document.docEntry;
                BizQuotation.Update(OQUT);
            }
            catch (FaultException<DataAccessFault> ex)
            {
                ViewBag.colorMensaje = "danger";
                ViewBag.mensaje = "No se pudo crear la Cotización";
                ViewBag.docEntry = string.Format("Codigo {0} error:{1} {2}", ex.Code, ex.Detail.Description, ex.Message);
            }
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

            List<DocumentSeries> docSeries = backEnd.GetDocumentSeriesList(SapDocumentType.Quotation, appConnData);
            return Json(docSeries, JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        public JsonResult AsyncQuotationLinesList(string id)
        {
            List<QUT1> items = BizQuotation.GetLinesList(int.Parse(id)).ToList();

            var data =
                from c in items
                select new[] { c.itemCode, c.quantity.ToString(), c.price.ToString(), (c.price * c.quantity).ToString("#.##") };

            return Json(new
            {
                data = data
            }, JsonRequestBehavior.AllowGet);
        }

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
