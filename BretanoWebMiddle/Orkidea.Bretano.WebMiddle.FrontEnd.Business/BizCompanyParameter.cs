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
            EntityCRUD<CompanyParameter> ec = new EntityCRUD<CompanyParameter>();
            return ec.GetAll();
        }

        public static IList<CompanyParameter> GetList(int idCompany)
        {
            EntityCRUD<CompanyParameter> ec = new EntityCRUD<CompanyParameter>();
            return ec.GetList(c => c.idCompany.Equals(idCompany));
        }

        public static CompanyParameter GetSingle(int idCompany, int idParameter)
        {
            EntityCRUD<CompanyParameter> ec = new EntityCRUD<CompanyParameter>();
            return ec.GetSingle(c => c.idCompany.Equals(idCompany) && c.idParameter.Equals(idParameter));
        }        

        public static void Add(params CompanyParameter[] CompanyParameters)
        {
            EntityCRUD<CompanyParameter> ec = new EntityCRUD<CompanyParameter>();

            try
            {                
                    ec.Add(CompanyParameters);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static void Update(params CompanyParameter[] CompanyParameters)
        {
            EntityCRUD<CompanyParameter> ec = new EntityCRUD<CompanyParameter>();

            try
            {                
                    ec.Update(CompanyParameters);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static void Remove(params CompanyParameter[] CompanyParameters)
        {
            EntityCRUD<CompanyParameter> ec = new EntityCRUD<CompanyParameter>();
            ec.Remove(CompanyParameters);
        }
    }
}
