using Orkidea.Bretano.WebMiddle.FrontEnd.Business;
using Orkidea.Bretano.WebMiddle.FrontEnd.Entities;
using Orkidea.Framework.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Orkidea.Bretano.WebMiddle.FrontEnd.Controllers
{
    public class CompanyController : Controller
    {
        //
        // GET: /Company/
        [Authorize]
        public ActionResult Index()
        {
            List<Company> companies = BizCompany.GetList().ToList();
            return View(companies);
        }
        
        //
        // GET: /Company/Create
        [Authorize]
        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Company/Create
        [HttpPost]
        [Authorize]
        public ActionResult Create(Company company)
        {
            try
            {
                company.dataBaseName = HexSerialization.StringToHex(Cryptography.Encrypt(company.dataBaseName));
                company.connStringName = HexSerialization.StringToHex(Cryptography.Encrypt(company.connStringName));
                company.sapUser = HexSerialization.StringToHex(Cryptography.Encrypt(company.sapUser));
                company.sapPassword = HexSerialization.StringToHex(Cryptography.Encrypt(company.sapPassword));

                BizCompany.Add(company);

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /Company/Edit/5
        [Authorize]
        public ActionResult Edit(int id)
        {
            Company company = BizCompany.GetSingle(id);

            company.dataBaseName = Cryptography.Decrypt(HexSerialization.HexToString(company.dataBaseName));
            company.connStringName = Cryptography.Decrypt(HexSerialization.HexToString(company.connStringName));
            company.sapUser = Cryptography.Decrypt(HexSerialization.HexToString(company.sapUser));
            company.sapPassword = Cryptography.Decrypt(HexSerialization.HexToString(company.sapPassword));

            return View(company);
        }

        //
        // POST: /Company/Edit/5
        [HttpPost]
        [Authorize]
        public ActionResult Edit(int id, Company company)
        {
            try
            {
                company.dataBaseName = HexSerialization.StringToHex(Cryptography.Encrypt(company.dataBaseName));
                company.connStringName = HexSerialization.StringToHex(Cryptography.Encrypt(company.connStringName));
                company.sapUser = HexSerialization.StringToHex(Cryptography.Encrypt(company.sapUser));
                company.sapPassword = HexSerialization.StringToHex(Cryptography.Encrypt(company.sapPassword));
                company.id = id;

                BizCompany.Update(company);

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /Company/Delete/5
        [Authorize]
        public ActionResult Delete(int id)
        {
            BizCompany.Remove(new Company() { id = id });

            return RedirectToAction("Index");
        }        
    }
}
