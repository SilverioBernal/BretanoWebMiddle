using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Orkidea.Framework.SAP.BusinessOne.DiApiClient;
using Orkidea.Framework.SAP.BusinessOne.DiApiClient.SecurityData;
using Orkidea.Framework.SAP.BusinessOne.Entities.Global.Administration;
using Orkidea.Framework.SAP.BusinessOne.Entities.Global.ExceptionManagement;
using Orkidea.Framework.SAP.BusinessOne.Entities.Global.Misc;
using Orkidea.Framework.SAP.BusinessOne.Entities.MarketingDocuments;
using Orkidea.Framework.Security;
using SAPbobsCOM;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Common;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Orkidea.Bretano.WebMiddle.BackEnd.Business
{
    public class BizQuotation
    {
        #region Attributes
        /// <summary>
        /// Permite el acceso módulo de socio de negocios
        /// </summary>
        private MarketingDocumentData SaleOrderAccess;
        public SAPConnectionData DataConnection;
        #endregion

        #region Constructor
        public BizQuotation()
        {
            DataConnection = new SAPConnectionData();
        }
        #endregion

        #region Methods
        public MarketingDocument Add(MarketingDocument document, AppConnData oAppConnData)
        {
            try
            {
                if (!BizUtilities.ValidateServiceConnection(oAppConnData))
                    throw new BusinessException(15, "Nombre de Usuario o Contraseña incorrecta para el Servicio");

                oAppConnData = BizUtilities.GetDataConnection(oAppConnData);

                string licenseServer = Cryptography.Decrypt(HexSerialization.HexToString(ConfigurationManager.AppSettings["licenseServer"]));
                string dbServer = Cryptography.Decrypt(HexSerialization.HexToString(ConfigurationManager.AppSettings["dbServer"]));
                string dbUser = Cryptography.Decrypt(HexSerialization.HexToString(ConfigurationManager.AppSettings["dbUser"]));
                string dbUserPassword = Cryptography.Decrypt(HexSerialization.HexToString(ConfigurationManager.AppSettings["dbUserPassword"]));
                string serverType = ConfigurationManager.AppSettings["serverType"];

                DataConnection = new SAPConnectionData(oAppConnData.dataBaseName, licenseServer, dbServer, oAppConnData.sapUser, oAppConnData.sapUserPassword, dbUser, dbUserPassword, serverType);

                if (DataConnection.ConnectCompany(oAppConnData.dataBaseName, oAppConnData.sapUser, oAppConnData.sapUserPassword))
                {
                    DataConnection.BeginTran();
                    SaleOrderAccess = new MarketingDocumentData(oAppConnData.adoConnString);
                    document = SaleOrderAccess.Add(SapDocumentType.Quotation, document, DataConnection.Conn);
                    DataConnection.EndTranAndRelease(BoWfTransOpt.wf_Commit);
                    return document;
                }


            }
            #region Catch
            catch (SAPException ex)
            {
                DataConnection.EndTranAndRelease(SAPbobsCOM.BoWfTransOpt.wf_RollBack);
                BizUtilities.ProcessSapException(ex, "Gestión de Pagos");
                
            }
            catch (COMException ex)
            {
                DataConnection.EndTranAndRelease(SAPbobsCOM.BoWfTransOpt.wf_RollBack);
                Exception outEx;
                try
                {
                    if (ExceptionPolicy.HandleException(ex, "Politica_Excepcion_Com", out outEx))
                    {
                        outEx.Data.Add("1", "3");
                        outEx.Data.Add("2", "NA");
                        outEx.Data.Add("3", outEx.Message + " Descripción: " + ex.Message);
                        throw outEx;
                    }
                    else
                    {
                        throw;
                    }
                }
                catch (Exception)
                {
                }

                throw new Exception(ex.Message + "::" + ex.StackTrace);
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
                ex.Data.Add("1", ex.ErrorId);
                ex.Data.Add("2", "NA");
                ex.Data.Add("3", ex.Message);
                throw ex;
            }
            catch (Exception ex)
            {
                DataConnection.EndTranAndRelease(SAPbobsCOM.BoWfTransOpt.wf_RollBack);
                Exception outEx;
                if (ex.Data["1"] == null)
                {
                    if (ExceptionPolicy.HandleException(ex, "Politica_ExcepcionGenerica", out outEx))
                    {
                        outEx.Data.Add("1", "3");
                        outEx.Data.Add("2", "NA");
                        outEx.Data.Add("3", outEx.Message);
                        throw outEx;

                    }
                }
                else
                {
                    throw ex;
                    //return 0;
                }
                throw;
            }
            #endregion
            return document;
        }

        public List<LightMarketingDocument> List(DateTime startDate, DateTime endDate, string cardCode, AppConnData oAppConnData)
        {
            try
            {
                if (!BizUtilities.ValidateServiceConnection(oAppConnData))
                    throw new BusinessException(15, "Nombre de Usuario o Contraseña incorrecta para el Servicio");

                oAppConnData = BizUtilities.GetDataConnection(oAppConnData);
                SaleOrderAccess = new MarketingDocumentData(oAppConnData.adoConnString);

                List<LightMarketingDocument> orders = SaleOrderAccess.GetList(SapDocumentType.Quotation,startDate, endDate, cardCode);

                foreach (LightMarketingDocument item in orders)
                {
                    if (item.docStatus == "O")
                        item.docStatus = "Abierto";
                    else
                        item.docStatus = "Cerrado";
                }

                return orders;
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

        public LightMarketingDocument GetSingle(string docNum, AppConnData oAppConnData)
        {
            try
            {
                if (!BizUtilities.ValidateServiceConnection(oAppConnData))
                    throw new BusinessException(15, "Nombre de Usuario o Contraseña incorrecta para el Servicio");

                oAppConnData = BizUtilities.GetDataConnection(oAppConnData);
                SaleOrderAccess = new MarketingDocumentData(oAppConnData.adoConnString);

                LightMarketingDocument order = SaleOrderAccess.GetSingle(SapDocumentType.Quotation, docNum);

                if (order.docStatus == "O")
                    order.docStatus = "Abierto";
                else
                    order.docStatus = "Cerrado";

                return order;
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

        public void Cancel(int docEntry, AppConnData oAppConnData)
        {
            try
            {
                if (!BizUtilities.ValidateServiceConnection(oAppConnData))
                    throw new BusinessException(15, "Nombre de Usuario o Contraseña incorrecta para el Servicio");

                oAppConnData = BizUtilities.GetDataConnection(oAppConnData);

                string licenseServer = Cryptography.Decrypt(HexSerialization.HexToString(ConfigurationManager.AppSettings["licenseServer"]));
                string dbServer = Cryptography.Decrypt(HexSerialization.HexToString(ConfigurationManager.AppSettings["dbServer"]));
                string dbUser = Cryptography.Decrypt(HexSerialization.HexToString(ConfigurationManager.AppSettings["dbUser"]));
                string dbUserPassword = Cryptography.Decrypt(HexSerialization.HexToString(ConfigurationManager.AppSettings["dbUserPassword"]));
                string serverType = ConfigurationManager.AppSettings["serverType"];

                DataConnection = new SAPConnectionData(oAppConnData.dataBaseName, licenseServer, dbServer, oAppConnData.sapUser, oAppConnData.sapUserPassword, dbUser, dbUserPassword, serverType);

                if (DataConnection.ConnectCompany(oAppConnData.dataBaseName, oAppConnData.sapUser, oAppConnData.sapUserPassword))
                {
                    DataConnection.BeginTran();
                    SaleOrderAccess = new MarketingDocumentData(oAppConnData.adoConnString);
                    SaleOrderAccess.Cancel(SapDocumentType.Quotation, docEntry, DataConnection.Conn);
                    DataConnection.EndTranAndRelease(BoWfTransOpt.wf_Commit);                    
                }
            }
            #region Catch
            catch (SAPException ex)
            {
                DataConnection.EndTranAndRelease(SAPbobsCOM.BoWfTransOpt.wf_RollBack);
                BizUtilities.ProcessSapException(ex, "Gestión de Pagos");

            }
            catch (COMException ex)
            {
                DataConnection.EndTranAndRelease(SAPbobsCOM.BoWfTransOpt.wf_RollBack);
                Exception outEx;
                try
                {
                    if (ExceptionPolicy.HandleException(ex, "Politica_Excepcion_Com", out outEx))
                    {
                        outEx.Data.Add("1", "3");
                        outEx.Data.Add("2", "NA");
                        outEx.Data.Add("3", outEx.Message + " Descripción: " + ex.Message);
                        throw outEx;
                    }
                    else
                    {
                        throw;
                    }
                }
                catch (Exception)
                {
                }

                throw new Exception(ex.Message + "::" + ex.StackTrace);
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
                ex.Data.Add("1", ex.ErrorId);
                ex.Data.Add("2", "NA");
                ex.Data.Add("3", ex.Message);
                throw ex;
            }
            catch (Exception ex)
            {
                DataConnection.EndTranAndRelease(SAPbobsCOM.BoWfTransOpt.wf_RollBack);
                Exception outEx;
                if (ex.Data["1"] == null)
                {
                    if (ExceptionPolicy.HandleException(ex, "Politica_ExcepcionGenerica", out outEx))
                    {
                        outEx.Data.Add("1", "3");
                        outEx.Data.Add("2", "NA");
                        outEx.Data.Add("3", outEx.Message);
                        throw outEx;

                    }
                }
                else
                {
                    throw ex;
                    //return 0;
                }
                throw;
            }
            #endregion            
        }
        #endregion
    }
}
