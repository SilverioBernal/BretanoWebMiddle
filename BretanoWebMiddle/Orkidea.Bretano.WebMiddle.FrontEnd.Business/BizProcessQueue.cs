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
            return DbMngmt<ProcessQueue>.GetList();
        }

        public static IList<ProcessQueue> GetList(bool status)
        {
            if (status)
                return DbMngmt<ProcessQueue>.GetList(x => x.processed != null).ToList();
            else
                return DbMngmt<ProcessQueue>.GetList(x => x.processed == null).ToList();
        }

        public static IList<ProcessQueue> GetList(int companyId, int daysOld)
        {            
            //return ec.GetList(x => x.idCompany.Equals(companyId) && (DateTime.Now - x.addedToQueue).TotalDays <= daysOld).ToList();
            return DbMngmt<ProcessQueue>.executeSqlQueryToList(string.Format("Select * from ProcessQueue where idCompany = {0} and datediff(dd, addedToQueue, getdate()) <= {1}", companyId.ToString(), daysOld));
        }

        public static ProcessQueue GetSingle(int id)
        {
            return DbMngmt<ProcessQueue>.GetSingle(c => c.id.Equals(id));
        }

        public static ProcessQueue GetSingle(string targetId)
        {
            int id = int.Parse(targetId);                        
            //return ec.GetSingle(c => c.idTarget.Equals(id));
            return DbMngmt<ProcessQueue>.executeSqlQuerySingle(string.Format("Select * from ProcessQueue where idTarget = {0}", id.ToString()));
        }

        public static void Add(params ProcessQueue[] Parameters)
        {
            try
            {
                foreach (ProcessQueue item in Parameters)
                {
                    DbMngmt<ProcessQueue>.Add(Parameters);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static void Update(ProcessQueue processQueue)
        {
            try
            {
                DbMngmt<ProcessQueue>.Update(processQueue);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static void Remove(ProcessQueue processQueue)
        {
            DbMngmt<ProcessQueue>.Remove(processQueue);
        }
    }
}
