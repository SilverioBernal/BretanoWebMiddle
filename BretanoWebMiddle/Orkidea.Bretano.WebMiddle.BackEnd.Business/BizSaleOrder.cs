using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Orkidea.Framework.SAP.BusinessOne.DiApiClient;
using Orkidea.Framework.SAP.BusinessOne.DiServer;
using Orkidea.Framework.SAP.BusinessOne.DiApiClient.SecurityData;
using Orkidea.Framework.SAP.BusinessOne.Entities.Global.Administration;
using Orkidea.Framework.SAP.BusinessOne.Entities.Global.ExceptionManagement;
using Orkidea.Framework.SAP.BusinessOne.Entities.Global.Misc;
using Orkidea.Framework.SAP.BusinessOne.Entities.Global.UserDefinedFileds;
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
    public class BizSaleOrder
    {
        #region Attributes
        /// <summary>
        /// Permite el acceso módulo de socio de negocios
        /// </summary>
        private MarketingDocumentData SaleOrderAccess;

        public SAPConnectionData DataConnection;
        #endregion

        #region Constructor
        public BizSaleOrder()
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
                    SaleOrderAccess = new MarketingDocumentData(oAppConnData.adoConnString);

                    UserDefinedField webDocument = document.userDefinedFields.Where(x => x.name == "U_orkWebDocument").FirstOrDefault();

                    if (webDocument != null)
                        if (!SaleOrderAccess.ExistWebDocument(SapDocumentType.SalesOrder, webDocument.value))
                        {
                            DataConnection.BeginTran();
                            document = SaleOrderAccess.Add(SapDocumentType.SalesOrder, document, DataConnection.Conn);
                            DataConnection.EndTranAndRelease(BoWfTransOpt.wf_Commit);
                        }
                        else
                        {
                            int docEntry = SaleOrderAccess.GetDocEntry(SapDocumentType.SalesOrder, int.Parse(webDocument.value));
                            document.docEntry = docEntry;
                        }
                    return document;
                }

                //***** prueba DIServer *****//
                //OrderAccess = new DocumentData(oAppConnData.dataBaseName, licenseServer, dbServer, oAppConnData.sapUser, oAppConnData.sapUserPassword, dbUser, dbUserPassword, serverType);
                //OrderAccess.Add(SapDocumentType.SalesOrder, document);
            }
            #region Catch
            catch (SAPException ex)
            {
                DataConnection.EndTranAndRelease(SAPbobsCOM.BoWfTransOpt.wf_RollBack);
                BizUtilities.ProcessSapException(ex, "Gestión de Pagos");

            }
            catch (COMException ex)
            {
                string[] error = ex.Message.Split(' ');

                if (error.Length == 2)
                    if (error[0] == "(-7)")
                    {
                        document.docEntry = Convert.ToInt32(error[1]);
                        return document;
                    }

                DataConnection.EndTranAndRelease(SAPbobsCOM.BoWfTransOpt.wf_RollBack);
                Exception outEx;
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
                        outEx.Data.Add("3", outEx.Message + " Descripción: " + ex.Message);
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

        public List<MarketingDocument> Add(List<MarketingDocument> documents, AppConnData oAppConnData)
        {
            if (!BizUtilities.ValidateServiceConnection(oAppConnData))
                throw new BusinessException(15, "Nombre de Usuario o Contraseña incorrecta para el Servicio");

            oAppConnData = BizUtilities.GetDataConnection(oAppConnData);

            string licenseServer = Cryptography.Decrypt(HexSerialization.HexToString(ConfigurationManager.AppSettings["licenseServer"]));
            string dbServer = Cryptography.Decrypt(HexSerialization.HexToString(ConfigurationManager.AppSettings["dbServer"]));
            string dbUser = Cryptography.Decrypt(HexSerialization.HexToString(ConfigurationManager.AppSettings["dbUser"]));
            string dbUserPassword = Cryptography.Decrypt(HexSerialization.HexToString(ConfigurationManager.AppSettings["dbUserPassword"]));
            string serverType = ConfigurationManager.AppSettings["serverType"];
            bool dataConnection = false;

            #region Sap Connection
            try
            {
                DataConnection = new SAPConnectionData(oAppConnData.dataBaseName, licenseServer, dbServer, oAppConnData.sapUser, oAppConnData.sapUserPassword, dbUser, dbUserPassword, serverType);
                dataConnection = DataConnection.ConnectCompany(oAppConnData.dataBaseName, oAppConnData.sapUser, oAppConnData.sapUserPassword);
            }
            #region Catch
            catch (SAPException ex)
            {
                BizUtilities.ProcessSapException(ex, "Gestión de Pagos");
            }
            catch (COMException ex)
            {
                string[] error = ex.Message.Split(' ');

                Exception outEx;
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
                Exception outEx;
                if (ex.Data["1"] == null)
                {
                    if (ExceptionPolicy.HandleException(ex, "Politica_ExcepcionGenerica", out outEx))
                    {
                        outEx.Data.Add("1", "3");
                        outEx.Data.Add("2", "NA");
                        outEx.Data.Add("3", outEx.Message + " Descripción: " + ex.Message);
                        throw outEx;

                    }
                }
                else
                {
                    throw ex;
                }
                throw;
            }
            #endregion
            #endregion

            if (dataConnection)
            {
                SaleOrderAccess = new MarketingDocumentData(oAppConnData.adoConnString);

                foreach (MarketingDocument document in documents)
                {
                    #region Order processing
                    try
                    {
                        UserDefinedField webDocument = document.userDefinedFields.Where(x => x.name == "U_orkWebDocument").FirstOrDefault();
                        if (webDocument != null)
                            if (!SaleOrderAccess.ExistWebDocument(SapDocumentType.SalesOrder, webDocument.value))
                            {
                                DataConnection.BeginTran();
                                document.docEntry = SaleOrderAccess.Add(SapDocumentType.SalesOrder, document, DataConnection.Conn).docEntry;
                                DataConnection.EndTran(BoWfTransOpt.wf_Commit);
                                System.Threading.Thread.Sleep(3000);
                            }
                            else
                            {
                                int docEntry = SaleOrderAccess.GetDocEntry(SapDocumentType.SalesOrder, int.Parse(webDocument.value));
                                document.docEntry = docEntry;
                            }
                    }
                    #region Catch
                    catch (SAPException ex)
                    {
                        document.transactionInformation = string.Format("Sap Exception: {1}", ex.Message);
                        DataConnection.EndTran(SAPbobsCOM.BoWfTransOpt.wf_RollBack);
                    }
                    catch (COMException ex)
                    {
                        string[] error = ex.Message.Split(' ');
                        document.transactionInformation = string.Format("COM Exception: {1}", ex.Message);

                        if (error.Length == 2)
                            if (error[0] == "(-7)")
                            {
                                document.docEntry = Convert.ToInt32(error[1]);
                                document.transactionInformation = string.Empty;
                            }

                        DataConnection.EndTran(SAPbobsCOM.BoWfTransOpt.wf_RollBack);
                    }
                    catch (DbException ex)
                    {
                        document.transactionInformation = string.Format("DataBase Exception: {1}", ex.Message);
                    }
                    catch (BusinessException ex)
                    {
                        document.transactionInformation = string.Format("Business Exception: {1}", ex.Message);
                        DataConnection.EndTran(SAPbobsCOM.BoWfTransOpt.wf_RollBack);
                    }
                    catch (Exception ex)
                    {
                        document.transactionInformation = string.Format("Generic Exception: {1}", ex.Message);
                        DataConnection.EndTran(SAPbobsCOM.BoWfTransOpt.wf_RollBack);
                    }
                    #endregion
                    #endregion
                }
            }

            DataConnection.EndTranAndRelease(BoWfTransOpt.wf_Commit);
            System.Threading.Thread.Sleep(3000);
            return documents;
        }

        public List<LightMarketingDocument> List(DateTime startDate, DateTime endDate, string cardCode, AppConnData oAppConnData)
        {
            try
            {
                if (!BizUtilities.ValidateServiceConnection(oAppConnData))
                    throw new BusinessException(15, "Nombre de Usuario o Contraseña incorrecta para el Servicio");

                oAppConnData = BizUtilities.GetDataConnection(oAppConnData);
                SaleOrderAccess = new MarketingDocumentData(oAppConnData.adoConnString);

                List<LightMarketingDocument> orders = new List<LightMarketingDocument>();

                if (!string.IsNullOrEmpty(cardCode))
                    orders = SaleOrderAccess.GetList(SapDocumentType.SalesOrder, startDate, endDate, cardCode);
                else
                    orders = SaleOrderAccess.GetList(SapDocumentType.SalesOrder, startDate, endDate);

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

        public List<LightMarketingDocument> List(DateTime startDate, DateTime endDate, char fieldFilter, string slp_card_Code, AppConnData oAppConnData)
        {
            try
            {
                if (!BizUtilities.ValidateServiceConnection(oAppConnData))
                    throw new BusinessException(15, "Nombre de Usuario o Contraseña incorrecta para el Servicio");

                oAppConnData = BizUtilities.GetDataConnection(oAppConnData);
                SaleOrderAccess = new MarketingDocumentData(oAppConnData.adoConnString);

                List<LightMarketingDocument> orders = new List<LightMarketingDocument>();

                orders = SaleOrderAccess.GetList(SapDocumentType.SalesOrder, startDate, endDate, fieldFilter, slp_card_Code);

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

                LightMarketingDocument order = SaleOrderAccess.GetSingle(SapDocumentType.SalesOrder, docNum);

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

        public int GetOrderNum(int docEntry, AppConnData oAppConnData)
        {
            try
            {
                if (!BizUtilities.ValidateServiceConnection(oAppConnData))
                    throw new BusinessException(15, "Nombre de Usuario o Contraseña incorrecta para el Servicio");

                oAppConnData = BizUtilities.GetDataConnection(oAppConnData);
                SaleOrderAccess = new MarketingDocumentData(oAppConnData.adoConnString);

                return SaleOrderAccess.GetDocNum(SapDocumentType.SalesOrder, docEntry); ;
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
            return -1;
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
                    SaleOrderAccess.Cancel(SapDocumentType.SalesOrder, docEntry, DataConnection.Conn);
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

        public List<MarketingDocument> Cancel(List<MarketingDocument> documents, AppConnData oAppConnData)
        {
            if (!BizUtilities.ValidateServiceConnection(oAppConnData))
                throw new BusinessException(15, "Nombre de Usuario o Contraseña incorrecta para el Servicio");

            oAppConnData = BizUtilities.GetDataConnection(oAppConnData);

            string licenseServer = Cryptography.Decrypt(HexSerialization.HexToString(ConfigurationManager.AppSettings["licenseServer"]));
            string dbServer = Cryptography.Decrypt(HexSerialization.HexToString(ConfigurationManager.AppSettings["dbServer"]));
            string dbUser = Cryptography.Decrypt(HexSerialization.HexToString(ConfigurationManager.AppSettings["dbUser"]));
            string dbUserPassword = Cryptography.Decrypt(HexSerialization.HexToString(ConfigurationManager.AppSettings["dbUserPassword"]));
            string serverType = ConfigurationManager.AppSettings["serverType"];
            bool dataConnection = false;

            #region Sap Connection
            try
            {
                DataConnection = new SAPConnectionData(oAppConnData.dataBaseName, licenseServer, dbServer, oAppConnData.sapUser, oAppConnData.sapUserPassword, dbUser, dbUserPassword, serverType);
                dataConnection = DataConnection.ConnectCompany(oAppConnData.dataBaseName, oAppConnData.sapUser, oAppConnData.sapUserPassword);
            }
            #region Catch
            catch (SAPException ex)
            {
                BizUtilities.ProcessSapException(ex, "Gestión de Pagos");
            }
            catch (COMException ex)
            {
                string[] error = ex.Message.Split(' ');

                Exception outEx;
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
                Exception outEx;
                if (ex.Data["1"] == null)
                {
                    if (ExceptionPolicy.HandleException(ex, "Politica_ExcepcionGenerica", out outEx))
                    {
                        outEx.Data.Add("1", "3");
                        outEx.Data.Add("2", "NA");
                        outEx.Data.Add("3", outEx.Message + " Descripción: " + ex.Message);
                        throw outEx;

                    }
                }
                else
                {
                    throw ex;
                }
                throw;
            }
            #endregion
            #endregion

            if (dataConnection)
            {
                foreach (MarketingDocument document in documents)
                {
                    #region Cancel processing
                    try
                    {
                        DataConnection.BeginTran();
                        SaleOrderAccess = new MarketingDocumentData(oAppConnData.adoConnString);
                        SaleOrderAccess.Cancel(SapDocumentType.SalesOrder, document, DataConnection.Conn);
                        DataConnection.EndTran(BoWfTransOpt.wf_Commit);
                        document.transactionInformation = "Cancelación exitosa";
                        System.Threading.Thread.Sleep(3000);
                    }
                    #region Catch
                    catch (SAPException ex)
                    {
                        document.transactionInformation = string.Format("Sap Exception: {1}", ex.Message);
                        DataConnection.EndTran(SAPbobsCOM.BoWfTransOpt.wf_RollBack);
                    }
                    catch (COMException ex)
                    {
                        string[] error = ex.Message.Split(' ');
                        document.transactionInformation = string.Format("COM Exception: {1}", ex.Message);

                        DataConnection.EndTran(SAPbobsCOM.BoWfTransOpt.wf_RollBack);
                    }
                    catch (DbException ex)
                    {
                        document.transactionInformation = string.Format("DataBase Exception: {1}", ex.Message);
                    }
                    catch (BusinessException ex)
                    {
                        document.transactionInformation = string.Format("Business Exception: {1}", ex.Message);
                        DataConnection.EndTran(SAPbobsCOM.BoWfTransOpt.wf_RollBack);
                    }
                    catch (Exception ex)
                    {
                        document.transactionInformation = string.Format("Generic Exception: {1}", ex.Message);
                        DataConnection.EndTran(SAPbobsCOM.BoWfTransOpt.wf_RollBack);
                    }
                    #endregion
                    #endregion
                }
            }

            DataConnection.EndTranAndRelease(BoWfTransOpt.wf_Commit);
            System.Threading.Thread.Sleep(3000);
            return documents;
        }

        public List<MarketingDocument> ProcessBatchTransaction(List<MarketingDocument> documents, AppConnData oAppConnData)
        {
            if (!BizUtilities.ValidateServiceConnection(oAppConnData))
                throw new BusinessException(15, "Nombre de Usuario o Contraseña incorrecta para el Servicio");

            oAppConnData = BizUtilities.GetDataConnection(oAppConnData);

            string licenseServer = Cryptography.Decrypt(HexSerialization.HexToString(ConfigurationManager.AppSettings["licenseServer"]));
            string dbServer = Cryptography.Decrypt(HexSerialization.HexToString(ConfigurationManager.AppSettings["dbServer"]));
            string dbUser = Cryptography.Decrypt(HexSerialization.HexToString(ConfigurationManager.AppSettings["dbUser"]));
            string dbUserPassword = Cryptography.Decrypt(HexSerialization.HexToString(ConfigurationManager.AppSettings["dbUserPassword"]));
            string serverType = ConfigurationManager.AppSettings["serverType"];
            bool dataConnection = false;

            #region Sap Connection
            try
            {
                DataConnection = new SAPConnectionData(oAppConnData.dataBaseName, licenseServer, dbServer, oAppConnData.sapUser, oAppConnData.sapUserPassword, dbUser, dbUserPassword, serverType);
                dataConnection = DataConnection.ConnectCompany(oAppConnData.dataBaseName, oAppConnData.sapUser, oAppConnData.sapUserPassword);
            }
            #region Catch
            catch (SAPException ex)
            {
                BizUtilities.ProcessSapException(ex, "Gestión de Pagos");
            }
            catch (COMException ex)
            {
                string[] error = ex.Message.Split(' ');

                Exception outEx;
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
                Exception outEx;
                if (ex.Data["1"] == null)
                {
                    if (ExceptionPolicy.HandleException(ex, "Politica_ExcepcionGenerica", out outEx))
                    {
                        outEx.Data.Add("1", "3");
                        outEx.Data.Add("2", "NA");
                        outEx.Data.Add("3", outEx.Message + " Descripción: " + ex.Message);
                        throw outEx;

                    }
                }
                else
                {
                    throw ex;
                }
                throw;
            }
            #endregion
            #endregion

            if (dataConnection)
            {
                foreach (MarketingDocument document in documents)
                {
                    #region Processing document
                    try
                    {
                        DataConnection.BeginTran();
                        SaleOrderAccess = new MarketingDocumentData(oAppConnData.adoConnString);

                        string tipo = "";

                        switch (document.actionType)
                        {
                            case ActionType.Add:
                                tipo = "Generación de orden de venta";
                                document.docEntry = SaleOrderAccess.Add(SapDocumentType.SalesOrder, document, DataConnection.Conn).docEntry;
                                break;
                            case ActionType.Cancel:
                                tipo = "Cancelación de orden de venta";
                                SaleOrderAccess.Cancel(SapDocumentType.SalesOrder, document, DataConnection.Conn);
                                break;
                            default:
                                break;
                        }

                        DataConnection.EndTran(BoWfTransOpt.wf_Commit);
                        document.transactionInformation = string.Format("{0} :: {1} exitosa", DateTime.Now.ToString("yyyy-MM-dd"), tipo);
                        System.Threading.Thread.Sleep(3000);
                    }
                    #region Catch
                    catch (SAPException ex)
                    {
                        document.transactionInformation = string.Format("Error SAP: {0}", ex.Description);
                        DataConnection.EndTran(SAPbobsCOM.BoWfTransOpt.wf_RollBack);
                    }
                    catch (COMException ex)
                    {
                        string[] error = ex.Message.Split(' ');
                        document.transactionInformation = string.Format("Error COM: {0}", ex.Message);

                        if (document.actionType == ActionType.Add)
                            if (error.Length == 2)
                                if (error[0] == "(-7)")
                                {
                                    document.docEntry = Convert.ToInt32(error[1]);
                                    document.transactionInformation = string.Format("{0} :: {1} exitosa", DateTime.Now.ToString("yyyy-MM-dd"), "Generación de orden de venta");
                                }

                        DataConnection.EndTran(SAPbobsCOM.BoWfTransOpt.wf_RollBack);
                    }
                    catch (DbException ex)
                    {
                        document.transactionInformation = string.Format("Error Base de datos: {1}", ex.Message);
                    }
                    catch (BusinessException ex)
                    {
                        document.transactionInformation = string.Format("Error Negocio: {1}", ex.Message);
                        DataConnection.EndTran(SAPbobsCOM.BoWfTransOpt.wf_RollBack);
                    }
                    catch (Exception ex)
                    {
                        document.transactionInformation = string.Format("Error Generico: {1}", ex.Message);
                        DataConnection.EndTran(SAPbobsCOM.BoWfTransOpt.wf_RollBack);
                    }
                    #endregion
                    #endregion
                }
            }

            DataConnection.EndTranAndRelease(BoWfTransOpt.wf_Commit);
            System.Threading.Thread.Sleep(3000);
            return documents;
        }
        #endregion
    }
}
