using Orkidea.Framework.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Security.Principal;
using System.Web.Security;
using Orkidea.Bretano.WebMiddle.FrontEnd.Models;
using Orkidea.Bretano.WebMiddle.FrontEnd.Entities;
using Orkidea.Bretano.WebMiddle.FrontEnd.Business;
using System.Configuration;

namespace Orkidea.Bretano.WebMiddle.FrontEnd.Controllers
{
    public class SecurityController : Controller
    {
        //
        // GET: /Security/
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            LoginViewModel model = new LoginViewModel();
            //model.companies = BizCompany.GetAll();//BizCompany.GetList().ToList();
            model.companies = BizCompany.GetList().ToList();

            ViewBag.ReturnUrl = returnUrl;
            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult Login(LoginViewModel model, string returnUrl)
        {

            if (!String.IsNullOrEmpty(model.UserName) && !String.IsNullOrEmpty(model.Password))
            {
                //user exists
                #region user exists
                WebUser userTarget = null;
                string contraseñaDesencriptada = "";

                if (model.UserName.ToLower() != "root")
                {
                    userTarget = BizWebUser.GetSingle(model.UserName);

                    if (userTarget == null)
                    {
                        ViewBag.ErrorMessage = "Usuario no existe";
                        model.companies = BizCompany.GetList().ToList();
                        return View(model);
                    }
                    else
                    {
                        if (!userTarget.active)
                        {
                            ViewBag.ErrorMessage = "Usuario inactivo";
                            model.companies = BizCompany.GetList().ToList();
                            return View(model);
                        }
                    }
                }
                else
                {
                    userTarget = new WebUser()
                    {
                        id = 999999,
                        name = "root",
                        pass = ConfigurationManager.AppSettings["RootKey"].ToString()
                    };
                }
                #endregion

                contraseñaDesencriptada = Cryptography.Decrypt(HexSerialization.HexToString(userTarget.pass));

                //authenticacion
                if (model.Password.Equals(contraseñaDesencriptada))
                {
                    WebUserCompany webUserCompany = null;

                    //Authorization
                    if (model.UserName.ToLower() != "root")
                        webUserCompany = BizWebUserCompany.GetSingle(userTarget.id, model.companyId);
                    else
                        webUserCompany = new WebUserCompany()
                        {
                            webUserId = 999999,
                            companyId = model.companyId,
                            admin = true,
                            customerCreator = true,
                            purchaseOrderCreator = true,
                            supervisor = true,
                            orderApprover = true,
                            slpCode = 0
                        };

                    if (webUserCompany == null)
                    {
                        model.companies = BizCompany.GetList().ToList();
                        return View(model);
                    }

                    #region SESSION OBJECTS
                    //List<CompanyParameter> companyParameters = BizCompanyParameter.GetList(model.companyId).ToList();

                    //Session["companyParameters"] = companyParameters;

                    Company co = BizCompany.GetSingle(model.companyId);


                    FormsAuthentication.SetAuthCookie(model.UserName, false);

                    int id = userTarget.id;
                    int isAdmin = webUserCompany.admin ? 1 : 0;
                    int customerCreator = webUserCompany.customerCreator ? 1 : 0;
                    int purchaseOrderCreator = webUserCompany.purchaseOrderCreator ? 1 : 0;
                    int orderApprover = webUserCompany.orderApprover ? 1 : 0;
                    int company = webUserCompany.companyId;
                    int slpCode = webUserCompany.slpCode;
                    int supervisor = webUserCompany.supervisor ? 1 : 0;

                    string userData = string.Format("{0}|{1}|{2}|{3}|{4}|{5}|{6}|{7}|{8}",
                        id.ToString().Trim(),
                        isAdmin.ToString().Trim(),
                        customerCreator.ToString().Trim(),
                        purchaseOrderCreator.ToString().Trim(),
                        company,
                        slpCode.ToString().Trim(),
                        co.name,
                        orderApprover.ToString().Trim(),
                        supervisor.ToString().Trim()
                        );

                    FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(1, model.UserName, DateTime.Now, DateTime.Now.AddMinutes(30), false, userData);

                    string encTicket = FormsAuthentication.Encrypt(ticket);
                    HttpCookie faCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encTicket);
                    HttpContext.Response.Cookies.Add(faCookie);

                    HttpCookie authCookie = Request.Cookies[FormsAuthentication.FormsCookieName];

                    if (authCookie != null)
                    {
                        FormsAuthenticationTicket authTicket = FormsAuthentication.Decrypt(authCookie.Value);
                        CustomIdentity identity = new CustomIdentity(authTicket.Name, userData);
                        GenericPrincipal newUser = new GenericPrincipal(identity, new string[] { });
                        HttpContext.User = newUser;
                    }
                    #endregion
                    //ActivityLogBiz.SaveActivityLog(new ActivityLog() { idUsuario = id, accion = "Login", fecha = DateTime.Now });
                    if (!string.IsNullOrEmpty(returnUrl))
                        return RedirectToLocal(returnUrl);
                    else
                        return RedirectToAction("index", "Home");
                }
            }
            model.companies = BizCompany.GetList().ToList();
            return View(model);
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                //return RedirectToAction("Index", "Home");
                return RedirectToAction
                ("Login");
            }
        }

        [Authorize]
        public ActionResult Logout()
        {
            Session.Abandon();
            FormsAuthentication.SignOut();

            return RedirectToAction("Index", "Home");
        }

        //[Authorize]
        //public ActionResult ChangePassword()
        //{
        //    UserBiz userBiz = new UserBiz();

        //    User user = userBiz.GetUserbyName(new User() { usuario = HttpContext.User.Identity.Name });
        //    vmUser oUser = new vmUser() { id = user.id, usuario = user.usuario, admin = user.admin };
        //    user.clave = "";
        //    return View(oUser);
        //}

        //[Authorize]
        //[HttpPost]
        //public ActionResult ChangePassword(User userTarget)
        //{
        //    string rootPath = Server.MapPath("~");
        //    UserBiz userBiz = new UserBiz();

        //    User user = userBiz.GetUserbyName(new User() { usuario = HttpContext.User.Identity.Name });

        //    user.clave = Cryptography.Encrypt(userTarget.clave);

        //    userBiz.SaveUser(user, rootPath);

        //    return RedirectToAction("Index", "Home");
        //}
    }
}