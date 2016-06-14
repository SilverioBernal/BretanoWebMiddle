using Orkidea.Bretano.WebMiddle.FrontEnd.DAL;
using Orkidea.Bretano.WebMiddle.FrontEnd.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orkidea.Bretano.WebMiddle.FrontEnd.Business
{
    public static class BizCompanyParameter
    {
        public static IList<CompanyParameter> GetList()
        {            
            return DbMngmt<CompanyParameter>.GetList();
        }

        public static IList<CompanyParameter> GetList(int idCompany)
        {
            return DbMngmt<CompanyParameter>.GetList(c => c.idCompany.Equals(idCompany));
        }

        public static CompanyParameter GetSingle(int idCompany, int idParameter)
        {
            return DbMngmt<CompanyParameter>.GetSingle(c => c.idCompany.Equals(idCompany) && c.idParameter.Equals(idParameter));
        }        

        public static void Add(params CompanyParameter[] CompanyParameters)
        {
            try
            {
                DbMngmt<CompanyParameter>.Add(CompanyParameters);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static void Update(params CompanyParameter[] CompanyParameters)
        {
            try
            {
                DbMngmt<CompanyParameter>.Update(CompanyParameters);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static void Remove(params CompanyParameter[] CompanyParameters)
        {
            DbMngmt<CompanyParameter>.Remove(CompanyParameters);
        }
    }
}
