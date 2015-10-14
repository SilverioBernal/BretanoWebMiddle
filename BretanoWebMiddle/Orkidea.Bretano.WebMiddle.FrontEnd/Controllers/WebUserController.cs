using Orkidea.Bretano.WebMiddle.FrontEnd.Business;
using Orkidea.Bretano.WebMiddle.FrontEnd.Business.Enums;
using Orkidea.Bretano.WebMiddle.FrontEnd.Entities;
using Orkidea.Framework.Security;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Orkidea.Bretano.WebMiddle.FrontEnd.Controllers
{
    public class WebUserController : Controller
    {
        //
        // GET: /WebUser/
        [Authorize]
        public ActionResult Index()
        {            
            return View(BizWebUser.GetList());
        }

        //
       
        //
        // GET: /WebUser/Create
        [Authorize]
        public ActionResult Create()
        {
            int specialUser = int.Parse(Cryptography.Decrypt(HexSerialization.HexToString(ConfigurationManager.AppSettings["SpecialUser"])));
            int standarlUser = int.Parse(Cryptography.Decrypt(HexSerialization.HexToString(ConfigurationManager.AppSettings["StandarUser"])));

            int totalUsers = BizWebUser.GetList().Where(x => x.active).Count();

            if (totalUsers >= (specialUser + standarlUser))
                return RedirectToAction("index");

            return View();
        }

        //
        // POST: /WebUser/Create
        [HttpPost]
        [Authorize]
        public ActionResult Create(WebUser user)
        {
            try
            {
                user.pass = HexSerialization.StringToHex(Cryptography.Encrypt(user.pass));
                BizWebUser.Add(user);
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /WebUser/Edit/5
        [Authorize]
        public ActionResult Enable(int id)
        {
            try
            {
                WebUser user = BizWebUser.GetSingle(id);
                BizWebUser.Update(WebUserAction.Enable, user);
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // POST: /WebUser/Edit/5        
        [Authorize]
        public ActionResult Disable(int id)
        {
            try
            {
                WebUser user = BizWebUser.GetSingle(id);                
                BizWebUser.Update(WebUserAction.Disable, user);
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /WebUser/Delete/5
        [Authorize]
        public ActionResult Delete(int id)
        {
            WebUser user = BizWebUser.GetSingle(id);
            BizWebUser.Remove(user);
            return RedirectToAction("Index");
        }

        [Authorize]
        public ActionResult Restore(int id)
        {
            WebUser user = BizWebUser.GetSingle(id);
            BizWebUser.Update(WebUserAction.UserReset,user);
            return RedirectToAction("Index");
        }

        [Authorize]
        public ActionResult Asociate(int id) 
        {
            WebUser user = BizWebUser.GetSingle(id);

            ViewBag.WebUser = user.name;
            ViewBag.WebUserId = user.id;
            return View();
        }

        [HttpPost]
        [Authorize]
        public ActionResult Asociate(int id, WebUserCompany company)
        {
            company.webUserId = id;

            BizWebUserCompany.Add(company);
            return RedirectToAction("Asociate", new { id = id });
        }

        [Authorize]
        public ActionResult Deasociate(string id)
        {
            string[] data = id.Split('|');

            BizWebUserCompany.Remove(new WebUserCompany() { webUserId = int.Parse( data[0]), companyId = int.Parse(data[1]) });

            return RedirectToAction("Asociate", new { id = int.Parse(data[0]) });
        }

        [Authorize]
        public JsonResult AsyncFreeCompanyList(int id)
        {            
            List<Company> companies = BizCompany.GetList().ToList();
            List<WebUserCompany> usedCompanies = BizWebUserCompany.GetList().ToList();
            usedCompanies = usedCompanies.Where(x => x.webUserId.Equals(id)).ToList();

            List<Company> freeCompanies = new List<Company>();

            foreach (Company item in companies)
            {
                if (usedCompanies.Where(x => x.companyId.Equals(item.id)).Count() == 0)
                    freeCompanies.Add(item);
            }

            return Json(freeCompanies, JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        public JsonResult AsyncUsedCompanyList(int id)
        {
            List<Company> companies = BizCompany.GetList().ToList();
            List<WebUserCompany> userCompanies = BizWebUserCompany.GetList().Where(x => x.webUserId.Equals(id)).ToList();
            List<Company> usedCompanies= new List<Company>();

            foreach (Company item in companies)
            {
                if (userCompanies.Where(x => x.companyId.Equals(item.id)).Count() != 0)
                    usedCompanies.Add(item);
            }

            var data = from c in usedCompanies select new[] { c.id.ToString(), c.name };

            return Json(new
            {
                data = data
            }, JsonRequestBehavior.AllowGet);
        }
    }
}
