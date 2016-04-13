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

        public static IList<ORDR> GetList(DateTime from, DateTime to, int slpCode, int idCompany)
        {
            to = to.AddDays(1);

            EntityCRUD<ORDR> ec = new EntityCRUD<ORDR>();
            return ec.GetList(x => x.docDate >= from && x.docDate < to && x.slpCode.Equals(slpCode) && x.idCompania.Equals(idCompany) && x.docEntry!= null);
        }

        public static IList<ORDR> GetPendingList(string idWebUser, int idCompany)
        {            
            EntityCRUD<ORDR> ec = new EntityCRUD<ORDR>();
            return ec.GetList(x => x.uOrkUsuarioWeb == idWebUser && x.idCompania.Equals(idCompany) && x.docEntry == null);
        }

        public static IList<ORDR> GetPendingList(string idWebUser, int idCompany, int daysOld)
        {
            EntityCRUD<ORDR> ec = new EntityCRUD<ORDR>();

            IList<ORDR> orders = ec.GetList(x => x.uOrkUsuarioWeb == idWebUser && x.idCompania.Equals(idCompany) && (DateTime.Now - x.docDate).TotalDays <= daysOld);

            return orders;

        }

        public static IList<ORDR> GetPendingList(int idCompany)
        {
            EntityCRUD<ORDR> ec = new EntityCRUD<ORDR>();
            return ec.GetList(x => x.idCompania.Equals(idCompany) && x.docEntry == null);
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

        public static void Update(ORDR order)
        {
            EntityCRUD<ORDR> ec = new EntityCRUD<ORDR>();

            try
            {
                ec.Update(order);
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

        public static void Delete(ORDR order)
        {
            EntityCRUD<ORDR> ec = new EntityCRUD<ORDR>();

            try
            {
                DeleteQueue(order.id);

                List<RDR1> orderLines = GetLinesList(order.id).ToList();

                foreach (RDR1 item in orderLines)
                {
                    DeleteLine(item);
                }

                ec.Remove(order);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static void DeleteLine(RDR1 orderLine)
        {
            EntityCRUD<RDR1> ec = new EntityCRUD<RDR1>();

            try
            {
                RDR1 line = ec.GetSingle(x => x.orderId.Equals(orderLine.orderId) && x.itemCode == orderLine.itemCode);
                ec.Remove(line);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static void DeleteQueue(int id)
        {
            ProcessQueue queue = BizProcessQueue.GetSingle(id.ToString());

            BizProcessQueue.Remove(queue);
        }
    }
}
