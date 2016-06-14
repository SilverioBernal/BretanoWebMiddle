using Orkidea.Bretano.WebMiddle.FrontEnd.Business;
using Orkidea.Bretano.WebMiddle.FrontEnd.Entities;
using Orkidea.Bretano.WebMiddle.FrontEnd.Models;
using Orkidea.Bretano.WebMiddle.FrontEnd.WebMiddleBackEnd;
using Orkidea.Framework.Security;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Security.Principal;
using System.ServiceModel;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;

namespace Orkidea.Bretano.WebMiddle.FrontEnd.Controllers
{
    public class CustomerController : Controller
    {
        WSSAPClient backEnd = new WSSAPClient();
        //
        // GET: /Customer/
        public ActionResult Index()
        {

            return View();
        }

        [Authorize]
        public ActionResult create()
        {
            CustomerViewModel customer = new CustomerViewModel();
            return View(customer);
        }

        [Authorize]
        [HttpPost]
        public ActionResult create(CustomerViewModel customer)
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

                BusinessPartner bp = new BusinessPartner()
                {
                    #region data
                    cardType = customer.bpType == "C" ? CardType.Customer : CardType.Lead,
                    cardCode = customer.cardCode,
                    cardName = customer.cardName,
                    cardFName = customer.cardFName,
                    groupCode = customer.groupCode,
                    licTradNum = customer.licTradNum,
                    currency = customer.currency,
                    password = customer.password,
                    e_Mail = customer.e_Mail,
                    phone1 = customer.phone1,
                    phone2 = customer.phone2,
                    slpCode = customer.slpCode,
                    dunTerm = customer.dunTerm,
                    groupNum = customer.groupNum,
                    qryGroup1 = customer.qryGroup1,
                    qryGroup2 = customer.qryGroup2,
                    qryGroup3 = customer.qryGroup3,
                    qryGroup4 = customer.qryGroup4,
                    qryGroup5 = customer.qryGroup5,
                    qryGroup6 = customer.qryGroup6,
                    qryGroup7 = customer.qryGroup7,
                    qryGroup8 = customer.qryGroup8,
                    qryGroup9 = customer.qryGroup9,
                    qryGroup10 = customer.qryGroup10,
                    qryGroup11 = customer.qryGroup11,
                    qryGroup12 = customer.qryGroup12,
                    qryGroup13 = customer.qryGroup13,
                    qryGroup14 = customer.qryGroup14,
                    qryGroup15 = customer.qryGroup15,
                    qryGroup16 = customer.qryGroup16,
                    qryGroup17 = customer.qryGroup17,
                    qryGroup18 = customer.qryGroup18,
                    qryGroup19 = customer.qryGroup19,
                    qryGroup20 = customer.qryGroup20,
                    qryGroup21 = customer.qryGroup21,
                    qryGroup22 = customer.qryGroup22,
                    qryGroup23 = customer.qryGroup23,
                    qryGroup24 = customer.qryGroup24,
                    qryGroup25 = customer.qryGroup25,
                    qryGroup26 = customer.qryGroup26,
                    qryGroup27 = customer.qryGroup27,
                    qryGroup28 = customer.qryGroup28,
                    qryGroup29 = customer.qryGroup29,
                    qryGroup30 = customer.qryGroup30,
                    qryGroup31 = customer.qryGroup31,
                    qryGroup32 = customer.qryGroup32,
                    qryGroup33 = customer.qryGroup33,
                    qryGroup34 = customer.qryGroup34,
                    qryGroup35 = customer.qryGroup35,
                    qryGroup36 = customer.qryGroup36,
                    qryGroup37 = customer.qryGroup37,
                    qryGroup38 = customer.qryGroup38,
                    qryGroup39 = customer.qryGroup39,
                    qryGroup40 = customer.qryGroup40,
                    qryGroup41 = customer.qryGroup41,
                    qryGroup42 = customer.qryGroup42,
                    qryGroup43 = customer.qryGroup43,
                    qryGroup44 = customer.qryGroup44,
                    qryGroup45 = customer.qryGroup45,
                    qryGroup46 = customer.qryGroup46,
                    qryGroup47 = customer.qryGroup47,
                    qryGroup48 = customer.qryGroup48,
                    qryGroup49 = customer.qryGroup49,
                    qryGroup50 = customer.qryGroup50,
                    qryGroup51 = customer.qryGroup51,
                    qryGroup52 = customer.qryGroup52,
                    qryGroup53 = customer.qryGroup53,
                    qryGroup54 = customer.qryGroup54,
                    qryGroup55 = customer.qryGroup55,
                    qryGroup56 = customer.qryGroup56,
                    qryGroup57 = customer.qryGroup57,
                    qryGroup58 = customer.qryGroup58,
                    qryGroup59 = customer.qryGroup59,
                    qryGroup60 = customer.qryGroup60,
                    qryGroup61 = customer.qryGroup61,
                    qryGroup62 = customer.qryGroup62,
                    qryGroup63 = customer.qryGroup63,
                    qryGroup64 = customer.qryGroup64,
                    freeText = customer.freeText,
                    wtLiable = true,
                    userDefinedFields = new List<UserDefinedField>()
                    #endregion
                };

                #region UDF's
                bp.userDefinedFields.Add(new UserDefinedField()
                        {
                            name = "U_BPCO_RTC",
                            value = customer.uBpcoRt,
                            type = UdfType.Alphanumeric
                        });

                bp.userDefinedFields.Add(new UserDefinedField()
                {
                    name = "U_BPCO_TDC",
                    value = customer.uBpcoTdc,
                    type = UdfType.Alphanumeric
                });

                bp.userDefinedFields.Add(new UserDefinedField()
                {
                    name = "U_BPCO_CS",
                    value = customer.uBpcoCs,
                    type = UdfType.Alphanumeric
                });

                bp.userDefinedFields.Add(new UserDefinedField()
                {
                    name = "U_BPCO_City",
                    value = customer.uBpcoCity,
                    type = UdfType.Alphanumeric
                });

                bp.userDefinedFields.Add(new UserDefinedField()
                {
                    name = "U_BPCO_TP",
                    value = customer.uBpcoTp,
                    type = UdfType.Alphanumeric
                });

                bp.userDefinedFields.Add(new UserDefinedField()
                {
                    name = "U_CSS_IVA",
                    value = customer.uCssIva,
                    type = UdfType.Alphanumeric
                });

                bp.userDefinedFields.Add(new UserDefinedField()
                {
                    name = "U_CSS_aceptacion_fac",
                    value = customer.uCssAcceptInvoice,
                    type = UdfType.Integer
                });

                bp.userDefinedFields.Add(new UserDefinedField()
                {
                    name = "U_QCA_SEGMENTACION",
                    value = customer.uQcaSegment,
                    type = UdfType.Alphanumeric
                });
                #endregion

                backEnd.AddBusinessPartner(bp, appConnData);

            }
            catch (FaultException<DataAccessFault> ex)
            {
                ViewBag.mensaje = string.Format("Codigo {0} error:{1} {2}", ex.Code, ex.Detail.Description, ex.Message);
                return View(new CustomerViewModel());
            }

            string id = HexSerialization.StringToHex(string.Format("{0}|{1}", customer.cardCode, customer.cardName));

            return RedirectToAction("addContact", new { id = id });
            //customer.contactPersons = new List<ContactEmployee>();
            //return View("addContact", customer);
        }

        [Authorize]
        public ActionResult edit(string id)
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

            BusinessPartner bp = backEnd.GetBusinessPartner(id, appConnData);

            CustomerViewModel customer = new CustomerViewModel(appConnData)
            {
                #region Fields
                cardType = bp.cardType,
                cardCode = bp.cardCode,
                cardName = bp.cardName,
                cardFName = bp.cardFName,
                groupCode = bp.groupCode,
                licTradNum = bp.licTradNum,
                currency = bp.currency,
                password = bp.password,
                e_Mail = bp.e_Mail,
                phone1 = bp.phone1,
                phone2 = bp.phone2,
                slpCode = bp.slpCode,
                groupNum = bp.groupNum,
                dunTerm = bp.dunTerm,
                qryGroup1 = bp.qryGroup1,
                qryGroup2 = bp.qryGroup2,
                qryGroup3 = bp.qryGroup3,
                qryGroup4 = bp.qryGroup4,
                qryGroup5 = bp.qryGroup5,
                qryGroup6 = bp.qryGroup6,
                qryGroup7 = bp.qryGroup7,
                qryGroup8 = bp.qryGroup8,
                qryGroup9 = bp.qryGroup9,
                qryGroup10 = bp.qryGroup10,
                qryGroup11 = bp.qryGroup11,
                qryGroup12 = bp.qryGroup12,
                qryGroup13 = bp.qryGroup13,
                qryGroup14 = bp.qryGroup14,
                qryGroup15 = bp.qryGroup15,
                qryGroup16 = bp.qryGroup16,
                qryGroup17 = bp.qryGroup17,
                qryGroup18 = bp.qryGroup18,
                qryGroup19 = bp.qryGroup19,
                qryGroup20 = bp.qryGroup20,
                qryGroup21 = bp.qryGroup21,
                qryGroup22 = bp.qryGroup22,
                qryGroup23 = bp.qryGroup23,
                qryGroup24 = bp.qryGroup24,
                qryGroup25 = bp.qryGroup25,
                qryGroup26 = bp.qryGroup26,
                qryGroup27 = bp.qryGroup27,
                qryGroup28 = bp.qryGroup28,
                qryGroup29 = bp.qryGroup29,
                qryGroup30 = bp.qryGroup30,
                qryGroup31 = bp.qryGroup31,
                qryGroup32 = bp.qryGroup32,
                qryGroup33 = bp.qryGroup33,
                qryGroup34 = bp.qryGroup34,
                qryGroup35 = bp.qryGroup35,
                qryGroup36 = bp.qryGroup36,
                qryGroup37 = bp.qryGroup37,
                qryGroup38 = bp.qryGroup38,
                qryGroup39 = bp.qryGroup39,
                qryGroup40 = bp.qryGroup40,
                qryGroup41 = bp.qryGroup41,
                qryGroup42 = bp.qryGroup42,
                qryGroup43 = bp.qryGroup43,
                qryGroup44 = bp.qryGroup44,
                qryGroup45 = bp.qryGroup45,
                qryGroup46 = bp.qryGroup46,
                qryGroup47 = bp.qryGroup47,
                qryGroup48 = bp.qryGroup48,
                qryGroup49 = bp.qryGroup49,
                qryGroup50 = bp.qryGroup50,
                qryGroup51 = bp.qryGroup51,
                qryGroup52 = bp.qryGroup52,
                qryGroup53 = bp.qryGroup53,
                qryGroup54 = bp.qryGroup54,
                qryGroup55 = bp.qryGroup55,
                qryGroup56 = bp.qryGroup56,
                qryGroup57 = bp.qryGroup57,
                qryGroup58 = bp.qryGroup58,
                qryGroup59 = bp.qryGroup59,
                qryGroup60 = bp.qryGroup60,
                qryGroup61 = bp.qryGroup61,
                qryGroup62 = bp.qryGroup62,
                qryGroup63 = bp.qryGroup63,
                qryGroup64 = bp.qryGroup64,
                freeText = bp.freeText,
                uBpcoRt = bp.userDefinedFields.Where(x => x.name.Equals("BPCO_RTC")).Select(x => x.value).FirstOrDefault(),
                uBpcoCity = bp.userDefinedFields.Where(x => x.name.Equals("BPCO_City")).Select(x => x.value).FirstOrDefault(),
                uBpcoTdc = bp.userDefinedFields.Where(x => x.name.Equals("BPCO_TDC")).Select(x => x.value).FirstOrDefault(),
                uBpcoCs = bp.userDefinedFields.Where(x => x.name.Equals("BPCO_CS")).Select(x => x.value).FirstOrDefault(),
                uBpcoTp = bp.userDefinedFields.Where(x => x.name.Equals("BPCO_TP")).Select(x => x.value).FirstOrDefault(),
                uCssIva = bp.userDefinedFields.Where(x => x.name.Equals("CSS_IVA")).Select(x => x.value).FirstOrDefault(),
                uCssAcceptInvoice = bp.userDefinedFields.Where(x => x.name.Equals("CSS_aceptacion_fac")).Select(x => x.value).FirstOrDefault(),
                uQcaSegment = bp.userDefinedFields.Where(x => x.name.Equals("QCA_SEGMENTACION")).Select(x => x.value).FirstOrDefault(),
                #endregion                               
            };

            ViewBag.code = HexSerialization.StringToHex(string.Format("{0}|{1}", bp.cardCode, bp.cardName));

            return View(customer);
        }

        [Authorize]
        [HttpPost]
        public ActionResult edit(string id, CustomerViewModel customer)
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

                BusinessPartner bp = new BusinessPartner()
                {
                    #region data
                    cardType = customer.bpType == "C" ? CardType.Customer : CardType.Lead,
                    cardCode = customer.cardCode,
                    cardName = customer.cardName,
                    cardFName = customer.cardFName,
                    groupCode = customer.groupCode,
                    licTradNum = customer.licTradNum,
                    currency = customer.currency,
                    password = customer.password,
                    e_Mail = customer.e_Mail,
                    phone1 = customer.phone1,
                    phone2 = customer.phone2,
                    slpCode = customer.slpCode,
                    dunTerm = customer.dunTerm,
                    groupNum = customer.groupNum,
                    qryGroup1 = customer.qryGroup1,
                    qryGroup2 = customer.qryGroup2,
                    qryGroup3 = customer.qryGroup3,
                    qryGroup4 = customer.qryGroup4,
                    qryGroup5 = customer.qryGroup5,
                    qryGroup6 = customer.qryGroup6,
                    qryGroup7 = customer.qryGroup7,
                    qryGroup8 = customer.qryGroup8,
                    qryGroup9 = customer.qryGroup9,
                    qryGroup10 = customer.qryGroup10,
                    qryGroup11 = customer.qryGroup11,
                    qryGroup12 = customer.qryGroup12,
                    qryGroup13 = customer.qryGroup13,
                    qryGroup14 = customer.qryGroup14,
                    qryGroup15 = customer.qryGroup15,
                    qryGroup16 = customer.qryGroup16,
                    qryGroup17 = customer.qryGroup17,
                    qryGroup18 = customer.qryGroup18,
                    qryGroup19 = customer.qryGroup19,
                    qryGroup20 = customer.qryGroup20,
                    qryGroup21 = customer.qryGroup21,
                    qryGroup22 = customer.qryGroup22,
                    qryGroup23 = customer.qryGroup23,
                    qryGroup24 = customer.qryGroup24,
                    qryGroup25 = customer.qryGroup25,
                    qryGroup26 = customer.qryGroup26,
                    qryGroup27 = customer.qryGroup27,
                    qryGroup28 = customer.qryGroup28,
                    qryGroup29 = customer.qryGroup29,
                    qryGroup30 = customer.qryGroup30,
                    qryGroup31 = customer.qryGroup31,
                    qryGroup32 = customer.qryGroup32,
                    qryGroup33 = customer.qryGroup33,
                    qryGroup34 = customer.qryGroup34,
                    qryGroup35 = customer.qryGroup35,
                    qryGroup36 = customer.qryGroup36,
                    qryGroup37 = customer.qryGroup37,
                    qryGroup38 = customer.qryGroup38,
                    qryGroup39 = customer.qryGroup39,
                    qryGroup40 = customer.qryGroup40,
                    qryGroup41 = customer.qryGroup41,
                    qryGroup42 = customer.qryGroup42,
                    qryGroup43 = customer.qryGroup43,
                    qryGroup44 = customer.qryGroup44,
                    qryGroup45 = customer.qryGroup45,
                    qryGroup46 = customer.qryGroup46,
                    qryGroup47 = customer.qryGroup47,
                    qryGroup48 = customer.qryGroup48,
                    qryGroup49 = customer.qryGroup49,
                    qryGroup50 = customer.qryGroup50,
                    qryGroup51 = customer.qryGroup51,
                    qryGroup52 = customer.qryGroup52,
                    qryGroup53 = customer.qryGroup53,
                    qryGroup54 = customer.qryGroup54,
                    qryGroup55 = customer.qryGroup55,
                    qryGroup56 = customer.qryGroup56,
                    qryGroup57 = customer.qryGroup57,
                    qryGroup58 = customer.qryGroup58,
                    qryGroup59 = customer.qryGroup59,
                    qryGroup60 = customer.qryGroup60,
                    qryGroup61 = customer.qryGroup61,
                    qryGroup62 = customer.qryGroup62,
                    qryGroup63 = customer.qryGroup63,
                    qryGroup64 = customer.qryGroup64,
                    freeText = customer.freeText,
                    wtLiable = true,
                    userDefinedFields = new List<UserDefinedField>()
                    #endregion
                };

                #region UDF's
                bp.userDefinedFields.Add(new UserDefinedField()
                {
                    name = "U_BPCO_RTC",
                    value = customer.uBpcoRt,
                    type = UdfType.Alphanumeric
                });

                bp.userDefinedFields.Add(new UserDefinedField()
                {
                    name = "U_BPCO_TDC",
                    value = customer.uBpcoTdc,
                    type = UdfType.Alphanumeric
                });

                bp.userDefinedFields.Add(new UserDefinedField()
                {
                    name = "U_BPCO_CS",
                    value = customer.uBpcoCs,
                    type = UdfType.Alphanumeric
                });

                bp.userDefinedFields.Add(new UserDefinedField()
                {
                    name = "U_BPCO_City",
                    value = customer.uBpcoCity,
                    type = UdfType.Alphanumeric
                });

                bp.userDefinedFields.Add(new UserDefinedField()
                {
                    name = "U_BPCO_TP",
                    value = customer.uBpcoTp,
                    type = UdfType.Alphanumeric
                });

                bp.userDefinedFields.Add(new UserDefinedField()
                {
                    name = "U_CSS_IVA",
                    value = customer.uCssIva,
                    type = UdfType.Alphanumeric
                });

                bp.userDefinedFields.Add(new UserDefinedField()
                {
                    name = "U_CSS_aceptacion_fac",
                    value = customer.uCssAcceptInvoice,
                    type = UdfType.Integer
                });

                bp.userDefinedFields.Add(new UserDefinedField()
                {
                    name = "U_QCA_SEGMENTACION",
                    value = customer.uQcaSegment,
                    type = UdfType.Alphanumeric
                });
                #endregion

                backEnd.UpdateBusinessPartner(bp, appConnData);

            }
            catch (FaultException<DataAccessFault> ex)
            {
                ViewBag.mensaje = string.Format("Codigo {0} error:{1} {2}", ex.Code, ex.Detail.Description, ex.Message);
                return View(customer);
            }

            id = HexSerialization.StringToHex(string.Format("{0}|{1}", customer.cardCode, customer.cardName));
            return RedirectToAction("addContact", new { id = id });
            //customer.contactPersons = new List<ContactEmployee>();
            //return View("addContact", customer);
        }

        [Authorize]
        public ActionResult addContact(string id)
        {
            string[] customerData = HexSerialization.HexToString(id).Split('|');
            ViewBag.customer = id;
            ViewBag.cardCode = customerData[0];
            ViewBag.cardName = customerData[1];
            return View();
        }

        [Authorize]
        [HttpPost]
        public ActionResult addContact(string id, ContactEmployee contact)
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
            string[] customerData = HexSerialization.HexToString(id).Split('|');

            BusinessPartner customer = new BusinessPartner() { cardCode = customerData[0] };

            try
            {
                contact.cardCode = customerData[0];
                backEnd.AddBusinessPartnerContact(contact, appConnData);
            }
            catch (FaultException<DataAccessFault> ex)
            {
                ViewBag.mensaje = string.Format("Codigo {0} error:{1} {2}", ex.Code, ex.Detail.Description, ex.Message);
                return View(contact);
            }


            return RedirectToAction("addContact", new { id = id });
        }

        [Authorize]
        public ActionResult addAddress(string id)
        {
            string[] customerData = HexSerialization.HexToString(id).Split('|');
            ViewBag.customer = id;
            ViewBag.cardCode = customerData[0];
            ViewBag.cardName = customerData[1];

            BusinessPartnerAddressViewModel address = new BusinessPartnerAddressViewModel();

            return View(address);
        }

        [Authorize]
        [HttpPost]
        public ActionResult addAddress(string id, BusinessPartnerAddressViewModel customerAddress)
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
            string[] customerData = HexSerialization.HexToString(id).Split('|');

            try
            {
                customerAddress.cardCode = customerData[0];
                BusinessPartnerAddress customer = new BusinessPartnerAddress()
                {
                    addressType = customerAddress.addType == "S" ? AddressType.ShipTo : AddressType.BillTo,
                    address = customerAddress.address,
                    city = customerAddress.city,
                    street = customerAddress.street,
                    zipCode = customerAddress.zipCode,
                    country = customerAddress.country,
                    county = customerAddress.county,
                    streetNo = customerAddress.streetNo,
                    taxCode = customerAddress.taxCode,
                    cardCode = customerData[0],
                    state = customerAddress.state,
                    UserDefinedFields = new List<UserDefinedField>()
                };

                customer.UserDefinedFields.Add(new UserDefinedField()
                {
                    name = "U_CSS_IVA",
                    type = UdfType.Alphanumeric,
                    value = customerAddress.uCssIva
                });

                backEnd.AddBusinessPartnerAddress(customer, appConnData);


            }
            catch (FaultException<DataAccessFault> ex)
            {
                ViewBag.mensaje = string.Format("Codigo {0} error:{1} {2}", ex.Code, ex.Detail.Description, ex.Message);
                return View(customerAddress);
            }


            return RedirectToAction("addAddress", new { id = id });
        }

        [Authorize]
        public ActionResult addTaxes(string id)
        {
            string[] customerData = HexSerialization.HexToString(id).Split('|');
            ViewBag.customer = id;
            ViewBag.cardCode = customerData[0];
            ViewBag.cardName = customerData[1];

            return View();
        }

        [Authorize]
        [HttpPost]
        public ActionResult addTaxes(string id, BusinessPartnerWithholdingTax customerTax)
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

            string[] customerData = HexSerialization.HexToString(id).Split('|');
            ViewBag.customer = id;
            ViewBag.cardCode = customerData[0];
            ViewBag.cardName = customerData[1];

            try
            {
                customerTax.cardCode = customerData[0];
                backEnd.AddBusinessPartnerWithholdingTax(customerTax, appConnData);
            }
            catch (FaultException<DataAccessFault> ex)
            {
                ViewBag.mensaje = string.Format("Codigo {0} error:{1} {2}", ex.Code, ex.Detail.Description, ex.Message);
                return View(customerTax);
            }

            return RedirectToAction("addTaxes", new { id = id });
        }

        public ActionResult CustomerPaymentAges()
        {
            return View();
        }

        [Authorize]
        public ActionResult paymentAge(string id)
        {
            ViewBag.id = id;
            return View();
        }

        [Authorize]
        public ActionResult smallPaymentAge(string id)
        {
            ViewBag.id = id;
            return View();
        }

        [Authorize]
        public ActionResult lastPrices()
        {
            return View();
        }

        [Authorize]
        public ActionResult searchLastPrices(string id)
        {
            ViewBag.data = id;

            return View();
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
            var data = from c in customers select new[] { c.cardCode, c.cardName };
            return Json(new
            {
                data = data
            }, JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        //[OutputCache(Duration = 3600, VaryByParam = "none")]
        public JsonResult AsyncCustomerGroupList()
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
            List<BusinessPartnerGroup> bpGroups = backEnd.GetAllBusinessPratnerGroup(CardType.Customer, appConnData);
            return Json(bpGroups, JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        //[OutputCache(Duration = 3600, VaryByParam = "none")]
        public JsonResult AsyncCurrencieList()
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
            List<Currency> currencies = backEnd.GetCurrencyList(appConnData);
            return Json(currencies, JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        //[OutputCache(Duration = 3600, VaryByParam = "none")]
        public JsonResult AsyncSalesPersonList()
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
            List<SalesPerson> salesPersons = backEnd.GetSalesPersonList(appConnData);
            return Json(salesPersons, JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        //[OutputCache(Duration = 3600, VaryByParam = "none")]
        public JsonResult AsyncPaymentTermList()
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
            List<PaymentTerm> paymentTerms = backEnd.GetPaymentTermList(appConnData);
            return Json(paymentTerms, JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        //[OutputCache(Duration = 3600, VaryByParam = "none")]
        public JsonResult AsyncContactList(string id)
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
            List<ContactEmployee> contacts = backEnd.GetContactList(id, appConnData);
            var data = from c in contacts select new[] { c.name, c.firstName };

            return Json(new
            {
                data = data
            }, JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        //[OutputCache(Duration = 3600, VaryByParam = "none")]
        public JsonResult AsyncCountryList()
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

            List<Country> countries = backEnd.GetCountryList(appConnData);
            return Json(countries, JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        ////[OutputCache(Duration = 3600, VaryByParam = "none")]
        public JsonResult AsyncSalesTaxCodeList()
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

            List<SalesTaxCode> salesTaxCodes = backEnd.GetSalesTaxCodeList(appConnData);
            return Json(salesTaxCodes, JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        //[OutputCache(Duration = 3600, VaryByParam = "none")]
        public JsonResult AsyncDunningTermsList()
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
            List<BusinessPartnerDunninTerm> dunningTerms = backEnd.GetDunninTermList(appConnData);
            return Json(dunningTerms, JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        //[OutputCache(Duration = 3600, VaryByParam = "none")]
        public JsonResult AsyncStateList(string id)
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
            List<State> states = backEnd.GetStateList(id, appConnData);
            return Json(states, JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        //[OutputCache(Duration = 3600, VaryByParam = "none")]
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

            List<BusinessPartnerAddress> contacts = backEnd.GetAddressList(par[0], par[1] == "S" ? AddressType.ShipTo : AddressType.BillTo, appConnData);
            var data = from c in contacts select new[] { c.address, c.street };

            return Json(new
            {
                data = data
            }, JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        //[OutputCache(Duration = 3600, VaryByParam = "none")]
        public JsonResult AsyncWithholdingTaxList(string id)
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

            List<WithholdingTax> taxes = backEnd.GetWithholdingTax(appConnData);
            List<BusinessPartnerWithholdingTax> customerTaxes = backEnd.GetBusinessPartnerWithholdingTaxList(id, appConnData);
            List<WithholdingTax> taxesAvailable = new List<WithholdingTax>();

            foreach (WithholdingTax item in taxes)
            {
                if (customerTaxes.Where(x => x.wtCode.Equals(item.wtCode)).Count() == 0)
                    taxesAvailable.Add(item);
            }


            return Json(taxesAvailable, JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        //[OutputCache(Duration = 3600, VaryByParam = "none")]
        public JsonResult AsyncBusinessPartnerWithholdingTaxList(string id)
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

            List<WithholdingTax> taxes = backEnd.GetWithholdingTax(appConnData);
            List<BusinessPartnerWithholdingTax> customerTaxes = backEnd.GetBusinessPartnerWithholdingTaxList(id, appConnData);
            List<WithholdingTax> usedTaxes = new List<WithholdingTax>();

            foreach (WithholdingTax item in taxes)
            {
                if (customerTaxes.Where(x => x.wtCode.Equals(item.wtCode)).Count() != 0)
                    usedTaxes.Add(item);
            }


            var data = from c in usedTaxes select new[] { c.wtCode, c.wtName };

            return Json(new
            {
                data = data
            }, JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        //[OutputCache(Duration = 3600, VaryByParam = "none")]
        public JsonResult AsyncGetPaymentAgeList(string id)
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

            var myTimeZone = TimeZoneInfo.FindSystemTimeZoneById("SA Pacific Standard Time");
            var currentDateTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, myTimeZone);

            List<PaymentAge> payments = backEnd.GetPaymentAgeList(id, appConnData);

            var data = from c in payments
                       select new[] 
                       { 
                           //c.cardCode, c.cardName, 
                           string.Format("{0}-{1}", c.seriesName, c.docNum), c.docDate, c.docDueDate, c.pendingTime.ToString(), c.up15.ToString(),
                           c.up30.ToString(), c.up60.ToString(), c.up90.ToString(), c.up120.ToString(), c.up9999.ToString() 
                       };

            return Json(new
            {
                data = data
            }, JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        //[OutputCache(Duration = 3600, VaryByParam = "none")]
        public JsonResult AsyncGetSmallPaymentAgeList(string id)
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

            List<PaymentAge> payments = backEnd.GetPaymentAgeList(id, appConnData);

            var data = from c in payments
                       select new[] 
                       { 
                          string.Format("{0}-{1}-({2})", c.seriesName, c.docNum, c.pendingTime.ToString()), c.docDate, c.pendingToPay.ToString("N", CultureInfo.CreateSpecificCulture("es-CO")), c.numAtCard 
                       };

            return Json(new
            {
                data = data
            }, JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        ////[OutputCache(Duration = 3600, VaryByParam = "none")]
        public JsonResult AsyncGetAddressIvaClassList()
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

            List<UserDefinedFieldValue> ivaClassList = backEnd.GetUserDefinedFieldValuesList("CRD1", "1", appConnData);

            return Json(ivaClassList, JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        //[OutputCache(Duration = 3600, VaryByParam = "none")]
        public JsonResult AsyncGetBusinessPartnerIvaClassList()
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

            List<UserDefinedFieldValue> ivaClassList = backEnd.GetUserDefinedFieldValuesList("OCRD", "111", appConnData);

            return Json(ivaClassList, JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        //[OutputCache(Duration = 3600, VaryByParam = "none")]
        public JsonResult AsyncGetBusinessPartnerSegmentList()
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

            List<UserDefinedFieldValue> ivaClassList = backEnd.GetUserDefinedFieldValuesList("OCRD", "125", appConnData);

            return Json(ivaClassList, JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        //[OutputCache(Duration = 3600, VaryByParam = "none")]
        public JsonResult AsyncGetPersonTypeList()
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

            List<UserDefinedFieldValue> ivaClassList = backEnd.GetUserDefinedFieldValuesList("OCRD", "4", appConnData);

            return Json(ivaClassList, JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        //[OutputCache(Duration = 3600, VaryByParam = "none")]
        public JsonResult AsyncGetTributaryRegList()
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

            List<UserDefinedFieldValue> ivaClassList = backEnd.GetUdoGenericKeyValueList("@BPCO_RT", appConnData);

            return Json(ivaClassList, JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        //[OutputCache(Duration = 3600, VaryByParam = "none")]
        public JsonResult AsyncGetDocumentTypeList()
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

            List<UserDefinedFieldValue> ivaClassList = backEnd.GetUdoGenericKeyValueList("@BPCO_TD", appConnData);

            return Json(ivaClassList, JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        //[OutputCache(Duration = 3600, VaryByParam = "none")]
        public JsonResult AsyncGetCityMagneticMediaList()
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

            List<UserDefinedFieldValue> ivaClassList = backEnd.GetUdoGenericKeyValueList("@BPCO_MU", appConnData);

            return Json(ivaClassList, JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        //[OutputCache(Duration = 3600, VaryByParam = "none")]
        public JsonResult AsyncGetZipCodeList()
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

            List<UserDefinedFieldValue> zipCodeList = backEnd.GetUdoGenericKeyValueList("@CSS_ZONA", appConnData);

            return Json(zipCodeList, JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        //[OutputCache(Duration = 3600, VaryByParam = "none")]
        public JsonResult AsyncGetBusinessPartnerPropList()
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

            List<BusinessPartnerProp> propList = backEnd.GetBusinessPartnerPropList(appConnData);

            return Json(propList, JsonRequestBehavior.AllowGet);
        }

        public JsonResult AsyncListPricesList(string id)
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

            string[] param = id.Split('|');            
            DateTime from = DateTime.Parse(param[0]);
            DateTime to = DateTime.Parse(param[1]);
            string cardCode = param[2];

            List<ItemPrice> prices = backEnd.GetBusinessPartnerLastPricesList(cardCode, from, to, appConnData);

            var data = from c in prices select new[] { c.docDate, c.itemCode, c.itemName, c.quantity.ToString(), c.price.ToString() };
            return Json(new
            {
                data = data
            }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult AsyncPaymentReportType(string id)
        {
            #region User identification
            IIdentity context = HttpContext.User.Identity;            
            int companyId = 0;                        

            if (context.IsAuthenticated)
            {

                System.Web.Security.FormsIdentity ci = (System.Web.Security.FormsIdentity)HttpContext.User.Identity;
                string[] userRole = ci.Ticket.UserData.Split('|');
                companyId = int.Parse(userRole[4]);
            }
            #endregion

            List<CompanyParameter> companyParameters = BizCompanyParameter.GetList(companyId).ToList();
            string ShowSmallPaymentReport = companyParameters.Where(x => x.idParameter.Equals(3)).Select(x => x.value).FirstOrDefault();
            
            
            return Json(ShowSmallPaymentReport, JsonRequestBehavior.AllowGet);
        }

        public JsonResult AsyncCustomerDetails(string id)
        {
            #region User identification
            IIdentity context = HttpContext.User.Identity;
            int companyId = 0;
            AppConnData appConnData = new AppConnData();

            if (context.IsAuthenticated)
            {

                System.Web.Security.FormsIdentity ci = (System.Web.Security.FormsIdentity)HttpContext.User.Identity;
                string[] userRole = ci.Ticket.UserData.Split('|');
                companyId = int.Parse(userRole[4]);
                appConnData = GetAppConnData(companyId);
            }
            #endregion

            BusinessPartner bp = backEnd.GetBusinessPartner(id, appConnData);

            return Json(bp, JsonRequestBehavior.AllowGet);
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