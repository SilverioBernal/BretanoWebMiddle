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
            return DbMngmt<WebUserCompany>.GetList();
        }

        public static WebUserCompany GetSingle(int webUserId, int companyId)
        {            
            return DbMngmt<WebUserCompany>.GetSingle(c => c.webUserId.Equals(webUserId) && c.companyId.Equals(companyId));
        }

        public static void Add(params WebUserCompany[] WebUserCompanys)
        {
            try
            {
                foreach (WebUserCompany item in WebUserCompanys)
                    DbMngmt<WebUserCompany>.Add(WebUserCompanys);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static void Update(params WebUserCompany[] WebUserCompanys)
        {
            try
            {
                foreach (WebUserCompany item in WebUserCompanys)
                    DbMngmt<WebUserCompany>.Update(WebUserCompanys);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static void Remove(params WebUserCompany[] WebUserCompanys)
        {
            DbMngmt<WebUserCompany>.Remove(WebUserCompanys);
        }
    }
}
