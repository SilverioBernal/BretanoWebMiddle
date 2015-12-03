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
            EntityCRUD<OQUT> ec = new EntityCRUD<OQUT>();
            return ec.GetAll();
        }

        public static IList<OQUT> GetList(DateTime from, DateTime to, int slpCode, int idCompany)
        {
            to = to.AddDays(1);

            EntityCRUD<OQUT> ec = new EntityCRUD<OQUT>();
            return ec.GetList(x => x.docDate >= from && x.docDate < to && x.slpCode.Equals(slpCode) && x.idCompania.Equals(idCompany) && x.docEntry != null);
        }

        public static IList<QUT1> GetLinesList(int qoutationId)
        {
            EntityCRUD<QUT1> ec = new EntityCRUD<QUT1>();

            return ec.GetList(c => c.orderId.Equals(qoutationId));
        }

        public static OQUT GetSingle(int id)
        {
            EntityCRUD<OQUT> ec = new EntityCRUD<OQUT>();

            try
            {
                return ec.GetSingle(c => c.id.Equals(id));
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static int Add(OQUT qoutation)
        {
            EntityCRUD<OQUT> ec = new EntityCRUD<OQUT>();

            try
            {
                return ec.Add(qoutation).id;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static void Update(OQUT qoutation)
        {
            EntityCRUD<OQUT> ec = new EntityCRUD<OQUT>();

            try
            {
                ec.Update(qoutation);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static void AddLine(QUT1 line)
        {
            EntityCRUD<QUT1> ec = new EntityCRUD<QUT1>();

            try
            {
                ec.Add(line);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
