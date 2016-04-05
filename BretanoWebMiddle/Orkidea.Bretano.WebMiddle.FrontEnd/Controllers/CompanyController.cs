using Orkidea.Bretano.WebMiddle.FrontEnd.Business;
using Orkidea.Bretano.WebMiddle.FrontEnd.Entities;
using Orkidea.Bretano.WebMiddle.FrontEnd.Models;
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

        [Authorize]
        public ActionResult Setup(int id)
        {
            Company company = BizCompany.GetSingle(id);

            List<Parameter> parameters = BizParameter.GetList().ToList();
            List<CompanyParameter> companyParameters = BizCompanyParameter.GetList(id).ToList();
            List<CompanyParameterViewModel> comParameters = new List<CompanyParameterViewModel>();

            #region Parameter Sync
            if (companyParameters.Count().Equals(0))
            {
                CompanyParameter[] cp = new CompanyParameter[parameters.Count()];

                for (int i = 0; i < parameters.Count(); i++)
                {
                    cp[i] = new CompanyParameter() { idCompany = id, idParameter = parameters[i].id };
                }

                BizCompanyParameter.Add(cp);

                companyParameters = BizCompanyParameter.GetList(id).ToList();
            }

            if (companyParameters.Count() < parameters.Count())
            {
                CompanyParameter[] cp = new CompanyParameter[parameters.Count() - companyParameters.Count()];

                int index = 0;

                for (int i = 0; i < parameters.Count(); i++)
                {                    
                    if (companyParameters.Where(x => x.idParameter.Equals(parameters[i].id)).Count().Equals(0))
                    {
                        cp[index] = new CompanyParameter() { idCompany = id, idParameter = parameters[i].id };
                        index++;
                    }                    
                }

                BizCompanyParameter.Add(cp);

                companyParameters = BizCompanyParameter.GetList(id).ToList();
            }
            #endregion

            foreach (Parameter item in parameters)
            {
                string value = "";

                if (item.type != "EncryptedText")
                    value = companyParameters.Where(x => x.idParameter.Equals(item.id)).Select(x => x.value).FirstOrDefault();
                else
                {
                    value = companyParameters.Where(x => x.idParameter.Equals(item.id)).Select(x => x.value).FirstOrDefault();

                    if (!string.IsNullOrEmpty(value))
                        value = Cryptography.Decrypt(HexSerialization.HexToString(value));
                    else
                        value = "";
                }

                comParameters.Add(new CompanyParameterViewModel()
                {
                    id = item.id,
                    label = item.label,
                    name = item.name,
                    type = item.type,
                    value = value
                });
            }
            //Cryptography.Decrypt(HexSerialization.HexToString(company.dataBaseName));
            ViewBag.idCompany = id;
            ViewBag.companyName = company.name;

            return View(comParameters);
        }

        public JsonResult SaveCompanySettings(List<CompanyParameter> CompanyParameters)
        {
            string res = "";

            try
            {
                CompanyParameter[] coParameters = new CompanyParameter[CompanyParameters.Count()];
                List<Parameter> parameters = BizParameter.GetList().ToList();

                for (int i = 0; i < CompanyParameters.Count(); i++)
                {
                    string type = parameters.Where(x => x.id.Equals(CompanyParameters[i].idParameter)).Select(x => x.type).FirstOrDefault();

                    if (type == "EncryptedText")
                    {
                        CompanyParameters[i].value = HexSerialization.StringToHex(Cryptography.Encrypt(CompanyParameters[i].value));
                    }
                    coParameters[i] = CompanyParameters[i];
                }

                BizCompanyParameter.Update(coParameters);

                res = "OK";
            }
            catch (Exception ex)
            {
                res = string.Format("Se presento un error inesperado Detalles {0}", ex.Message);
            }

            return Json(res, JsonRequestBehavior.AllowGet);
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
