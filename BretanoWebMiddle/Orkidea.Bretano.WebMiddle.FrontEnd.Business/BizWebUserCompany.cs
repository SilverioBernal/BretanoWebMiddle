using Orkidea.Bretano.WebMiddle.FrontEnd.DAL;
using Orkidea.Bretano.WebMiddle.FrontEnd.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orkidea.Bretano.WebMiddle.FrontEnd.Business
{
    public static class BizWebUserCompany
    {
        public static IList<WebUserCompany> GetList()
        {
            EntityCRUD<WebUserCompany> ec = new EntityCRUD<WebUserCompany>();
            return ec.GetAll();
        }

        public static WebUserCompany GetSingle(int webUserId, int companyId)
        {
            EntityCRUD<WebUserCompany> ec = new EntityCRUD<WebUserCompany>();
            return ec.GetSingle(c => c.webUserId.Equals(webUserId) && c.companyId.Equals(companyId));
        }        

        public static void Add(params WebUserCompany[] WebUserCompanys)
        {
            EntityCRUD<WebUserCompany> ec = new EntityCRUD<WebUserCompany>();

            try
            {
                foreach (WebUserCompany item in WebUserCompanys)
                    ec.Add(WebUserCompanys);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static void Update(params WebUserCompany[] WebUserCompanys)
        {
            EntityCRUD<WebUserCompany> ec = new EntityCRUD<WebUserCompany>();

            try
            {
                foreach (WebUserCompany item in WebUserCompanys)
                    ec.Update(WebUserCompanys);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static void Remove(params WebUserCompany[] WebUserCompanys)
        {
            EntityCRUD<WebUserCompany> ec = new EntityCRUD<WebUserCompany>();
            ec.Remove(WebUserCompanys);
        }
    }
}
