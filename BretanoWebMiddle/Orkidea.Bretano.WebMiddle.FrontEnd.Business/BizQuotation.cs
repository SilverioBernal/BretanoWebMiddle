using Orkidea.Bretano.WebMiddle.FrontEnd.DAL;
using Orkidea.Bretano.WebMiddle.FrontEnd.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orkidea.Bretano.WebMiddle.FrontEnd.Business
{
    public class BizQuotation
    {
        public static IList<OQUT> GetList()
        {
            return DbMngmt<OQUT>.GetList();
        }

        public static IList<OQUT> GetList(DateTime from, DateTime to, int slpCode, int idCompany)
        {
            to = to.AddDays(1);

            return DbMngmt<OQUT>.GetList(x => x.docDate >= from && x.docDate < to && x.slpCode.Equals(slpCode) && x.idCompania.Equals(idCompany) && x.docEntry != null);
        }

        public static IList<QUT1> GetLinesList(int qoutationId)
        {
            return DbMngmt<QUT1>.GetList(c => c.orderId.Equals(qoutationId));
        }

        public static OQUT GetSingle(int id)
        {           
            try
            {
                return DbMngmt<OQUT>.GetSingle(c => c.id.Equals(id));
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static int Add(OQUT qoutation)
        {
            try
            {
                return DbMngmt<OQUT>.Add(qoutation).id;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static void Update(OQUT qoutation)
        {            
            try
            {
                DbMngmt<OQUT>.Update(qoutation);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static void AddLine(QUT1 line)
        {            
            try
            {
                DbMngmt<QUT1>.Add(line);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
