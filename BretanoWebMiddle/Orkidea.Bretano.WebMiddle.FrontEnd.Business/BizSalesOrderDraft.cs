using Orkidea.Bretano.WebMiddle.FrontEnd.DAL;
using Orkidea.Bretano.WebMiddle.FrontEnd.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orkidea.Bretano.WebMiddle.FrontEnd.Business
{
    public class BizSalesOrderDraft
    {
        public static IList<ORDR> GetList()
        {
            EntityCRUD<ORDR> ec = new EntityCRUD<ORDR>();
            return ec.GetAll();
        }

        public static IList<RDR1> GetLinesList(int orderId)
        {
            EntityCRUD<RDR1> ec = new EntityCRUD<RDR1>();
            return ec.GetList(c => c.orderId.Equals(orderId));
        }

        public static ORDR GetSingle(int id)
        {
            EntityCRUD<ORDR> ec = new EntityCRUD<ORDR>();

            try
            {
                return ec.GetSingle(c => c.id.Equals(id));
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static int Add(ORDR order)
        {
            EntityCRUD<ORDR> ec = new EntityCRUD<ORDR>();

            try
            {                
                    return ec.Add(order).id;                
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static void AddLine(RDR1 line)
        {
            EntityCRUD<RDR1> ec = new EntityCRUD<RDR1>();

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
