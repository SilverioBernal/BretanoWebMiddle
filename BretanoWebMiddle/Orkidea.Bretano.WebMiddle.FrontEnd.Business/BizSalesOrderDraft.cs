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
            return DbMngmt<ORDR>.GetList();
        }

        public static IList<ORDR> GetList(DateTime from, DateTime to, int slpCode, int idCompany)
        {
            to = to.AddDays(1);
            //return DbMngmt<ORDR>.executeSqlQueryToList(string.Format("Select * from ordr where idCompania = {0} and slpcode = {1} and docdate between '{2}' and '{3}' ", idCompany, slpCode.ToString(), from.ToString("yyyy-MM-dd"), to.ToString("yyyy-MM-dd")));
            return DbMngmt<ORDR>.executeSqlQueryToList(string.Format("Select * from ordr where idCompania = {0} and slpcode = {1} and docentry is not null and docdate between '{2}' and '{3}' ", idCompany, slpCode.ToString(), from.ToString("yyyy-MM-dd"), to.ToString("yyyy-MM-dd")));
        }

        public static IList<ORDR> GetList(List<int> orderIds)
        {
            StringBuilder ids = new StringBuilder();

            for (int i = 0; i < orderIds.Count(); i++)
            {
                if (i == orderIds.Count() - 1)
                    ids.Append(string.Format("{0}", orderIds[i]));
                else
                    ids.Append(string.Format("{0}, ", orderIds[i]));
            }

            return DbMngmt<ORDR>.executeSqlQueryToList(string.Format("Select * from ordr where id in({0})", ids.ToString()));
            //return ec.GetList(x => x.idCompania.Equals(idCompany) && x.docEntry == null);
        }

        public static IList<ORDR> GetPendingList(string idWebUser, int idCompany)
        {
            //return DbMngmt<ORDR>.GetList(x => x.uOrkUsuarioWeb == idWebUser && x.idCompania.Equals(idCompany) && x.docEntry == null);

            IList<ORDR> orders = DbMngmt<ORDR>.executeSqlQueryToList(string.Format("Select * from ordr where uOrkUsuarioWeb = '{0}' and idCompania = {1} and docentry is null and (draftDM is not null or draftLC is not null or draftPB is not null) ", idWebUser, idCompany));
            return orders;
        }

        public static IList<ORDR> GetPendingList(string idWebUser, int idCompany, int daysOld)
        {
            IList<ORDR> orders = DbMngmt<ORDR>.executeSqlQueryToList(string.Format("Select * from ordr where uOrkUsuarioWeb = '{0}' and idCompania = {1} and datediff(dd, docdate, getdate()) <= {2}", idWebUser, idCompany, daysOld));
            return orders;

        }

        public static IList<ORDR> GetPendingList(int idCompany)
        {
            IList<ORDR> orders = DbMngmt<ORDR>.executeSqlQueryToList(string.Format("Select * from ordr where idCompania = {0} and docentry is null and (draftDM is not null or draftLC is not null or draftPB is not null)", idCompany));
            return orders;
            //return DbMngmt<ORDR>.GetList(x => x.idCompania.Equals(idCompany) && x.docEntry == null);//.Where(x => x.draftDM.Equals(true) || x.draftLC.Equals(true) || x.draftPB.Equals(true)).ToList();
        }



        public static IList<RDR1> GetLinesList(int orderId)
        {
            return DbMngmt<RDR1>.executeSqlQueryToList(string.Format("Select * from rdr1 where orderId = {0}", orderId.ToString()));            
        }

        public static IList<RDR1> GetLinesList(List<int> orderIds)
        {            
            StringBuilder ids = new StringBuilder();

            for (int i = 0; i < orderIds.Count(); i++)
            {
                if (i == orderIds.Count() - 1)
                    ids.Append(string.Format("{0}", orderIds[i]));
                else
                    ids.Append(string.Format("{0}, ", orderIds[i]));
            }

            return DbMngmt<RDR1>.executeSqlQueryToList(string.Format("Select * from rdr1 where orderId in({0})", ids.ToString()));

            //return ec.GetList(c => c.orderId.Equals(orderId));
        }

        public static IList<AuthReport> GetAuthReport(int companyId, DateTime from, DateTime to)
        {
            List<AuthReport> report = new List<AuthReport>();
            to = to.AddDays(1);

            List<ORDR> orders = DbMngmt<ORDR>.GetList(x => 
                x.idCompania == companyId && 
                x.docDate >= from && x.docDate < to
                ).ToList();

            foreach (ORDR ordr in orders)
            {
                if (ordr.draftDM != null || ordr.draftLC != null || ordr.draftPB != null)
                {
                    AuthReport auth = new AuthReport()
                    {
                        authComments = ordr.authComments,
                        authDate = ordr.authDate,
                        cardCode = ordr.cardCode,
                        cardName = ordr.cardName,
                        docDate = ordr.docDate,
                        draftComments = ordr.draftComments,
                        status = ordr.authStatus == null ? "Pendiente" : ((bool)ordr.authStatus ? "Aprobado" : "Rechazado"),
                        docEntry = ordr.docEntry,
                        id = ordr.id
                    };

                    if (ordr.authUser != null)
                    {
                        try
                        {
                            int idUser = int.Parse(ordr.authUser);
                            auth.authUser = DbMngmt<WebUser>.GetSingle(x => x.id.Equals(idUser)).name;
                        }
                        catch (Exception)
                        {
                            auth.authUser = "No disponible";
                        }
                    }

                    List<RDR1> lines = DbMngmt<RDR1>.GetList(x => x.orderId.Equals(ordr.id)).ToList();

                    decimal subTotal = 0, iva = 0, total = 0;

                    foreach (RDR1 line in lines)
                    {
                        subTotal += (line.price * line.quantity);
                        iva += line.taxRate == null ? 0 : (line.price * line.quantity) * ((decimal)line.taxRate / 100);
                        total += subTotal + iva;
                    }

                    auth.subtotalDoc = subTotal;
                    auth.ivaDoc = iva;
                    auth.totalDoc = total;

                    report.Add(auth);
                }
            }


            return report;
        }

        public static IList<SalesResume> GetSalesResumeList(DateTime from, DateTime to, int companyId)
        {            
            StringBuilder oSql = new StringBuilder();

            oSql.Append("select d.name userName, count(a.id) salesCount , sum(b.price * b.quantity) salesValue  from ordr a inner join rdr1 b on a.id = b.orderId ");
            oSql.Append("inner join webusercompany c on a.idCompania = c.companyId and a.uOrkUsuarioWeb = c.webUserId and a.slpCode = c.slpCode ");
            oSql.Append("inner join webUser d on c.webUserId = d.id ");
            oSql.Append(string.Format("where idcompania = {0} and a.docentry is not null and a.docDate between '{1}' and '{2}'", companyId, from.ToString("yyyy-MM-dd"), to.ToString("yyyy-MM-dd")));
            oSql.Append("group by d.name");

            return DbMngmt<SalesResume>.executeSqlQueryToList(oSql.ToString());

            //return ec.GetList(c => c.orderId.Equals(orderId));
        }

        public static ORDR GetSingle(int id)
        {
            try
            {
                //return ec.GetSingle(c => c.id.Equals(id));
                return DbMngmt<ORDR>.executeSqlQuerySingle(string.Format("Select * from ordr where id = {0}", id.ToString()));
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static RDR1 GetSingleLine(int orderId, string itemCode)
        {
            try
            {
                //return ec.GetSingle(c => c.id.Equals(id));
                return DbMngmt<RDR1>.executeSqlQuerySingle(string.Format("Select * from rdr1 where orderId = {0} and itemCode = '{1}'", orderId.ToString(), itemCode));
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static int Add(ORDR order)
        {            
            try
            {
                var myTimeZone = TimeZoneInfo.FindSystemTimeZoneById("SA Pacific Standard Time");
                //order.postDate = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, myTimeZone);

                return DbMngmt<ORDR>.Add(order).id;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static void Update(ORDR order)
        {           
            try
            {
                DbMngmt<ORDR>.Update(order);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static void AddLine(RDR1 line)
        {
            try
            {
                DbMngmt<RDR1>.Add(line);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static void Delete(ORDR order)
        {            
            try
            {
                DeleteQueue(order.id);

                List<RDR1> orderLines = GetLinesList(order.id).ToList();

                foreach (RDR1 item in orderLines)
                {
                    DeleteLine(item);
                }

                DbMngmt<ORDR>.Remove(order);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static void DeleteLine(RDR1 orderLine)
        {
            try
            {
                //RDR1 line = ec.GetSingle(x => x.orderId.Equals(orderLine.orderId) && x.itemCode == orderLine.itemCode);
                //RDR1 line = ec.executeSqlQuerySingle(string.Format("select * from rdr1 where orderId = {0} itemCode = '{1}' and whsCode = '{2}' ", orderLine.))
                DbMngmt<RDR1>.Remove(orderLine);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static void DeleteQueue(int id)
        {
            ProcessQueue queue = BizProcessQueue.GetSingle(id.ToString());

            if (queue != null)
                BizProcessQueue.Remove(queue);
        }
    }
}
