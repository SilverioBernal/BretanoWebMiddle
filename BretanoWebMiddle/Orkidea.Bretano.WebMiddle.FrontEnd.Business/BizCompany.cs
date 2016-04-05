using Orkidea.Bretano.WebMiddle.FrontEnd.DAL;
using Orkidea.Bretano.WebMiddle.FrontEnd.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orkidea.Bretano.WebMiddle.FrontEnd.Business
{
    public static class BizCompany
    {
        public static IList<Company> GetList()
        {
            EntityCRUD<Company> ec = new EntityCRUD<Company>();
            return ec.GetAll();
        }

        public static Company GetSingle(int id)
        {
            EntityCRUD<Company> ec = new EntityCRUD<Company>();
            return ec.GetSingle(c => c.id.Equals(id));
        }

        public static Company GetSingle(string name)
        {
            EntityCRUD<Company> ec = new EntityCRUD<Company>();
            return ec.GetSingle(c => c.name.Equals(name));
        }

        public static void Add(params Company[] Companys)
        {
            EntityCRUD<Company> ec = new EntityCRUD<Company>();

            try
            {
                ec.Add(Companys);

                foreach (Company item in Companys)
                {
                    List<Parameter> parameters = BizParameter.GetList().ToList();

                    CompanyParameter[] cp = new CompanyParameter[parameters.Count()];

                    for (int i = 0; i < parameters.Count(); i++)
                    {
                        cp[i] = new CompanyParameter() { idCompany = item.id, idParameter = parameters[i].id };
                    }

                    BizCompanyParameter.Add(cp);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static void Update(params Company[] Companys)
        {
            EntityCRUD<Company> ec = new EntityCRUD<Company>();

            try
            {
                foreach (Company item in Companys)
                   ec.Update(Companys);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static void Remove(params Company[] Companys)
        {
            EntityCRUD<Company> ec = new EntityCRUD<Company>();
            ec.Remove(Companys);
        }
    }
}
