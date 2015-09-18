using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Orkidea.Framework.SAP.BusinessOne.DiApiClient;
using Orkidea.Framework.SAP.BusinessOne.DiApiClient.SecurityData;
using Orkidea.Framework.SAP.BusinessOne.Entities.Global.ExceptionManagement;
using Orkidea.Framework.SAP.BusinessOne.Entities.MarketingDocuments;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orkidea.Bretano.WebMiddle.BackEnd.Business
{
    public class BizPurchaseOrder 
    {
        #region Attributes
        /// <summary>
        /// Permite el acceso módulo de socio de negocios
        /// </summary>
        private MarketingDocumentData PurchaseOrderAccess;
        public SAPConnectionData DataConnection;
        #endregion

        #region Constructor
        public BizPurchaseOrder()
        {
            DataConnection = new SAPConnectionData();
        }
        #endregion

        #region Methods
        public bool Add(MarketingDocument document) 
        {
            try
            {
                //if (!Util.ValidarDatosAccesoServicio(conexion))
                //    throw new BusinessException(15, "Nombre de Usuario o Contraseña incorrecta para el Servicio");

                PurchaseOrderAccess = new MarketingDocumentData();
                PurchaseOrderAccess.Add(MktgDocType.PurchaseOrder, document);

                return true;
            }
            catch (DbException ex)
            {
                Exception outEx;
                if (ExceptionPolicy.HandleException(ex, "Politica_SQLServer", out outEx))
                {
                    outEx.Data.Add("1", "14");
                    outEx.Data.Add("2", "NA");
                    outEx.Data.Add("3", outEx.Message + " Descripción: " + ex.Message);
                    throw outEx;
                }
                else
                {
                    throw ex;
                }
            }
            catch (BusinessException ex)
            {
                BizUtilities.ProcessBusinessException(ex);
            }
            catch (Exception ex)
            {
                Exception outEx;
                if (ExceptionPolicy.HandleException(ex, "Politica_ExcepcionGenerica", out outEx))
                {
                    outEx.Data.Add("1", "3");
                    outEx.Data.Add("2", "NA");
                    outEx.Data.Add("3", outEx.Message);
                    throw outEx;
                }
                else
                {
                    throw ex;
                }
            }
            return false;
        }
        #endregion
    }
}
