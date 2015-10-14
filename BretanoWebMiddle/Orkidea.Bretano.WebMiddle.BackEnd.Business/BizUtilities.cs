using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Orkidea.Framework.SAP.BusinessOne.DiApiClient;
using Orkidea.Framework.SAP.BusinessOne.Entities.Global.Administration;
using Orkidea.Framework.SAP.BusinessOne.Entities.Global.ExceptionManagement;
using Orkidea.Framework.SAP.BusinessOne.Entities.Global.Misc;
using Orkidea.Framework.Security;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orkidea.Bretano.WebMiddle.BackEnd.Business
{
    public static class BizUtilities
    {
        #region Exceptions
        /// <summary>
        /// Metodo para procesar las excepciones de tipo BALException
        /// </summary>
        /// <param name="ex">Excepcion ocurrida</param>
        public static void ProcessBusinessException(BusinessException ex)
        {
            ex.Data.Add("1", "16");
            ex.Data.Add("2", "NA");
            ex.Data.Add("3", ex.Message);
            throw ex;
        }

        /// <summary>
        /// Metodo para procesar las excepciones de tipo SAPException
        /// </summary>
        /// <param name="ex">Excepcion ocurrida</param>
        /// <param name="unProceso">Proceso que lanza la excepcion</param>
        public static void ProcessSapException(SAPException ex, string unProceso)
        {
            Exception outEx;
            switch (Math.Abs(ex.ErrorNumber))
            {
                case 103:
                    ExceptionPolicy.HandleException(ex, "Politica_ConexionBdSAP", out outEx);
                    outEx.Data.Add("1", "13");
                    outEx.Data.Add("2", "103");
                    outEx.Data.Add("3", outEx.Message);
                    break;
                case 104:
                    ExceptionPolicy.HandleException(ex, "Politica_ConexionLicenciaSAP", out outEx);
                    outEx.Data.Add("1", "14");
                    outEx.Data.Add("2", "104");
                    outEx.Data.Add("3", outEx.Message);
                    break;
                case 105:
                    ExceptionPolicy.HandleException(ex, "Politica_ObserverSAP", out outEx);
                    outEx.Data.Add("1", "15");
                    outEx.Data.Add("2", "105");
                    outEx.Data.Add("3", outEx.Message);
                    break;
                case 106:
                    ExceptionPolicy.HandleException(ex, "Politica_SinConexionSAP", out outEx);
                    outEx.Data.Add("1", "16");
                    outEx.Data.Add("2", "106");
                    outEx.Data.Add("3", outEx.Message);
                    break;
                case 107:
                    ExceptionPolicy.HandleException(ex, "Politica_AutenticacionSAP", out outEx);
                    outEx.Data.Add("1", "17");
                    outEx.Data.Add("2", "107");
                    outEx.Data.Add("3", outEx.Message);
                    break;
                case 109:
                    ExceptionPolicy.HandleException(ex, "Politica_CopiandoDllSAP", out outEx);
                    outEx.Data.Add("1", "18");
                    outEx.Data.Add("2", "109");
                    outEx.Data.Add("3", outEx.Message);
                    break;
                case 110:
                    ExceptionPolicy.HandleException(ex, "Politica_AbriendoObserverSAP", out outEx);
                    outEx.Data.Add("1", "19");
                    outEx.Data.Add("2", "110");
                    outEx.Data.Add("3", outEx.Message);
                    break;
                case 111:
                    ExceptionPolicy.HandleException(ex, "Politica_SBOCommonSAP", out outEx);
                    outEx.Data.Add("1", "20");
                    outEx.Data.Add("2", "111");
                    outEx.Data.Add("3", outEx.Message);
                    break;
                case 112:
                    ExceptionPolicy.HandleException(ex, "Politica_DllCabSAP", out outEx);
                    outEx.Data.Add("1", "21");
                    outEx.Data.Add("2", "112");
                    outEx.Data.Add("3", outEx.Message);
                    break;
                case 113:
                    ExceptionPolicy.HandleException(ex, "Politica_DirectorioTemporalSAP", out outEx);
                    outEx.Data.Add("1", "22");
                    outEx.Data.Add("2", "113");
                    outEx.Data.Add("3", outEx.Message);
                    break;
                default:
                    ExceptionPolicy.HandleException(ex, "Politica_ExcepcionSAP", out outEx);
                    outEx.Data.Add("1", "10");
                    outEx.Data.Add("2", "NA");
                    outEx.Data.Add("3", outEx.Message + " Número Error: " + ex.ErrorNumber + " Descripción: " + ex.Description);
                    break;
            }
            throw outEx;
        }

        public static bool ValidateServiceConnection(AppConnData appConnData)
        {
            string appKey = Cryptography.Decrypt(ConfigurationManager.AppSettings["AppKey"].ToString());
            string appSecret = Cryptography.Decrypt(ConfigurationManager.AppSettings["AppSecret"].ToString());

            string wsKey = Cryptography.Decrypt(HexSerialization.HexToString(appConnData.wsAppKey));
            string wsSecret = Cryptography.Decrypt(HexSerialization.HexToString(appConnData.wsSecret));

            if (appKey.Equals(wsKey) && appSecret.Equals(wsSecret))
                return true;
            else
                return false;
        }

        public static AppConnData GetDataConnection(AppConnData appConnData)
        {
            appConnData.dataBaseName = Cryptography.Decrypt(HexSerialization.HexToString(appConnData.dataBaseName));
            appConnData.sapUser = Cryptography.Decrypt(HexSerialization.HexToString(appConnData.sapUser));
            appConnData.sapUserPassword = Cryptography.Decrypt(HexSerialization.HexToString(appConnData.sapUserPassword));
            appConnData.adoConnString = Cryptography.Decrypt(HexSerialization.HexToString(appConnData.adoConnString));

            return appConnData;
        }
        #endregion        
    }
}
