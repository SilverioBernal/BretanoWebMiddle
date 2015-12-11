using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Orkidea.Framework.SAP.BusinessOne.DiApiClient;
using Orkidea.Framework.SAP.BusinessOne.DiApiClient.SecurityData;
using Orkidea.Framework.SAP.BusinessOne.Entities.Global.Administration;
using Orkidea.Framework.SAP.BusinessOne.Entities.Global.ExceptionManagement;
using Orkidea.Framework.SAP.BusinessOne.Entities.Global.Misc;
using Orkidea.Framework.SAP.BusinessOne.Entities.MarketingDocuments;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orkidea.Bretano.WebMiddle.BackEnd.Business
{
    public class BizManagement
    {
        #region Attributes
        /// <summary>
        /// Permite el acceso módulo de socio de negocios
        /// </summary>
        private ManagementData ManagementAccess;
        public SAPConnectionData DataConnection;
        #endregion

        #region Constructor
        public BizManagement()
        {
            DataConnection = new SAPConnectionData();
        }
        #endregion

        #region Methods        
        public List<AuthorizationStatus> List(DateTime startDate, DateTime endDate, AppConnData oAppConnData)
        {
            try
            {
                if (!BizUtilities.ValidateServiceConnection(oAppConnData))
                    throw new BusinessException(15, "Nombre de Usuario o Contraseña incorrecta para el Servicio");

                oAppConnData = BizUtilities.GetDataConnection(oAppConnData);
                ManagementAccess = new ManagementData(oAppConnData.adoConnString);

                List<AuthorizationStatus> authorizationStatusList = ManagementAccess.GetAuthorizationStatusReport("", "", "", "", "", "", startDate, endDate, "", "", "", "", "Y", "N", "N", "N", "N", "", "17", 1, "ODRF", 0);


                return authorizationStatusList;
            }
            catch (DbException ex)
            {
                Exception outEx;
                if (ExceptionPolicy.HandleException(ex, "Politica_SQLServer", out outEx))
                {
                    outEx.Data.Add("1", "14");
                    outEx.Data.Add("2", "NA");
                    //outEx.Data.Add("3", outEx.Message);
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
            return null;
        }
        #endregion
    }
}
