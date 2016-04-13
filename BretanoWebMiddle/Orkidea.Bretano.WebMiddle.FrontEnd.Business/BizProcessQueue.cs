using Orkidea.Bretano.WebMiddle.FrontEnd.DAL;
using Orkidea.Bretano.WebMiddle.FrontEnd.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orkidea.Bretano.WebMiddle.FrontEnd.Business
{
    public static class BizProcessQueue
    {
        public static IList<ProcessQueue> GetList()
        {
            EntityCRUD<ProcessQueue> ec = new EntityCRUD<ProcessQueue>();
            return ec.GetAll();
        }

        public static IList<ProcessQueue> GetList(bool status)
        {
            EntityCRUD<ProcessQueue> ec = new EntityCRUD<ProcessQueue>();
            if (status)
                return ec.GetList(x => x.processed != null).ToList();
            else
                return ec.GetList(x => x.processed == null).ToList();
        }

        public static IList<ProcessQueue> GetList(int companyId, int daysOld)
        {
            EntityCRUD<ProcessQueue> ec = new EntityCRUD<ProcessQueue>();

            return ec.GetList(x => x.idCompany.Equals(companyId) && (DateTime.Now - x.addedToQueue).TotalDays <= daysOld).ToList();
        }

        public static ProcessQueue GetSingle(int id)
        {
            EntityCRUD<ProcessQueue> ec = new EntityCRUD<ProcessQueue>();
            return ec.GetSingle(c => c.id.Equals(id));
        }

        public static ProcessQueue GetSingle(string targetId)
        {
            int id = int.Parse(targetId);
            EntityCRUD<ProcessQueue> ec = new EntityCRUD<ProcessQueue>();
            return ec.GetSingle(c => c.idTarget.Equals(id));
        }

        public static void Add(params ProcessQueue[] Parameters)
        {
            EntityCRUD<ProcessQueue> ec = new EntityCRUD<ProcessQueue>();

            try
            {
                foreach (ProcessQueue item in Parameters)
                {
                    ec.Add(Parameters);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static void Update(ProcessQueue processQueue)
        {
            EntityCRUD<ProcessQueue> ec = new EntityCRUD<ProcessQueue>();

            try
            {
                ec.Update(processQueue);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static void Remove(ProcessQueue processQueue)
        {
            EntityCRUD<ProcessQueue> ec = new EntityCRUD<ProcessQueue>();
            ec.Remove(processQueue);
        }
    }
}
