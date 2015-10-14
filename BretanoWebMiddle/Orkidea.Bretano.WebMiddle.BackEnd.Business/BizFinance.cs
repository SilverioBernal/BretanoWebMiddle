using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Orkidea.Framework.SAP.BusinessOne.DiApiClient;
using Orkidea.Framework.SAP.BusinessOne.DiApiClient.SecurityData;
using Orkidea.Framework.SAP.BusinessOne.Entities.Finance;
using Orkidea.Framework.SAP.BusinessOne.Entities.Global.ExceptionManagement;
using Orkidea.Framework.SAP.BusinessOne.Entities.Global.Misc;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orkidea.Bretano.WebMiddle.BackEnd.Business
{
    public class BizFinance
    {
        #region Attributes
        /// <summary>
        /// Permite el acceso módulo de socio de negocios
        /// </summary>
        private FinanceData FinanceAccess;
        public SAPConnectionData DataConnection;
        #endregion

        #region Constructor
        public BizFinance()
        {
            DataConnection = new SAPConnectionData();
        }
        #endregion

        #region Methods
        public List<SalesTaxCode> GetSalesTaxCodeList(AppConnData oAppConnData)
        {
            try
            {
                if (!BizUtilities.ValidateServiceConnection(oAppConnData))
                    throw new BusinessException(15, "Nombre de Usuario o Contraseña incorrecta para el Servicio");

                oAppConnData = BizUtilities.GetDataConnection(oAppConnData);

                FinanceAccess = new FinanceData(oAppConnData.adoConnString);
                return FinanceAccess.GetSalesTaxCodeList();
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
            return null;
        }

        public List<WithholdingTax> GetWithholdingTax(AppConnData oAppConnData)
        {
            try
            {
                if (!BizUtilities.ValidateServiceConnection(oAppConnData))
                    throw new BusinessException(15, "Nombre de Usuario o Contraseña incorrecta para el Servicio");

                oAppConnData = BizUtilities.GetDataConnection(oAppConnData);

                FinanceAccess = new FinanceData(oAppConnData.adoConnString);
                return FinanceAccess.GetWithholdingTax();
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
            return null;
        }
        #endregion
    }
}
